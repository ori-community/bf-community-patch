using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SmartInput;
using UnityEngine;

namespace OriModding.BF.Speedrun;

public class TurboController : MonoBehaviour
{
    class TurboButton(IButtonInput button, global::Core.Input.InputButtonProcessor target)
    {
        readonly IButtonInput button = button;
        readonly global::Core.Input.InputButtonProcessor target = target;
        int value = 0;

        public void Update()
        {
            if (button.GetButton())
            {
                target.Update(false);
                if (value == 0)
                    target.Update(true);

                value = (value + 1) % 3;
            }
            else
            {
                value = 0;
            }
        }
    }

    List<TurboButton> turboButtons = new List<TurboButton>();

    void Awake()
    {
        turboButtons = new List<TurboButton>();
        Load(Path.Combine(OutputFolder.PlayerDataFolderPath, "turbo.txt"));

        On.PlayerInput.FixedUpdate += (orig, self) =>
        {
            orig(self);
            foreach (var button in turboButtons)
                button.Update();
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

            var targetInput = targets.FirstOrDefault(x => x.Name.ToLower() == line[0].ToLower());
            if (targetInput == null)
            {
                Plugin.Logger.LogWarning($"Invalid turbo configuration (target): {line}");
                continue;
            }

            var input = new CompoundButtonInput(InputLib.CustomInput.ParseButtons(line[1]));
            if (input.Buttons.Length != line[1].Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Length)
            {
                Plugin.Logger.LogWarning($"Invalid turbo configuration (buttons): {line}");
                continue;
            }

            turboButtons.Add(new TurboButton(input, targetInput.GetValue(null) as global::Core.Input.InputButtonProcessor));
            Plugin.Logger.LogInfo($"Added turbo: {line[0]}:{line[1]}");
        }
    }

    void OnGUI()
    {
        if (global::Core.Input.SpiritFlame.IsPressed)
        {

            GUI.Box(new Rect(5, 5, 50, 50), "SPIRITFLAME");
        }
    }
}
