using HarmonyLib;

namespace KFT.OriBF.Qol;

[HarmonyPatch(typeof(GameController), "OnApplicationFocus")]
internal class AlwaysRunInBackground
{
    private static bool Prefix()
    {
        if (Plugin.RunInBackground.Value)
        {
            GameController.IsFocused = true;
            return false;
        }

        return true;
    }
}
