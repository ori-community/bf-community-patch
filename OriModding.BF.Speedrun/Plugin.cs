using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using OriModding.BF.Core;
using OriModding.BF.Core.SeinAbilities;
using OriModding.BF.InputLib;
using UnityEngine;

namespace OriModding.BF.Speedrun;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(OriModding.BF.Core.PluginInfo.PLUGIN_GUID)]
//[BepInDependency(OriModding.BF.ConfigMenu.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> RunInBackground { get; set; }
    public static ConfigEntry<float> BashDeadzone { get; set; }
    public static ConfigEntry<CustomInput> DoubleBashInput { get; set; }

    private Harmony harmony;

    public static new ManualLogSource Logger { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        BashDeadzoneFix.Patch(harmony);
        MoreSaveSlots.Patch(harmony);

        BashDeadzone = Config.Bind("Speedrun", "Bash Deadzone", 0.5f, "How large should the deadzone be while bashing (min 0%, max 100%)");
        RunInBackground = Config.Bind("Speedrun", "Run In Background", true, "Whether the game should continue to run when the window is not selected");

        var inputLib = this.GetPlugin<OriModding.BF.Core.Plugin>("OriModding.BF.Core").InputManager;
        DoubleBashInput = inputLib.BindAndRegister(this, "Speedrun", "Double Bash",
            new CustomInput()
                .AddKeyCodes(KeyCode.T)
                .AddControllerButtons(ControllerButton.LB)
        );
        
        QTMBugfix.Init();
        Controllers.Add<TurboController>();
        CustomSeinAbilityManager.Add<DoubleBashAbility>("5aa4389e-318b-4756-a57b-42565dd53208");
    }
}
