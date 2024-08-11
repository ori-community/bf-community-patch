using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SmartInput;
using UnityEngine;

namespace OriModding.BF.Speedrun;

public class TurboController : MonoBehaviour
{
    enum SpecialTargets
    {
        None,
        LSLeft,
        LSRight,
        LSUp,
        LSDown,
        RSLeft,
        RSRight,
        RSUp,
        RSDown,
        DPadLeft,
        DPadRight,
        DPadUp,
        DPadDown
    }

    class TurboButton(IButtonInput button, global::Core.Input.InputButtonProcessor target, SpecialTargets specialTarget = SpecialTargets.None)
    {
        private readonly IButtonInput button = button;
        private readonly global::Core.Input.InputButtonProcessor target = target;
        private readonly SpecialTargets specialTarget = specialTarget;
        int value = 0;

        public void Update()
        {
            if (button.GetButton())
            {
                switch (specialTarget)
                {
                    case SpecialTargets.None:
                        target.Update(false);
                        if (value == 0)
                            target.Update(true);
                        break;

                    case SpecialTargets.LSLeft:
                        global::Core.Input.HorizontalAnalogLeft = value == 0 ? -1 : 0;
                        break;
                    case SpecialTargets.LSRight:
                        global::Core.Input.HorizontalAnalogLeft = value == 0 ? 1 : 0;
                        break;
                    case SpecialTargets.LSUp:
                        global::Core.Input.VerticalAnalogLeft = value == 0 ? 1 : 0;
                        break;
                    case SpecialTargets.LSDown:
                        global::Core.Input.VerticalAnalogLeft = value == 0 ? -1 : 0;
                        break;
                    case SpecialTargets.RSLeft:
                        global::Core.Input.HorizontalAnalogRight = value == 0 ? -1 : 0;
                        break;
                    case SpecialTargets.RSRight:
                        global::Core.Input.HorizontalAnalogRight = value == 0 ? 1 : 0;
                        break;
                    case SpecialTargets.RSUp:
                        global::Core.Input.VerticalAnalogRight = value == 0 ? 1 : 0;
                        break;
                    case SpecialTargets.RSDown:
                        global::Core.Input.VerticalAnalogRight = value == 0 ? -1 : 0;
                        break;
                    case SpecialTargets.DPadLeft:
                        global::Core.Input.HorizontalDigiPad = value == 0 ? -1 : 0;
                        break;
                    case SpecialTargets.DPadRight:
                        global::Core.Input.HorizontalDigiPad = value == 0 ? 1 : 0;
                        break;
                    case SpecialTargets.DPadUp:
                        global::Core.Input.VerticalDigiPad = value == 0 ? 1 : 0;
                        break;
                    case SpecialTargets.DPadDown:
                        global::Core.Input.VerticalDigiPad = value == 0 ? -1 : 0;
                        break;
                }

                value = (value + 1) % 3;
            }
            else
            {
                value = 0;
            }
        }
    }

    List<TurboButton> turboButtons;

    void Awake()
    {
        turboButtons = new List<TurboButton>();
        Load(Path.Combine(OutputFolder.PlayerDataFolderPath, "turbo.txt"));

        On.PlayerInput.FixedUpdate += (orig, self) =>
        {
            orig(self);
            foreach (var button in turboButtons)
                button.Update();
            self.RefreshControls();
        };
    }

    private void Load(string filepath)
    {
        // Format per line:
        // <target>:<control string>
        // e.g. SpiritFlame:T,LT+FaceX
        //  Spirit flame turbo is active if T is held, or if left trigger and X are held on a controller

        if (!File.Exists(filepath))
            return;

        var targets = typeof(global::Core.Input).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.FieldType == typeof(global::Core.Input.InputButtonProcessor))
            .ToList();

        using var reader = new StreamReader(filepath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine().Split(new[] { ':' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (line.Length == 0) // blank line
                continue;

            if (line[0].Trim().StartsWith("#")) // comment
                continue;

            if (line.Length != 2) // invalid line
            {
                Plugin.Logger.LogWarning($"Invalid turbo configuration (format): {line}");
                continue;
            }


            var input = new CompoundButtonInput(InputLib.CustomInput.ParseButtons(line[1]));
            if (input.Buttons.Length != line[1].Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Length)
            {
                Plugin.Logger.LogWarning($"Invalid turbo configuration (buttons): {line}");
                continue;
            }

            var targetInput = targets.FirstOrDefault(x => x.Name.ToLower() == line[0].ToLower());
            if (targetInput != null)
            {
                turboButtons.Add(new TurboButton(input, targetInput.GetValue(null) as global::Core.Input.InputButtonProcessor));
                Plugin.Logger.LogInfo($"Added turbo: {line[0]}:{line[1]}");
            }
            else
            {
                // Could be a movement one
                try
                {
                    var t = (SpecialTargets)Enum.Parse(typeof(SpecialTargets), line[0], true);
                    turboButtons.Add(new TurboButton(input, null, t));
                    Plugin.Logger.LogInfo($"Added turbo: {line[0]}:{line[1]}");
                }
                catch (Exception)
                {
                    Plugin.Logger.LogWarning($"Invalid turbo configuration (target): {line}");
                    continue;
                }
            }
        }
    }
}
