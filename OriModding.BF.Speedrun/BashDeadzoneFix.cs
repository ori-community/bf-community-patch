using HarmonyLib;
using System.Collections.Generic;

namespace OriModding.BF.Speedrun;

internal class BashDeadzoneFix
{
    internal static void Patch(Harmony harmony)
    {
        // Required to patch internal class
        harmony.Patch(AccessTools.Method("BashAttackGame:FixedUpdate"), transpiler: new HarmonyMethod(typeof(BashDeadzoneFix), nameof(Transpiler)));
    }

    private static float GetBashDeadzone()
    {
        return Plugin.BashDeadzone.Value;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var i in instructions)
        {
            if (i.LoadsConstant(0.0400000028f))
                yield return CodeInstruction.Call(typeof(BashDeadzoneFix), nameof(GetBashDeadzone));
            else
                yield return i;
        }
    }
}