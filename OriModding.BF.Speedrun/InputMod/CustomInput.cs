using SmartInput;
using UnityEngine;

namespace OriModding.BF.Speedrun;

public class CustomPlayerInput
{
    // private static CompoundButtonInput stompInput = new CompoundButtonInput();

    // public static global::Core.Input.InputButtonProcessor Stomp = new global::Core.Input.InputButtonProcessor();

    public static void ClearControls()
    {
        // stompInput.Clear();
    }

    public static void AddKeyboardControls()
    {
        // AddKeyCodesToButtonInput(CustomKeyRebindings.KeyRebindings.Stomp, stompInput);
    }

    public static void AddControllerControls()
    {
        var playerInput = PlayerInput.Instance;

        playerInput.HorizontalAnalogLeft.Add(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickX));
        playerInput.VerticalAnalogLeft.Add(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickY));
        //playerInput.HorizontalAnalogRight.Add(new ControllerAxisInput(XboxControllerInput.Axis.RightStickX));
        //playerInput.VerticalAnalogRight.Add(new ControllerAxisInput(XboxControllerInput.Axis.RightStickY));

        // TODO this is required at this point for pause menu navigation using dpad, fix controller to use menu buttons instead of axis to navigate
        playerInput.HorizontalDigiPad.Add(new ControllerAxisInput(XboxControllerInput.Axis.DpadX));
        playerInput.VerticalDigiPad.Add(new ControllerAxisInput(XboxControllerInput.Axis.DpadY));

        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Jump, playerInput.Jump);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.SpiritFlame, playerInput.SpiritFlame);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.SoulFlame, playerInput.SoulFlame);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Bash, playerInput.Bash);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.ChargeJump, playerInput.ChargeJump);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.ZoomIn, playerInput.ZoomIn);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Glide, playerInput.Glide);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Grab, playerInput.Grab);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.ZoomOut, playerInput.ZoomOut);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.LeftShoulder, playerInput.LeftShoulder);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.RightShoulder, playerInput.RightShoulder);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Select, playerInput.Select);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Start, playerInput.Start);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.LeftStick, playerInput.LeftStick);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.RightStick, playerInput.RightStick);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.MenuDown, playerInput.MenuDown);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.MenuUp, playerInput.MenuUp);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.MenuLeft, playerInput.MenuLeft);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.MenuRight, playerInput.MenuRight);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.ActionButtonA, playerInput.ActionButtonA);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Cancel, playerInput.Cancel);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.MenuPageLeft, playerInput.MenuPageLeft);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.MenuPageRight, playerInput.MenuPageRight);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Copy, playerInput.Copy);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Delete, playerInput.Delete);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Focus, playerInput.Focus);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Filter, playerInput.Filter);
        AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Legend, playerInput.Legend);

        // AddControllerButtonsToButtonInput(ControllerRebinds.ControllerRebindings.Stomp, stompInput);
    }

    public static void FixedUpdate()
    {
        // var proceed = global::Core.Input.ActionButtonA.Pressed;
        // var cancel = global::Core.Input.Cancel.Pressed;
        //
        // Debug.Log($"A: {PlayerInput.Instance.ActionButtonA.GetButton()}      B: {PlayerInput.Instance.Cancel.GetButton()}      Proceed: {proceed}      Cancel: {cancel}");
        
        // Stomp.Update(stompInput.GetButton());
        //
        // // this.RefreshControls
        // Stomp.Used = false;

        // TODO last pressed button stuff
        /*
            if (!ControlsScreen.IsVisible && this.m_lastPressedButtonInput != -1)
            {
                bool flag = this.WasKeyboardUsedLast;
                if (this.m_lastPressedButtonInput != -1)
                {
                    flag = this.KeyboardUsedLast(this.m_allButtonInput[this.m_lastPressedButtonInput]);
                }
                if (flag != this.WasKeyboardUsedLast)
                {
                    GameSettings.Instance.CurrentControlScheme = ((!flag) ? ControlScheme.Controller : GameSettings.Instance.KeyboardScheme);
                }
            }
         */
    }

    private static void AddKeyCodesToButtonInput(KeyCode[] keyCodes, CompoundButtonInput buttonInput)
    {
        for (int i = 0; i < keyCodes.Length; i++)
            buttonInput.Add(new KeyCodeButtonInput(keyCodes[i]));
    }

    private static void AddControllerButtonsToButtonInput(ControllerRebinds.ControllerButton[] buttons, CompoundButtonInput buttonInput)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttonInput.Add(ControllerButtonToButtonInput(buttons[i]));
    }

    private static IButtonInput ControllerButtonToButtonInput(ControllerRebinds.ControllerButton button)
    {
        switch (button)
        {
            case ControllerRebinds.ControllerButton.A:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonA);
            case ControllerRebinds.ControllerButton.B:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonB);
            case ControllerRebinds.ControllerButton.X:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonX);
            case ControllerRebinds.ControllerButton.Y:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonY);
            case ControllerRebinds.ControllerButton.LT:
                return new ControllerButtonInput(XboxControllerInput.Button.LeftTrigger);
            case ControllerRebinds.ControllerButton.RT:
                return new ControllerButtonInput(XboxControllerInput.Button.RightTrigger);
            case ControllerRebinds.ControllerButton.LB:
                return new ControllerButtonInput(XboxControllerInput.Button.LeftShoulder);
            case ControllerRebinds.ControllerButton.RB:
                return new ControllerButtonInput(XboxControllerInput.Button.RightShoulder);
            case ControllerRebinds.ControllerButton.LS:
                return new ControllerButtonInput(XboxControllerInput.Button.LeftStick);
            case ControllerRebinds.ControllerButton.RS:
                return new ControllerButtonInput(XboxControllerInput.Button.RightStick);
            case ControllerRebinds.ControllerButton.LUp:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickY), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerRebinds.ControllerButton.LDown:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickY), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerRebinds.ControllerButton.LLeft:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickX), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerRebinds.ControllerButton.LRight:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickX), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerRebinds.ControllerButton.DUp:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadY), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerRebinds.ControllerButton.DDown:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadY), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerRebinds.ControllerButton.DLeft:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadX), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerRebinds.ControllerButton.DRight:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadX), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerRebinds.ControllerButton.RUp:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickY), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerRebinds.ControllerButton.RDown:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickY), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerRebinds.ControllerButton.RLeft:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickX), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerRebinds.ControllerButton.RRight:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickX), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerRebinds.ControllerButton.Back:
                return new ControllerButtonInput(XboxControllerInput.Button.Select);
            case ControllerRebinds.ControllerButton.Start:
                return new ControllerButtonInput(XboxControllerInput.Button.Start);
            default:
                return null;
        }
    }
}
