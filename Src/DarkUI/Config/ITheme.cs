using System.Drawing;

namespace DarkUI.Config
{
    public interface ITheme
    {
        string Name { get; }
        bool UseImmersiveDarkMode { get; }
        int CornerPreference { get; }
        int BackdropType { get; }
        Color GreyBackground { get; }
        Color HeaderBackground { get; }
        Color AccentBackground { get; }
        Color DarkAccentBackground { get; }
        Color DarkBackground { get; }
        Color MediumBackground { get; }
        Color LightBackground { get; }
        Color LighterBackground { get; }
        Color LightestBackground { get; }
        Color LightBorder { get; }
        Color DarkBorder { get; }
        Color LightText { get; }
        Color DisabledText { get; }
        Color AccentHighlight { get; }
        Color AccentSelection { get; }
        Color GreyHighlight { get; }
        Color GreySelection { get; }
        Color DarkGreySelection { get; }
        Color DarkAccentBorder { get; }
        Color LightAccentBorder { get; }
        Color ActiveControl { get; }
    }
}
