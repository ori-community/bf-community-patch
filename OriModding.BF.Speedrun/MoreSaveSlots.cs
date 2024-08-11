using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OriModding.BF.Speedrun;

internal static class MoreSaveSlots
{
    private const sbyte SaveSlots = 50;

    public static void Patch(Harmony harmony)
    {
        harmony.Patch(AccessTools.Method("SaveSlotsManager:Awake"), transpiler: new HarmonyMethod(AccessTools.Method("MoreSaveSlots:Transpiler")));
        harmony.Patch(AccessTools.Method("SaveSlotsManager:PrepareSlots"), transpiler: new HarmonyMethod(AccessTools.Method("MoreSaveSlots:Transpiler")));
        harmony.Patch(AccessTools.Method("SaveSlotsItemsUI:Awake"), transpiler: new HarmonyMethod(AccessTools.Method("MoreSaveSlots:Transpiler")));
        harmony.Patch(AccessTools.Method("SaveSlotsItemsUI:Refresh"), transpiler: new HarmonyMethod(AccessTools.Method("MoreSaveSlots:Transpiler")));
        harmony.Patch(AccessTools.Method("SaveSlotBackupsManager:ClearCache"), transpiler: new HarmonyMethod(AccessTools.Method("MoreSaveSlots:Transpiler")));
    }

    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var ci in instructions)
        {
            if (ci.opcode == OpCodes.Ldc_I4_S && (sbyte)ci.operand == 10)
                yield return new CodeInstruction(OpCodes.Ldc_I4_S, SaveSlots);
            else
                yield return ci;
        }
    }
}
