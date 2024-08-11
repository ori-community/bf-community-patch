using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace OriModding.BF.Speedrun;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(OriModding.BF.Core.PluginInfo.PLUGIN_GUID)]
//[BepInDependency(OriModding.BF.ConfigMenu.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> RunInBackground { get; set; }
    public static ConfigEntry<float> BashDeadzone { get; set; }

    private Harmony harmony;

    private void Awake()
    {
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        BashDeadzoneFix.Patch(harmony);
        MoreSaveSlots.Patch(harmony);

        BashDeadzone = Config.Bind("QOL", "Bash Deadzone", 0.5f, "How large should the deadzone be while bashing (min 0%, max 100%)");
        RunInBackground = Config.Bind("QOL", "Run In Background", true, "Whether the game should continue to run when the window is not selected");

        QTMBugfix.Init();
    }
}
