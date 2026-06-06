using System.Drawing;
using static DarkUI.Win32.Native;

namespace DarkUI.Config
{
    public class LightTheme : ITheme
    {
        public string Name => "Light";
        public bool UseImmersiveDarkMode => false;
        public int CornerPreference => (int)WindowCornerPreference.DoNotRound;
        public int BackdropType => (int)SystemBackdropType.Mica;
        public Color GreyBackground => ColorTranslator.FromHtml("#B4B7B9");
        public Color HeaderBackground => ColorTranslator.FromHtml("#B1B4B6");
        public Color AccentBackground => ColorTranslator.FromHtml("#BAC5D7");
        public Color DarkAccentBackground => ColorTranslator.FromHtml("#ACB1BA");
        public Color DarkBackground => ColorTranslator.FromHtml("#A0A0A0");
        public Color MediumBackground => ColorTranslator.FromHtml("#A9ABAD");
        public Color LightBackground => ColorTranslator.FromHtml("#BDC1C2");
        public Color LighterBackground => ColorTranslator.FromHtml("#646566");
        public Color LightestBackground => ColorTranslator.FromHtml("#B2B2B2");
        public Color LightBorder => ColorTranslator.FromHtml("#C9C9C9");
        public Color DarkBorder => ColorTranslator.FromHtml("#ABABAB");
        public Color LightText => ColorTranslator.FromHtml("#141414");
        public Color DisabledText => ColorTranslator.FromHtml("#676767");
        public Color AccentHighlight => ColorTranslator.FromHtml("#6897BB");
        public Color AccentSelection => ColorTranslator.FromHtml("#4B6EAF");
        public Color GreyHighlight => ColorTranslator.FromHtml("#A2A5A7");
        public Color GreySelection => ColorTranslator.FromHtml("#909090");
        public Color DarkGreySelection => ColorTranslator.FromHtml("#CACACA");
        public Color DarkAccentBorder => ColorTranslator.FromHtml("#ABB5C6");
        public Color LightAccentBorder => ColorTranslator.FromHtml("#C9D1E0");
        public Color ActiveControl => ColorTranslator.FromHtml("#9FB2C4");
    }
}
