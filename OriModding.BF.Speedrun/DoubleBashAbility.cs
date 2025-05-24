using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using OriModding.BF.Core.SeinAbilities;
using UnityEngine;
using Console = System.Console;
using Input = Core.Input;

namespace OriModding.BF.Speedrun;

public class DoubleBashAbility : CustomSeinAbility
{
    public static DoubleBashAbility Instance { get; private set; }

    public int frame = -1;
    public int maxFrames = 2;
    private bool blockDoubleBash = false;
    public bool shouldHighlight = false;

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        frame = -1;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (Instance == this)
            Instance = null;
    }

    public override bool AllowAbility(SeinLogicCycle logicCycle)
    {
        return Sein.PlayerAbilities.Bash.HasAbility;
    }

    public override void UpdateCharacterState()
    {
        // Stop double bash key working if you move the mouse
        if (Sein.Abilities.Bash.IsBashing && Sein.Abilities.Bash.m_bashAttackGame.m_mode == BashAttackGame.Modes.Mouse)
            blockDoubleBash = true;
        else if (!Sein.Abilities.Bash.IsBashing)
            blockDoubleBash = false;

        if (frame == -1)
        {
            // While holding bash, press <double bash button> after the initial minimum bash windup time
            if (!blockDoubleBash && Sein.Abilities.Bash.IsBashing && Sein.Abilities.Bash.m_bashAttackGame?.m_currentState == BashAttackGame.State.Playing && Plugin.DoubleBashInput.Value.OnPressed)
            {
                shouldHighlight = true;
                frame = 0;
            }
        }
        else
        {
            frame++;
            if (frame >= maxFrames)
                frame = -1;
        }
    }

    public override void Serialize(Archive ar)
    {
        ar.Serialize(ref frame);
        ar.Serialize(ref blockDoubleBash);
        ar.Serialize(ref shouldHighlight);
    }
}

[HarmonyPatch]
public static class DoubleBashPatches
{
    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.FixedUpdate))]
    static void SuppressBash()
    {
        // Runs after all input has been read
        // Override with our expected double bash inputs
        // i.e. release bash, then press it again on the following frame

        if (!DoubleBashAbility.Instance)
            return;

        int frame = DoubleBashAbility.Instance.frame;
        var bash = Input.Bash;

        switch (frame)
        {
            case 0:
                bash.Update(true);
                bash.Update(false);
                break;

            case 1:
                bash.Update(false);
                bash.Update(true);
                break;
        }
    }

    static void SetColour(Object unityObject)
    {
        Console.WriteLine("Setting the colour!");
        Console.WriteLine(DoubleBashAbility.Instance.shouldHighlight);

        var colour = DoubleBashAbility.Instance.shouldHighlight
            ? new Color(1f, 0f, 0f, 0.7373f)
            : new Color(0.4822f, 0.5228f, 0.5522f, 0.7373f);

        DoubleBashAbility.Instance.shouldHighlight = false;

        ((GameObject)unityObject).transform.Find("glowHolder/glow").GetComponent<MeshRenderer>().material.color = colour;
        ((GameObject)unityObject).transform.Find("glowHolder/glow").GetComponent<LegacyTransparancyAnimator>().m_colors = [colour];
    }

    [HarmonyTranspiler, HarmonyPatch(typeof(SeinBashAttack), nameof(SeinBashAttack.JumpOffTarget))]
    static IEnumerable<CodeInstruction> PatchBashReleaseEffectColour(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();

        for (int i = 0; i < instructionList.Count; i++)
        {
            if (instructionList[i].opcode == OpCodes.Stloc_S
                && instructionList[i - 1].opcode == OpCodes.Castclass
                && instructionList[i - 2].Calls(AccessTools.Method(typeof(InstantiateUtility), nameof(InstantiateUtility.Instantiate), [typeof(Object)]))
                && instructionList[i - 3].LoadsField(AccessTools.Field(typeof(SeinBashAttack), nameof(SeinBashAttack.BashReleaseEffect))))
            {
                yield return instructionList[i];

                yield return new CodeInstruction(OpCodes.Ldloc_S, instructionList[i].operand);
                yield return CodeInstruction.Call(typeof(DoubleBashPatches), nameof(SetColour));
            }
            else
            {
                yield return instructionList[i];
            }
        }
    }
}
