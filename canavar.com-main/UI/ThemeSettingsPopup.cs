using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProductWizardApp.UI;

public class ThemeSettingsPopup : Form
{
    public ThemeSettingsPopup()
    {
        BuildUI();
        ApplyCurrentTheme(ThemeManager.Current);
    }

    private void BuildUI()
    {
        Text = "Tema Seçimi";
        base.FormBorderStyle = FormBorderStyle.None;
        base.StartPosition = FormStartPosition.Manual;
        base.Size = new Size(320, 60 + ThemeManager.Themes.Count * 76 + 20);
        base.ShowInTaskbar = false;
        base.TopMost = true;
        BackColor = Color.White;
        SetRoundedRegion();
        base.Deactivate += delegate
        {
            Close();
        };
        base.FormClosed += delegate
        {
            Dispose();
        };
        Panel titlePanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = ThemeManager.Current.Dark
        };
        Label lblTitle = new Label
        {
            Text = "⚙  Tema Seç",
            Dock = DockStyle.Fill,
            ForeColor = ThemeManager.Current.TextOnDark,
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 0, 0)
        };
        titlePanel.Controls.Add(lblTitle);
        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(12, 10, 12, 10)
        };
        foreach (AppTheme theme in ThemeManager.Themes)
        {
            Panel themeCard = BuildThemeCard(theme);
            flowLayoutPanel.Controls.Add(themeCard);
        }
        base.Controls.Add(flowLayoutPanel);
        base.Controls.Add(titlePanel);
    }

    private Panel BuildThemeCard(AppTheme theme)
    {
        bool isActive = ThemeManager.Current == theme;
        Panel card = new Panel
        {
            Width = 296,
            Height = 64,
            Margin = new Padding(0, 0, 0, 8),
            BackColor = (isActive ? theme.Light : Color.FromArgb(245, 245, 245)),
            Cursor = Cursors.Hand,
            Tag = theme
        };
        int stripeWidth = 14;
        Color[] themeColors = new Color[4] { theme.Primary, theme.Light, theme.Secondary, theme.Dark };
        for (int i = 0; i < 4; i++)
        {
            Panel colorStripe = new Panel
            {
                Width = stripeWidth,
                Height = 64,
                Left = i * stripeWidth,
                Top = 0,
                BackColor = themeColors[i]
            };
            card.Controls.Add(colorStripe);
        }
        Label lblName = new Label
        {
            Text = theme.Name,
            Left = 4 * stripeWidth + 12,
            Top = 10,
            Width = 200,
            Height = 22,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = (isActive ? theme.Dark : Color.FromArgb(60, 60, 60))
        };
        card.Controls.Add(lblName);
        Label lblHexList = new Label
        {
            Text = $"{ColorToHex(theme.Primary)}  {ColorToHex(theme.Secondary)}  {ColorToHex(theme.Dark)}",
            Left = 4 * stripeWidth + 12,
            Top = 36,
            Width = 240,
            Height = 18,
            Font = new Font("Segoe UI", 8f),
            ForeColor = Color.Gray
        };
        card.Controls.Add(lblHexList);
        if (isActive)
        {
            Label lblCheck = new Label
            {
                Text = "✓",
                Left = card.Width - 32,
                Top = 20,
                Width = 22,
                Height = 22,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = theme.Primary,
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblCheck);
        }
        EventHandler clickHandler = delegate
        {
            ThemeManager.Apply(theme);
            Close();
        };
        card.Click += clickHandler;
        foreach (Control control in card.Controls)
        {
            control.Click += clickHandler;
        }
        card.MouseEnter += delegate
        {
            card.BackColor = Color.FromArgb(230, 240, 255);
        };
        card.MouseLeave += delegate
        {
            card.BackColor = (isActive ? theme.Light : Color.FromArgb(245, 245, 245));
        };
        return card;
    }

    private void ApplyCurrentTheme(AppTheme theme)
    {
        if (base.Controls.Count > 0 && base.Controls[base.Controls.Count - 1] is Panel titlePanel)
        {
            titlePanel.BackColor = theme.Dark;
            if (titlePanel.Controls.Count > 0 && titlePanel.Controls[0] is Label label)
            {
                label.ForeColor = theme.TextOnDark;
            }
        }
    }

    private void SetRoundedRegion()
    {
        int cornerRadius = 14;
        using GraphicsPath graphicsPath = new GraphicsPath();
        Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
        graphicsPath.AddArc(rectangle.X, rectangle.Y, cornerRadius, cornerRadius, 180f, 90f);
        graphicsPath.AddArc(rectangle.Right - cornerRadius, rectangle.Y, cornerRadius, cornerRadius, 270f, 90f);
        graphicsPath.AddArc(rectangle.Right - cornerRadius, rectangle.Bottom - cornerRadius, cornerRadius, cornerRadius, 0f, 90f);
        graphicsPath.AddArc(rectangle.X, rectangle.Bottom - cornerRadius, cornerRadius, cornerRadius, 90f, 90f);
        graphicsPath.CloseFigure();
        base.Region = new Region(graphicsPath);
    }

    private static string ColorToHex(Color c)
    {
        return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
    }
}
