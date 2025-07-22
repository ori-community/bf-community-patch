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
    private const string Version = "0.1.3";

    public static ConfigEntry<bool> RunInBackground { get; set; }
    public static ConfigEntry<bool> CursorLock { get; set; }
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
        CursorLock = Config.Bind("Speedrun", "Cursor Lock", false, "Whether the cursor should be confined to the game window while it is selected");

        SetCursorLock(CursorLock.Value);
        CursorLock.SettingChanged += (sender, _) => SetCursorLock(((ConfigEntry<bool>)sender).Value);
        
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

    private static void SetCursorLock(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.None;
    }

    private GUIStyle style;

    public void OnGUI()
    {
        // Show version in main menu and pause menu
        if (GameStateMachine.Instance?.CurrentState is GameStateMachine.State.Logos or GameStateMachine.State.StartScreen or GameStateMachine.State.TitleScreen
            || Game.UI.Menu?.MainMenuVisible == true)
        {
            style ??= new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerLeft };
            GUI.Label(new Rect(6f, Screen.height - 64f - 6f, 300f, 64f), $"Ori DE Speedrun Mod v{Version}", style);
        }
    }
}
