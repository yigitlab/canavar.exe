namespace ProductWizardApp;

using System;
using System.Drawing;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.UI;

public partial class Form1 : Form
{
    private DataService _dataService;
    private TabControl _tabControl = null!;
    private Button _btnSettings = null!;
    private Panel _titleBar = null!;
    private Panel _pnlNewsTicker = null!;
    private FlowLayoutPanel _flpNewsRow = null!;
    private Label _lblNewsTitle = null!;
    private LinkLabel _lnkShowMore = null!;
    private HazirSistemlerControl _hazirControl = null!;

    public Form1(DataService dataService)
    {
        _dataService = dataService;
        InitializeComponent();
        SetupUI();

        // Tema değişimlerini dinle
        ThemeManager.ThemeChanged += OnThemeChanged;
        ApplyTheme(ThemeManager.Current);

        LoadNewsTickerAsync();
    }

    private void SetupUI()
    {
        this.Text = "canavar.com";
        this.Size = new Size(1000, 720);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(800, 600);
        this.Font = new Font("Segoe UI", 10);

        string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "canavar.ico");
        if (File.Exists(iconPath))
        {
            try { this.Icon = new Icon(iconPath); } catch { }
        }

        // ── Özel başlık çubuğu ────────────────────────────────────────
        _titleBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 48,
            BackColor = ThemeManager.Current.Dark
        };

        // Uygulama başlığı
        var lblTitle = new Label
        {
            Name = "lblTitle",
            Text = "⚡  canavar.com",
            Dock = DockStyle.Fill,
            ForeColor = ThemeManager.Current.TextOnDark,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(16, 0, 0, 0)
        };

        // ⚙ Ayarlar butonu
        _btnSettings = new Button
        {
            Text = "⚙",
            Width = 48,
            Height = 48,
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ForeColor = ThemeManager.Current.TextOnDark,
            Font = new Font("Segoe UI", 16),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        _btnSettings.FlatAppearance.BorderSize = 0;
        _btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 255, 255, 255);
        _btnSettings.FlatAppearance.MouseDownBackColor = Color.FromArgb(60, 0, 0, 0);
        _btnSettings.Click += BtnSettings_Click;

        _titleBar.Controls.Add(lblTitle);
        _titleBar.Controls.Add(_btnSettings);

        // ── Tab kontrolü ──────────────────────────────────────────────
        _tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11),
            Appearance = TabAppearance.FlatButtons
        };

        var compareControl = new CompareWizardControl(_dataService);

        TabPage tabCompare = new TabPage("  Ürün Karşılaştırma  ");
        tabCompare.Controls.Add(compareControl);

        TabPage tabBuilder = new TabPage("  Bilgisayar Toplama  ");
        var pcBuilder = new PcBuilderControl(_dataService);
        tabBuilder.Controls.Add(pcBuilder);

        _tabControl.TabPages.Add(tabCompare);
        _tabControl.TabPages.Add(tabBuilder);

        TabPage tabHazir = new TabPage("  Hazır Sistemler  ");
        _hazirControl = new HazirSistemlerControl();
        tabHazir.Controls.Add(_hazirControl);
        _tabControl.TabPages.Add(tabHazir);

        // ── Haberler Paneli (Alt Kısım) ───────────────────────────────
        _pnlNewsTicker = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 180,
            Padding = new Padding(10),
            BackColor = ThemeManager.Current.Light
        };

        _lblNewsTitle = new Label
        {
            Text = "📰 Son Teknoloji Haberleri",
            Dock = DockStyle.Top,
            Height = 24,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = ThemeManager.Current.Dark
        };

        _lnkShowMore = new LinkLabel
        {
            Text = "Devamını Göster...",
            Dock = DockStyle.Bottom,
            Height = 24,
            TextAlign = ContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            LinkColor = ThemeManager.Current.Primary,
            ActiveLinkColor = ThemeManager.Current.Secondary,
            Cursor = Cursors.Hand
        };
        _lnkShowMore.LinkClicked += (s, e) => ShowAllNewsTab();

        _flpNewsRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight
        };

        _pnlNewsTicker.Controls.Add(_flpNewsRow);
        _pnlNewsTicker.Controls.Add(_lblNewsTitle);
        _pnlNewsTicker.Controls.Add(_lnkShowMore);

        // Z-order ve Dock düzeni
        this.Controls.Add(_tabControl);
        this.Controls.Add(_pnlNewsTicker);
        this.Controls.Add(_titleBar);
    }

    // ── Ayarlar butonu: pop-up'ı butonun altına konumlandır ──────────
    private void BtnSettings_Click(object? sender, EventArgs e)
    {
        var popup = new ThemeSettingsPopup();

        // Pop-up'ın sol üstünü butonun sol altına hizala
        Point screenPos = _btnSettings.PointToScreen(new Point(0, _btnSettings.Height));
        int popupLeft = screenPos.X - popup.Width + _btnSettings.Width;
        popup.Location = new Point(popupLeft, screenPos.Y + 4);

        popup.Show(this);
    }

    // ── Tema değişimi ─────────────────────────────────────────────────
    private void OnThemeChanged(object? sender, AppTheme theme)
    {
        if (this.InvokeRequired) { this.Invoke(() => ApplyTheme(theme)); return; }
        ApplyTheme(theme);
    }

    private void ApplyTheme(AppTheme theme)
    {
        // Başlık çubuğu
        _titleBar.BackColor = theme.Dark;
        _btnSettings.ForeColor = theme.TextOnDark;

        if (_titleBar.Controls["lblTitle"] is Label lbl)
        {
            lbl.ForeColor = theme.TextOnDark;
        }

        // Tab kontrolü zemin
        _tabControl.BackColor = theme.Light;

        // Tüm tab sayfaları + içerikleri tema uygula
        foreach (TabPage tab in _tabControl.TabPages)
        {
            tab.BackColor = theme.Light;
        }
        _hazirControl?.ApplyTheme(theme);

        // Form arka planı
        this.BackColor = theme.Light;

        if (_pnlNewsTicker != null)
        {
            _pnlNewsTicker.BackColor = theme.Light;
            if (_lblNewsTitle != null) _lblNewsTitle.ForeColor = theme.Dark;
            if (_lnkShowMore != null)
            {
                _lnkShowMore.LinkColor = theme.Primary;
                _lnkShowMore.ActiveLinkColor = theme.Secondary;
            }
        }
    }

    private async void LoadNewsTickerAsync()
    {
        var news = await NewsService.GetTechnologyNewsAsync();
        _flpNewsRow.Controls.Clear();

        foreach (var item in news)
        {
            var card = new Panel { Width = 280, Height = 100, Margin = new Padding(0, 0, 10, 0), Cursor = Cursors.Hand };

            var pic = new PictureBox { Width = 90, Height = 90, Left = 5, Top = 5, SizeMode = PictureBoxSizeMode.Zoom };
            if (!string.IsNullOrEmpty(item.UrlToImage))
            {
                try { pic.LoadAsync(item.UrlToImage); } catch { }
            }

            var lbl = new Label
            {
                Text = item.Title,
                Left = 100,
                Top = 5,
                Width = 175,
                Height = 90,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoEllipsis = true
            };

            card.Controls.Add(pic);
            card.Controls.Add(lbl);

            // Habere tıklayınca tarayıcıda aç
            void OpenUrl(object? sender, EventArgs e)
            {
                try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = item.Url, UseShellExecute = true }); } catch { }
            }
            card.Click += OpenUrl;
            pic.Click += OpenUrl;
            lbl.Click += OpenUrl;

            _flpNewsRow.Controls.Add(card);
        }
    }

    private async void ShowAllNewsTab()
    {
        // Sekme zaten varsa odaklan
        foreach (TabPage tab in _tabControl.TabPages)
        {
            if (tab.Text == "  Teknoloji Haberleri  ")
            {
                _tabControl.SelectedTab = tab;
                return;
            }
        }

        var newsTab = new TabPage("  Teknoloji Haberleri  ");
        newsTab.BackColor = ThemeManager.Current.Light;

        var flowLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(20),
            BackColor = ThemeManager.Current.Light
        };

        var loadingLabel = new Label { Text = "Haberler yükleniyor...", AutoSize = true, Font = new Font("Segoe UI", 12) };
        flowLayout.Controls.Add(loadingLabel);
        newsTab.Controls.Add(flowLayout);

        _tabControl.TabPages.Add(newsTab);
        _tabControl.SelectedTab = newsTab;

        var news = await NewsService.GetTechnologyNewsAsync();
        flowLayout.Controls.Clear();

        foreach (var item in news)
        {
            var card = new Panel { Width = 300, Height = 300, Margin = new Padding(10), BackColor = Color.White, Cursor = Cursors.Hand };

            var pic = new PictureBox { Width = 300, Height = 200, Left = 0, Top = 0, SizeMode = PictureBoxSizeMode.Zoom };
            if (!string.IsNullOrEmpty(item.UrlToImage))
            {
                try { pic.LoadAsync(item.UrlToImage); } catch { }
            }

            var lbl = new Label
            {
                Text = item.Title,
                Left = 10,
                Top = 210,
                Width = 280,
                Height = 80,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoEllipsis = true
            };

            card.Controls.Add(pic);
            card.Controls.Add(lbl);

            // Habere tıklayınca tarayıcıda aç
            void OpenUrl(object? sender, EventArgs e)
            {
                try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = item.Url, UseShellExecute = true }); } catch { }
            }
            card.Click += OpenUrl;
            pic.Click += OpenUrl;
            lbl.Click += OpenUrl;

            flowLayout.Controls.Add(card);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        ThemeManager.ThemeChanged -= OnThemeChanged;
        base.OnFormClosed(e);
    }
}
