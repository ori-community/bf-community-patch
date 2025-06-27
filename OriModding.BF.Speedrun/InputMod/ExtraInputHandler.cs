using HarmonyLib;
using OriModding.BF.Core;

namespace OriModding.BF.Speedrun;

[HarmonyPatch]
internal class ExtraInputHandler
{
    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.ClearControls))]
    private static void ClearControls() => CustomPlayerInput.ClearControls();

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.AddKeyboardControls))]
    private static void AddKeyboardControls() => CustomPlayerInput.AddKeyboardControls();

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.FixedUpdate))]
    private static void FixedUpdate() => CustomPlayerInput.FixedUpdate();

    [HarmonyPrefix, HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.AddControllerControls))]
    private static bool AddControllerControls()
    {
        CustomPlayerInput.AddControllerControls();
        return HarmonyHelper.StopExecution;
    }
}
