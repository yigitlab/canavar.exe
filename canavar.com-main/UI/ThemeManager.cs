using System;
using System.Collections.Generic;

namespace ProductWizardApp.UI;

public static class ThemeManager
{
    public static readonly IReadOnlyList<AppTheme> Themes = new List<AppTheme>
    {
        new AppTheme("Stormy Morning", "#6a89a7", "#bdddfc", "#88bdf2", "#384959"),
        new AppTheme("Ink Wash", "#4a4a4a", "#cbcbcb", "#ffffe3", "#6d8196"),
        new AppTheme("Fresh Peach", "#ffd3ac", "#ffb5ab", "#e39a7b", "#dbb06b"),
        new AppTheme("Guava", "#ff8559", "#ffb578", "#e65447", "#cf5376"),
        new AppTheme("HasanHocamKraldır", "#f95c4b", "#f6f4f1", "#e4ded2", "#000000")
    };

    private static AppTheme _current = Themes[0];

    public static AppTheme Current => _current;

    public static event EventHandler<AppTheme>? ThemeChanged;

    public static void Apply(AppTheme theme)
    {
        if (theme != _current)
        {
            _current = theme;
            ThemeManager.ThemeChanged?.Invoke(null, theme);
        }
    }

    public static void Apply(int index)
    {
        Apply(Themes[index]);
    }
}
