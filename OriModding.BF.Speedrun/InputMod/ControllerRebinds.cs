using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OriModding.BF.Speedrun;

public class ControllerRebinds
{
    private static ControllerBindingSettings controllerRebindings;

    private const string ControllerInputRebindingsFileName = "ControllerRebindings.txt";
    private static string ControllerRebindingFile => Path.Combine(OutputFolder.PlayerDataFolderPath, ControllerInputRebindingsFileName);

    public static ControllerBindingSettings ControllerRebindings
    {
        get
        {
            if (controllerRebindings == null)
            {
                if (!File.Exists(ControllerRebindingFile))
                {
                    SetDefaultControllerBindingSettings();
                    WriteControllerRebindSettings();
                }
                else
                {
                    GetControllerRebindSettingsFromFile();
                }
            }
            return controllerRebindings;
        }
    }

    private static void SetDefaultControllerBindingSettings()
    {
        controllerRebindings = new ControllerBindingSettings();
    }

    private static void GetControllerRebindSettingsFromFile()
    {
        try
        {
            using (StreamReader streamReader = new StreamReader(new FileStream(ControllerRebindingFile, FileMode.Open)))
            {
                streamReader.ReadLine();
                streamReader.ReadLine();
                controllerRebindings = new ControllerBindingSettings
                {
                    HorizontalDigiPadLeft = StringToControllerBinding(streamReader.ReadLine()),
                    HorizontalDigiPadRight = StringToControllerBinding(streamReader.ReadLine()),
                    VerticalDigiPadDown = StringToControllerBinding(streamReader.ReadLine()),
                    VerticalDigiPadUp = StringToControllerBinding(streamReader.ReadLine()),
                    MenuLeft = StringToControllerBinding(streamReader.ReadLine()),
                    MenuRight = StringToControllerBinding(streamReader.ReadLine()),
                    MenuDown = StringToControllerBinding(streamReader.ReadLine()),
                    MenuUp = StringToControllerBinding(streamReader.ReadLine()),
                    MenuPageLeft = StringToControllerBinding(streamReader.ReadLine()),
                    MenuPageRight = StringToControllerBinding(streamReader.ReadLine()),
                    ActionButtonA = StringToControllerBinding(streamReader.ReadLine()),
                    SoulFlame = StringToControllerBinding(streamReader.ReadLine()),
                    Jump = StringToControllerBinding(streamReader.ReadLine()),
                    Grab = StringToControllerBinding(streamReader.ReadLine()),
                    SpiritFlame = StringToControllerBinding(streamReader.ReadLine()),
                    Bash = StringToControllerBinding(streamReader.ReadLine()),
                    Glide = StringToControllerBinding(streamReader.ReadLine()),
                    ChargeJump = StringToControllerBinding(streamReader.ReadLine()),
                    Select = StringToControllerBinding(streamReader.ReadLine()),
                    Start = StringToControllerBinding(streamReader.ReadLine()),
                    Cancel = StringToControllerBinding(streamReader.ReadLine()),
                    LeftShoulder = StringToControllerBinding(streamReader.ReadLine()),
                    RightShoulder = StringToControllerBinding(streamReader.ReadLine()),
                    LeftStick = StringToControllerBinding(streamReader.ReadLine()),
                    RightStick = StringToControllerBinding(streamReader.ReadLine()),
                    ZoomIn = StringToControllerBinding(streamReader.ReadLine()),
                    ZoomOut = StringToControllerBinding(streamReader.ReadLine()),
                    Copy = StringToControllerBinding(streamReader.ReadLine()),
                    Delete = StringToControllerBinding(streamReader.ReadLine()),
                    Focus = StringToControllerBinding(streamReader.ReadLine()),
                    Filter = StringToControllerBinding(streamReader.ReadLine()),
                    Legend = StringToControllerBinding(streamReader.ReadLine()),
                    // Stomp = StringToControllerBinding(streamReader.ReadLine())
                };
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Failed to read " + ControllerInputRebindingsFileName);
            Debug.Log(ex.ToString());
            SetDefaultControllerBindingSettings();
        }
    }

    public static void WriteControllerRebindSettings()
    {
        using (StreamWriter streamWriter = new StreamWriter(new FileStream(ControllerRebindingFile, FileMode.OpenOrCreate)))
        {
            ControllerBindingSettings binds = controllerRebindings;
            streamWriter.WriteLine("Controller Rebindings");
            streamWriter.WriteLine("--------");
            streamWriter.WriteLine("Movement Left: " + ControllerBindingToString(binds.HorizontalDigiPadLeft));
            streamWriter.WriteLine("Movement Right: " + ControllerBindingToString(binds.HorizontalDigiPadRight));
            streamWriter.WriteLine("Movement Down: " + ControllerBindingToString(binds.VerticalDigiPadDown));
            streamWriter.WriteLine("Movement Up: " + ControllerBindingToString(binds.VerticalDigiPadUp));
            streamWriter.WriteLine("Menu Left: " + ControllerBindingToString(binds.MenuLeft));
            streamWriter.WriteLine("Menu Right: " + ControllerBindingToString(binds.MenuRight));
            streamWriter.WriteLine("Menu Down: " + ControllerBindingToString(binds.MenuDown));
            streamWriter.WriteLine("Menu Up: " + ControllerBindingToString(binds.MenuUp));
            streamWriter.WriteLine("Menu Previous: " + ControllerBindingToString(binds.MenuPageLeft));
            streamWriter.WriteLine("Menu Next: " + ControllerBindingToString(binds.MenuPageRight));
            streamWriter.WriteLine("Proceed: " + ControllerBindingToString(binds.ActionButtonA));
            streamWriter.WriteLine("Soul Link: " + ControllerBindingToString(binds.SoulFlame));
            streamWriter.WriteLine("Jump: " + ControllerBindingToString(binds.Jump));
            streamWriter.WriteLine("Grab: " + ControllerBindingToString(binds.Grab));
            streamWriter.WriteLine("Spirit Flame: " + ControllerBindingToString(binds.SpiritFlame));
            streamWriter.WriteLine("Bash: " + ControllerBindingToString(binds.Bash));
            streamWriter.WriteLine("Glide: " + ControllerBindingToString(binds.Glide));
            streamWriter.WriteLine("Charge Jump: " + ControllerBindingToString(binds.ChargeJump));
            streamWriter.WriteLine("Map: " + ControllerBindingToString(binds.Select));
            streamWriter.WriteLine("Start: " + ControllerBindingToString(binds.Start));
            streamWriter.WriteLine("Cancel: " + ControllerBindingToString(binds.Cancel));
            streamWriter.WriteLine("Grenade: " + ControllerBindingToString(binds.LeftShoulder));
            streamWriter.WriteLine("Dash: " + ControllerBindingToString(binds.RightShoulder));
            streamWriter.WriteLine("Left Stick: " + ControllerBindingToString(binds.LeftStick));
            streamWriter.WriteLine("Debug Menu (shhh): " + ControllerBindingToString(binds.RightStick));
            streamWriter.WriteLine("Zoom In World Map: " + ControllerBindingToString(binds.ZoomIn));
            streamWriter.WriteLine("Zoom Out World Map: " + ControllerBindingToString(binds.ZoomOut));
            streamWriter.WriteLine("Copy: " + ControllerBindingToString(binds.Copy));
            streamWriter.WriteLine("Delete: " + ControllerBindingToString(binds.Delete));
            streamWriter.WriteLine("Focus: " + ControllerBindingToString(binds.Focus));
            streamWriter.WriteLine("Filter: " + ControllerBindingToString(binds.Filter));
            streamWriter.WriteLine("Legend: " + ControllerBindingToString(binds.Legend));
            // streamWriter.WriteLine("Stomp: " + ControllerBindingToString(binds.Stomp));
            streamWriter.WriteLine("--------");
            streamWriter.WriteLine("Usage:");
            streamWriter.WriteLine("- There is no guarantee of the game still being playable after key rebinding. Please use with caution and delete this file in case of breakage");
            streamWriter.WriteLine("- Spelling and syntactical errors will result in the key rebindings not registering properly, and the game will get set to default");
            streamWriter.WriteLine("- Deleting this file will result in this file being recreated by the game, containing the default settings");
            streamWriter.WriteLine("--------");
            streamWriter.WriteLine("Don't forget to restart the game after editing this file!");
            streamWriter.WriteLine("Don't forget to close this file before restarting the game!");
            streamWriter.WriteLine("--------");
            streamWriter.WriteLine("Movement is always on the left thumbstick");
            streamWriter.WriteLine("LLeft, LRight, LUp, LDown is for the left thumbstick");
            streamWriter.WriteLine("RLeft, RRight, RUp, RDown is for the right thumbstick");
            streamWriter.WriteLine("DLeft, DRight, DUp, DDown is for the D-Pad");
            streamWriter.WriteLine("Based on an xbox controller - i.e. A, B, X, Y, LT, RT, LB, RB, LS, RS correspond to Cross, Circle, Square, Triangle, L2, R2, L1, R1, L3, R3");
            streamWriter.WriteLine("Not guaranteed to work on controllers other than xbox");
        }
    }

    private static string ControllerBindingToString(ControllerButton[] codes)
    {
        //string text = string.Empty;
        //bool flag = true;
        return string.Join(", ", codes.Select(c => c.ToString()).ToArray());
        //foreach (ControllerButton controllerButton in codes)
        //{
        //    text += flag ? string.Empty : ", ";
        //    text += controllerButton;
        //    flag = false;
        //}
        //return text;
    }

    private static ControllerButton[] StringToControllerBinding(string s)
    {
        s = s.Split(new string[] { ": " }, StringSplitOptions.None)[1];
        string[] array = s.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        List<ControllerButton> list = new List<ControllerButton>();
        foreach (string value in array)
        {
            list.Add((ControllerButton)(int)Enum.Parse(typeof(ControllerButton), value));
        }
        return list.ToArray();
    }

    public class ControllerBindingSettings
    {
        public ControllerButton[] HorizontalDigiPadLeft = new ControllerButton[] { ControllerButton.DLeft };
        public ControllerButton[] HorizontalDigiPadRight = new ControllerButton[] { ControllerButton.DRight };
        public ControllerButton[] VerticalDigiPadUp = new ControllerButton[] { ControllerButton.DUp };
        public ControllerButton[] VerticalDigiPadDown = new ControllerButton[] { ControllerButton.DDown };
        public ControllerButton[] ActionButtonA = new ControllerButton[] { ControllerButton.A };
        public ControllerButton[] Bash = new ControllerButton[] { ControllerButton.Y };
        public ControllerButton[] Cancel = new ControllerButton[] { ControllerButton.B };
        public ControllerButton[] ChargeJump = new ControllerButton[] { ControllerButton.LT };
        public ControllerButton[] Copy = new ControllerButton[] { ControllerButton.X };
        public ControllerButton[] Delete = new ControllerButton[] { ControllerButton.Y };
        public ControllerButton[] Filter = new ControllerButton[] { ControllerButton.X };
        public ControllerButton[] Focus = new ControllerButton[] { ControllerButton.X };
        public ControllerButton[] Glide = new ControllerButton[] { ControllerButton.RT };
        public ControllerButton[] Grab = new ControllerButton[] { ControllerButton.RT };
        public ControllerButton[] Jump = new ControllerButton[] { ControllerButton.A };
        public ControllerButton[] LeftShoulder = new ControllerButton[] { ControllerButton.LB };
        public ControllerButton[] LeftStick = new ControllerButton[] { ControllerButton.LS };
        public ControllerButton[] Legend = new ControllerButton[] { ControllerButton.Y };
        public ControllerButton[] MenuDown = new ControllerButton[] { ControllerButton.LDown, ControllerButton.DDown };
        public ControllerButton[] MenuLeft = new ControllerButton[] { ControllerButton.LLeft, ControllerButton.DLeft };
        public ControllerButton[] MenuPageLeft = new ControllerButton[] { ControllerButton.LT };
        public ControllerButton[] MenuPageRight = new ControllerButton[] { ControllerButton.RT };
        public ControllerButton[] MenuRight = new ControllerButton[] { ControllerButton.LRight, ControllerButton.DRight };
        public ControllerButton[] MenuUp = new ControllerButton[] { ControllerButton.LUp, ControllerButton.DUp };
        public ControllerButton[] RightShoulder = new ControllerButton[] { ControllerButton.RB };
        public ControllerButton[] RightStick = new ControllerButton[] { ControllerButton.RS };
        public ControllerButton[] Select = new ControllerButton[] { ControllerButton.Back };
        public ControllerButton[] SoulFlame = new ControllerButton[] { ControllerButton.B };
        public ControllerButton[] SpiritFlame = new ControllerButton[] { ControllerButton.X };
        public ControllerButton[] Start = new ControllerButton[] { ControllerButton.Start };
        // public ControllerButton[] Stomp = new ControllerButton[] { ControllerButton.LDown, ControllerButton.DDown };
        public ControllerButton[] ZoomIn = new ControllerButton[] { ControllerButton.RT };
        public ControllerButton[] ZoomOut = new ControllerButton[] { ControllerButton.LT };
    }

    public enum ControllerButton
    {
        A,
        B,
        X,
        Y,
        LT,
        RT,
        LB,
        RB,
        LS,
        RS,
        LUp,
        LDown,
        LLeft,
        LRight,
        DUp,
        DDown,
        DLeft,
        DRight,
        RUp,
        RDown,
        RLeft,
        RRight,
        Back,
        Start
    }
}
