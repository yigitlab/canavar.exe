using System.Drawing;

namespace ProductWizardApp.UI;

public class AppTheme
{
    public string Name { get; }

    public Color Primary { get; }

    public Color Light { get; }

    public Color Secondary { get; }

    public Color Dark { get; }

    public Color TextOnPrimary => IsDark(Primary) ? Color.White : Color.FromArgb(30, 30, 30);

    public Color TextOnLight => IsDark(Light) ? Color.White : Color.FromArgb(30, 30, 30);

    public Color TextOnDark => IsDark(Dark) ? Color.White : Color.FromArgb(30, 30, 30);

    public Color TextOnSecondary => IsDark(Secondary) ? Color.White : Color.FromArgb(30, 30, 30);

    public AppTheme(string name, string primary, string light, string secondary, string dark)
    {
        Name = name;
        Primary = ColorTranslator.FromHtml(primary);
        Light = ColorTranslator.FromHtml(light);
        Secondary = ColorTranslator.FromHtml(secondary);
        Dark = ColorTranslator.FromHtml(dark);
    }

    private static bool IsDark(Color c)
    {
        return (c.R * 299 + c.G * 587 + c.B * 114) / 1000 < 128;
    }
}
