using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace OriModding.BF.Speedrun;

[HarmonyPatch(typeof(CleverMenuItemSelectionManager), nameof(CleverMenuItemSelectionManager.Start))]
public class PauseMenuWraparound_Awake
{
    public static void Postfix(CleverMenuItemSelectionManager __instance)
    {
        if (__instance.name == "inventoryScreen")
        {
            CleverMenuItem continueButton = __instance.MenuItems[0];
            CleverMenuItem exitButton = __instance.MenuItems[9];
            __instance.Navigation.Add(new CleverMenuItemSelectionManager.NavigationData
            {
                From = continueButton,
                To = exitButton
            });
            __instance.Navigation.Add(new CleverMenuItemSelectionManager.NavigationData
            {
                From = exitButton,
                To = continueButton
            });
        }
    }
}

[HarmonyPatch(typeof(CleverMenuItemSelectionManager), nameof(CleverMenuItemSelectionManager.ChangeMenuItem))]
public class PauseMenuWraparound_ChangeMenuItem
{
    public static Vector2 CalcDiff(CleverMenuItemSelectionManager instance, CleverMenuItemSelectionManager.NavigationData navData)
    {
        // Moves the resume (0) and quit (9) buttons relative to each other so they can be navigated to
        if (instance.name == "inventoryScreen")
        {
            if (instance.CurrentMenuItem == instance.MenuItems[0] && navData.To == instance.MenuItems[9])
                return new Vector2(0, 1f);
            if (instance.CurrentMenuItem == instance.MenuItems[9] && navData.To == instance.MenuItems[0])
                return new Vector2(0, -1f);
        }

        return (navData.To.Transform.position - instance.CurrentMenuItem.Transform.position).normalized;
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // Replaces calculation of direction to next menu item with CalcDiff
        var instructionList = instructions.ToList();

        var isVisibleMethod = AccessTools.PropertyGetter(typeof(CleverMenuItem), nameof(CleverMenuItem.IsVisible));

        bool skip = false;
        for (int i = 0; i < instructionList.Count; i++)
        {
            var instruction = instructionList[i];

            if (instruction.opcode == OpCodes.Brfalse && instructionList[i - 1].Calls(isVisibleMethod))
            {
                yield return instruction;
                yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                yield return new CodeInstruction(OpCodes.Ldloc_S, 4); // local variable (navigationData)
                yield return CodeInstruction.Call(typeof(PauseMenuWraparound_ChangeMenuItem), nameof(PauseMenuWraparound_ChangeMenuItem.CalcDiff)); // CalcDiff(this, navigationData)
                skip = true;
            }

            if (skip && instruction.IsStloc_S(7))
                skip = false;

            if (!skip)
                yield return instruction;
        }
    }
}

public static class HarmonyExtensions
{
    public static bool IsStloc_S(this CodeInstruction ci, int localIndex)
    {
        return ci.opcode == OpCodes.Stloc_S && ((LocalBuilder)ci.operand).LocalIndex == localIndex;
    }
}
