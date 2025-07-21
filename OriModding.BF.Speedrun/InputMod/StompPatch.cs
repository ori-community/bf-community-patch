// using HarmonyLib;
// using OriModding.BF.Core;
// using OriModding.BF.InputLib;
//
// namespace OriModding.BF.Speedrun;
//
// [HarmonyPatch(typeof(SeinStomp), nameof(SeinStomp.UpdateStompInactiveState))]
// internal class StompPatch
// {
//     private static bool Prefix(SeinStomp __instance)
//     {
//         if (__instance.Sein.Controller.IsSwimming)
//             return HarmonyHelper.StopExecution;
//
//         // if (CustomInput.Stomp.OnPressed && !CustomInput.Stomp.Used && __instance.CanStomp())
//         //     __instance.Logic.ChangeState(__instance.State.StompIdle);
//             
//         return HarmonyHelper.StopExecution;
//     }
// }
