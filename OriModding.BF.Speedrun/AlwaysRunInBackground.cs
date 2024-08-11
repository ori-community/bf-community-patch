using HarmonyLib;
using OriModding.BF.Core;

namespace OriModding.BF.Speedrun;

[HarmonyPatch(typeof(GameController), "OnApplicationFocus")]
internal class AlwaysRunInBackground
{
    private static bool Prefix()
    {
        if (Plugin.RunInBackground.Value)
        {
            GameController.IsFocused = true;
            return HarmonyHelper.StopExecution;
        }

        return HarmonyHelper.ContinueExecution;
    }
}
