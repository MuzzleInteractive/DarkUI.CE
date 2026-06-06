using System.Drawing;
using static DarkUI.Win32.Native;

namespace DarkUI.Config
{
    public class DarkTheme : ITheme
    {
        public string Name => "Dark";
        public bool UseImmersiveDarkMode => true;
        public int CornerPreference => (int)WindowCornerPreference.DoNotRound;
        public int BackdropType => (int)SystemBackdropType.Mica;
        public Color GreyBackground => ColorTranslator.FromHtml("#3C3F41");
        public Color HeaderBackground => ColorTranslator.FromHtml("#393C3E");
        public Color AccentBackground => ColorTranslator.FromHtml("#424D5F");
        public Color DarkAccentBackground => ColorTranslator.FromHtml("#343942");
        public Color DarkBackground => ColorTranslator.FromHtml("#2B2B2B");
        public Color MediumBackground => ColorTranslator.FromHtml("#313335");
        public Color LightBackground => ColorTranslator.FromHtml("#45494A");
        public Color LighterBackground => ColorTranslator.FromHtml("#5F6566");
        public Color LightestBackground => ColorTranslator.FromHtml("#B2B2B2");
        public Color LightBorder => ColorTranslator.FromHtml("#515151");
        public Color DarkBorder => ColorTranslator.FromHtml("#333333");
        public Color LightText => ColorTranslator.FromHtml("#DCDCDC");
        public Color DisabledText => ColorTranslator.FromHtml("#999999");
        public Color AccentHighlight => ColorTranslator.FromHtml("#6897BB");
        public Color AccentSelection => ColorTranslator.FromHtml("#4B6EAF");
        public Color GreyHighlight => ColorTranslator.FromHtml("#7A8084");
        public Color GreySelection => ColorTranslator.FromHtml("#5C5C5C");
        public Color DarkGreySelection => ColorTranslator.FromHtml("#525252");
        public Color DarkAccentBorder => ColorTranslator.FromHtml("#333D4E");
        public Color LightAccentBorder => ColorTranslator.FromHtml("#566172");
        public Color ActiveControl => ColorTranslator.FromHtml("#9FB2C4");
    }
}
