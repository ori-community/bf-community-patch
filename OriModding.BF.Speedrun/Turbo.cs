using System.Collections.Generic;
using SmartInput;
using UnityEngine;

namespace OriModding.BF.Speedrun;

public class TurboController : MonoBehaviour
{
    class TurboButton
    {
        public IButtonInput button;
        public global::Core.Input.InputButtonProcessor target;
        public int value = 0;

        public void Update()
        {
            if (button.GetButton())
            {
                target.Update(value == 0);
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
        turboButtons = new List<TurboButton>
        {
            new TurboButton
            {
                button = new KeyCodeButtonInput(KeyCode.Y),
                target = global::Core.Input.SpiritFlame
            }
        };

        On.PlayerInput.FixedUpdate += (orig, self) =>
        {
            orig(self);
            foreach (var button in turboButtons)
                button.Update();
        };
    }

    void OnGUI()
    {
        if (global::Core.Input.SpiritFlame.IsPressed)
        {

            GUI.Box(new Rect(5, 5, 50, 50), "SPIRITFLAME");
        }
    }
}
