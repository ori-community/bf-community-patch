namespace KFT.OriBF.Qol;

internal static class QTMBugfix
{
    public static bool delayNavigation;

    public static void Init()
    {
        On.CleverMenuItemSelectionManager.Start += (orig, self) =>
        {
            delayNavigation = Core.Input.MenuDown.IsPressed || Core.Input.MenuUp.IsPressed;
            orig(self);
        };

        On.CleverMenuItemSelectionManager.FixedUpdate += (orig, self) =>
        {
            if (self.IsActive && delayNavigation && !Core.Input.MenuDown.IsPressed && !Core.Input.MenuUp.IsPressed)
                delayNavigation = false;
            orig(self);
        };

        On.CleverMenuItemSelectionManager.MoveSelection += (orig, self, forward) =>
        {
            if (!delayNavigation)
                orig(self, forward);
        };
    }
}
