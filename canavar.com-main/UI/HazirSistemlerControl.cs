using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.Models;

namespace ProductWizardApp.UI
{
    public class HazirSistemlerControl : UserControl
    {
        private Panel leftPanel = null!;
        private FlowLayoutPanel cardFlow = null!;
        private Panel rightPanel = null!;
        private ListBox lstSepet = null!;
        private Label lblToplamTutar = null!;
        private Button btnTemizle = null!;
        private TextBox txtTaksitler = null!;

        private int toplamTutar = 0;
        private List<HazirSistem> hazirSistemler = new List<HazirSistem>();

        public HazirSistemlerControl()
        {
            InitializeComponent();
            LoadHazirSistemler();
            
            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme(ThemeManager.Current);
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);

            // Left panel (FlowLayout containing HazirSistemKarti)
            leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            cardFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            leftPanel.Controls.Add(cardFlow);

            // Right panel (Basket and Installments)
            rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 320,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(45, 45, 48)
            };

            lblToplamTutar = new Label
            {
                Text = "Toplam Tutar: 0 TL",
                Dock = DockStyle.Top,
                Height = 45,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.Gold,
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnTemizle = new Button
            {
                Text = "🗑 Sepeti Temizle",
                Dock = DockStyle.Top,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnTemizle.FlatAppearance.BorderSize = 0;
            btnTemizle.Click += BtnTemizle_Click;

            lstSepet = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 220,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F)
            };

            Label lblTaksitTitle = new Label
            {
                Text = "Taksit Seçenekleri:",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.BottomLeft
            };

            txtTaksitler = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.LightGray,
                Font = new Font("Consolas", 9F)
            };

            rightPanel.Controls.Add(txtTaksitler);
            rightPanel.Controls.Add(lblTaksitTitle);
            rightPanel.Controls.Add(btnTemizle);
            rightPanel.Controls.Add(lstSepet);
            rightPanel.Controls.Add(lblToplamTutar);

            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);

            TaksitleriHesapla();
        }

        private HazirSistem SistemOlustur(string ad, int fiyat, string cpu, string gpu, string mobo, string ram, string ssd)
        {
            HazirSistem s = new HazirSistem { SistemAdi = ad, ToplamFiyat = fiyat };
            s.Parcalar.Add(new Parca { Kategori = "İşlemci", Ad = cpu });
            s.Parcalar.Add(new Parca { Kategori = "Ekran Kartı", Ad = gpu });
            s.Parcalar.Add(new Parca { Kategori = "Anakart", Ad = mobo });
            s.Parcalar.Add(new Parca { Kategori = "RAM", Ad = ram });
            s.Parcalar.Add(new Parca { Kategori = "Depolama", Ad = ssd });
            return s;
        }

        private void LoadHazirSistemler()
        {
            // Giriş Seviyesi
            hazirSistemler.Add(SistemOlustur("Vortex", 14500, "Intel Core i3-12100F", "Intel Arc A380", "Gigabyte B760 DS3H", "16GB", "500GB"));
            hazirSistemler.Add(SistemOlustur("Phantom", 16800, "AMD Ryzen 5 5500", "NVIDIA RTX 3050", "ASUS PRIME B550-PLUS", "16GB", "500GB"));
            hazirSistemler.Add(SistemOlustur("Specter", 18200, "Intel Core i5-12400F", "AMD RX 6600", "MSI PRO Z790-A", "16GB", "500GB"));
            hazirSistemler.Add(SistemOlustur("Mirage", 21000, "AMD Ryzen 5 5600", "NVIDIA RTX 3060", "MSI MAG B550 TOMAHAWK", "16GB", "500GB"));
            hazirSistemler.Add(SistemOlustur("Horizon", 23500, "Intel Core i5-13400F", "NVIDIA RTX 4060", "ASUS PRIME Z790-P", "16GB", "500GB"));

            // Orta Seviye
            hazirSistemler.Add(SistemOlustur("Raptor", 27000, "AMD Ryzen 5 7500F", "AMD RX 7600", "ASUS PRIME B650-PLUS", "32GB", "1TB"));
            hazirSistemler.Add(SistemOlustur("Frostbite", 31000, "Intel Core i5-13600K", "NVIDIA RTX 4060 Ti", "MSI MAG Z790 TOMAHAWK", "32GB", "1TB"));
            hazirSistemler.Add(SistemOlustur("Avalanche", 35500, "AMD Ryzen 5 7600X", "AMD RX 7700 XT", "ASUS TUF GAMING B650-PLUS", "32GB", "1TB"));
            hazirSistemler.Add(SistemOlustur("Cyclone", 41000, "Intel Core i7-13700K", "NVIDIA RTX 4070", "Gigabyte Z790 AORUS Elite", "32GB", "1TB"));
            hazirSistemler.Add(SistemOlustur("Spitfire", 46000, "AMD Ryzen 7 7700X", "AMD RX 7800 XT", "Gigabyte B650 AORUS Elite", "32GB", "1TB"));

            // Üst Seviye
            hazirSistemler.Add(SistemOlustur("Vanguard", 54000, "AMD Ryzen 7 5800X3D", "NVIDIA RTX 3080", "ASUS ROG Crosshair VIII", "32GB", "1TB"));
            hazirSistemler.Add(SistemOlustur("Obsidian", 62000, "Intel Core i7-14700K", "NVIDIA RTX 4070 Super", "ASUS ROG Strix Z790-E", "32GB", "1TB"));
            hazirSistemler.Add(SistemOlustur("Warmonger", 71000, "AMD Ryzen 7 7800X3D", "AMD RX 7900 XT", "MSI MAG B650 TOMAHAWK", "64GB", "2TB"));
            hazirSistemler.Add(SistemOlustur("SubZero", 84000, "Intel Core i9-13900K", "NVIDIA RTX 4080", "MSI MEG Z790 ACE", "64GB", "2TB"));
            hazirSistemler.Add(SistemOlustur("Doomsday", 96000, "AMD Ryzen 9 7950X3D", "AMD RX 7900 XTX", "Gigabyte X670E AORUS Master", "64GB", "2TB"));

            // Ultimate Seviye
            hazirSistemler.Add(SistemOlustur("Overlord", 112000, "Intel Core i9-14900K", "NVIDIA RTX 4090", "ASUS ROG Maximus Z790", "64GB", "2TB"));
            hazirSistemler.Add(SistemOlustur("Juggernaut", 128000, "AMD Ryzen 9 7950X", "NVIDIA RTX 4090", "ASUS ROG Strix X670E-F", "128GB", "4TB"));
            hazirSistemler.Add(SistemOlustur("Nemesis", 145000, "Intel Core i9-14900K", "NVIDIA RTX 4090", "ASRock Z790 Taichi", "128GB", "4TB"));
            hazirSistemler.Add(SistemOlustur("Leviathan", 170000, "AMD Ryzen 9 7950X3D", "NVIDIA RTX 4090", "Gigabyte X670E AORUS Master", "128GB", "8TB"));
            hazirSistemler.Add(SistemOlustur("Ragnarok", 195000, "Intel Core i9-14900K", "NVIDIA RTX 4090", "MSI MEG Z790 ACE", "128GB", "8TB"));

            // Yeni Nesil
            hazirSistemler.Add(SistemOlustur("Nova", 115000, "Intel Core i7-14700K", "NVIDIA RTX 5060 Ti 16GB", "ASUS ROG Strix Z790-E", "32GB", "2TB"));
            hazirSistemler.Add(SistemOlustur("Supernova", 138000, "Intel Core i9-13900K", "NVIDIA RTX 5070 Ti 16GB", "Gigabyte Z790 AORUS Elite", "64GB", "2TB"));
            hazirSistemler.Add(SistemOlustur("Singularity", 165000, "Intel Core i9-14900K", "NVIDIA RTX 5080 16GB", "ASUS ROG Maximus Z790", "128GB", "4TB"));
            hazirSistemler.Add(SistemOlustur("Armageddon", 220000, "Intel Core i9-14900K", "NVIDIA RTX 5090 32GB", "MSI MEG Z790 ACE", "128GB", "8TB"));

            ListHazirSistemler();
        }

        private void ListHazirSistemler()
        {
            cardFlow.Controls.Clear();
            foreach (var sistem in hazirSistemler.OrderBy(x => x.ToplamFiyat))
            {
                HazirSistemKarti kart = new HazirSistemKarti();
                kart.BilgileriDoldur(sistem);
                cardFlow.Controls.Add(kart);
            }
        }

        public void SepeteEkle(HazirSistem sistem)
        {
            if (sistem == null) return;

            lstSepet.Items.Add("=================================");
            lstSepet.Items.Add("SİSTEM: " + sistem.SistemAdi.ToUpper());
            lstSepet.Items.Add("=================================");

            foreach (var parca in sistem.Parcalar)
            {
                lstSepet.Items.Add($"  • {parca.Kategori}: {parca.Ad}");
            }

            lstSepet.Items.Add(" ");

            toplamTutar += sistem.ToplamFiyat;
            lblToplamTutar.Text = "Toplam Tutar: " + toplamTutar.ToString("N0") + " TL";

            TaksitleriHesapla();
        }

        private void TaksitleriHesapla()
        {
            txtTaksitler.Clear();
            if (toplamTutar == 0)
            {
                txtTaksitler.Text = "Sepet boş, taksit planı bulunmuyor.";
                return;
            }

            txtTaksitler.AppendText("--- ÖDEME PLANI ---" + Environment.NewLine);
            for (int i = 1; i <= 12; i++)
            {
                if (i != 1 && i != 3 && i != 6 && i != 9 && i != 12) continue;

                double odenecekTutar = toplamTutar;
                string aciklama = "";

                if (i <= 3)
                {
                    double taksit = odenecekTutar / i;
                    aciklama = $"{i} Taksit (Faizsiz): {i} x {taksit:N2} TL";
                }
                else
                {
                    double faizOrani = i == 6 ? 1.05 : (i == 9 ? 1.08 : 1.10);
                    odenecekTutar = toplamTutar * faizOrani;
                    double taksit = odenecekTutar / i;
                    aciklama = $"{i} Taksit: {i} x {taksit:N2} TL (Toplam: {odenecekTutar:N0} TL)";
                }

                txtTaksitler.AppendText(aciklama + Environment.NewLine);
            }
        }

        private void BtnTemizle_Click(object? sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show("Sepetteki tüm hazır sistemler ve taksit planı temizlenecek. Emin misiniz?",
                                                "Sepeti Temizle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (soru == DialogResult.Yes)
            {
                lstSepet.Items.Clear();
                toplamTutar = 0;
                lblToplamTutar.Text = "Toplam Tutar: 0 TL";
                TaksitleriHesapla();
                MessageBox.Show("Sepet başarıyla sıfırlandı.");
            }
        }

        private void OnThemeChanged(object? sender, AppTheme theme)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => ApplyTheme(theme));
                return;
            }
            ApplyTheme(theme);
        }

        public void ApplyTheme(AppTheme theme)
        {
            this.BackColor = theme.Light;
            cardFlow.BackColor = theme.Light;
            leftPanel.BackColor = theme.Light;

            rightPanel.BackColor = theme.Dark;
            lblToplamTutar.ForeColor = Color.Gold;

            lstSepet.BackColor = Color.FromArgb(Math.Max(0, theme.Dark.R - 10), Math.Max(0, theme.Dark.G - 10), Math.Max(0, theme.Dark.B - 10));
            lstSepet.ForeColor = theme.TextOnDark;

            txtTaksitler.BackColor = Color.FromArgb(Math.Max(0, theme.Dark.R - 10), Math.Max(0, theme.Dark.G - 10), Math.Max(0, theme.Dark.B - 10));
            txtTaksitler.ForeColor = Color.LightGray;

            // Apply theme to children inside FlowLayoutPanel
            foreach (Control control in cardFlow.Controls)
            {
                if (control is HazirSistemKarti card)
                {
                    card.ApplyTheme(theme);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeManager.ThemeChanged -= OnThemeChanged;
            }
            base.Dispose(disposing);
        }
    }
}
