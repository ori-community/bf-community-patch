using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace KFT.OriBF.Qol;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(OriModding.BF.Core.PluginInfo.PLUGIN_GUID)]
[BepInDependency(OriModding.BF.ConfigMenu.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> CursorLock { get; set; }
    public static ConfigEntry<bool> RunInBackground { get; set; }
    public static ConfigEntry<float> BashDeadzone { get; set; }
    public static ConfigEntry<float> AbilityMenuOpacity { get; set; }
    public static ConfigEntry<float> ScreenShakeStrength { get; set; }
    public static ConfigEntry<bool> SkipText { get; set; }
    public static ConfigEntry<bool> CameraSway { get; set; }
    public static ConfigEntry<float> HudScale { get; set; }
    public static ConfigEntry<bool> FasterTeleport { get; set; }
    public static ConfigEntry<bool> MouseChargeJumpControls { get; set; }
    public static ConfigEntry<bool> MouseSwimControls { get; set; }
    public static ConfigEntry<bool> InvertSwim { get; set; }

    private Harmony harmony;

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        BashDeadzoneFix.Patch(harmony);
        MoreSaveSlots.Patch(harmony);

        CursorLock = Config.Bind("QOL", "Cursor Lock", false, "Whether the cursor should be locked to the screen");
        ScreenShakeStrength = Config.Bind("QOL", "Screen Shake Strength", 1f, "How strong should the screen shake effects be (min 0%, max 100%)");
        BashDeadzone = Config.Bind("QOL", "Bash Deadzone", 0.5f, "How large should the deadzone be while bashing (min 0%, max 100%)");
        RunInBackground = Config.Bind("QOL", "Run In Background", true, "Whether the game should continue to run when the window is not selected");
        AbilityMenuOpacity = Config.Bind("QOL", "Ability Menu Opacity", 1f, "How opaque should the ability menu be while moving in the background (min 0%, max 100%)");
        SkipText = Config.Bind("QOL", "Skip Text", false, "Whether the text boxes from Sein and pickups should be skipped");
        CameraSway = Config.Bind("QOL", "Camera Sway", true, "Whether the camera should subtly move when stationary");
        HudScale = Config.Bind("QOL", "HUD Scale", 1f, "How large the HUD should appear on screen (min 40%, max 160%)");
        FasterTeleport = Config.Bind("QOL", "Faster Teleporting", true, "Whether the teleportation animation should be sped up");
        MouseChargeJumpControls = Config.Bind("QOL", "Cursor Charge Jump Control", false, "Whether the mouse cursor can be used to aim charge jump");
        MouseSwimControls = Config.Bind("QOL", "Cursor Swim Control", false, "Whether Ori will swim towards the mouse cursor");
        InvertSwim = Config.Bind("QOL", "Invert Swim", false, "Whether Ori swims at maximum speed without holding [Jump]");

        if (Chainloader.PluginInfos.TryGetValue(OriModding.BF.ConfigMenu.PluginInfo.PLUGIN_GUID, out var pi))
        {
            ConfigMenu(pi.Instance);
        }


        Cursor.lockState = CursorLock.Value ? CursorLockMode.Confined : CursorLockMode.None;
        CursorLock.SettingChanged += (_, _) => Cursor.lockState = CursorLock.Value ? CursorLockMode.Confined : CursorLockMode.None;


        QTMBugfix.Init();
        On.AreaMapNavigation.Awake += (orig, self) =>
        {
            orig(self);
            self.AreaMapZoomLevel = 1;
        };
    }

    private void ConfigMenu(BaseUnityPlugin plugin)
    {
        var configMenu = plugin as OriModding.BF.ConfigMenu.Plugin;
        configMenu.ConfigureSlider(HudScale, 0.4f, 1.6f, 0.1f);
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
