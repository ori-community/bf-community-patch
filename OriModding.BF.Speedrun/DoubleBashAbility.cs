using Core;
using HarmonyLib;
using OriModding.BF.Core.SeinAbilities;

namespace OriModding.BF.Speedrun;

public class DoubleBashAbility : CustomSeinAbility
{
    public static DoubleBashAbility Instance { get; private set; }

    public int frame = -1;
    public int maxFrames = 2;

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
        if (frame == -1)
        {
            // While holding bash, press <double bash button> after the initial minimum bash windup time
            if (Sein.Abilities.Bash.IsBashing && Sein.Abilities.Bash.m_bashAttackGame?.m_currentState == BashAttackGame.State.Playing && Plugin.DoubleBashInput.Value.OnPressed)
                frame = 0;
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
    }
}

[HarmonyPatch]
public static class BashInputSuppression
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
}
