using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.Models;

namespace ProductWizardApp.UI
{
    public partial class HazirSistemKarti : UserControl
    {
        public HazirSistem? karttakiSistem;

        public HazirSistemKarti()
        {
            InitializeComponent();
            ApplyTheme(ThemeManager.Current);
        }

        public void BilgileriDoldur(HazirSistem sistem)
        {
            this.karttakiSistem = sistem;
            lblSistemAdi.Text = sistem.SistemAdi;
            lblFiyat.Text = sistem.ToplamFiyat.ToString("N0") + " TL";

            // Load Image with high-quality fallback
            LoadSistemImage(sistem);

            // Populate Specs
            string detayliListe = "";
            foreach (var p in sistem.Parcalar)
            {
                detayliListe += $"• {p.Kategori}: {p.Ad}\n";
            }
            lblDonanimlar.Text = detayliListe;
        }

        private void LoadSistemImage(HazirSistem sistem)
        {
            pbKasa.Image?.Dispose();
            pbKasa.Image = null;

            string baseDir = DataService.GetImagesDir();
            bool loaded = false;

            if (!string.IsNullOrEmpty(sistem.ResimYolu) && File.Exists(sistem.ResimYolu))
            {
                try
                {
                    using (FileStream fs = new FileStream(sistem.ResimYolu, FileMode.Open, FileAccess.Read))
                    {
                        pbKasa.Image = Image.FromStream(fs);
                    }
                    loaded = true;
                }
                catch { }
            }

            if (!loaded && !string.IsNullOrEmpty(baseDir))
            {
                // Fallback: Choose a nice case image from Images\Products based on price
                string fallbackImageName = "NZXT H510.png";
                if (sistem.ToplamFiyat >= 100000)
                {
                    fallbackImageName = "Lian Li PC-O11 XL.png";
                }
                else if (sistem.ToplamFiyat >= 50000)
                {
                    fallbackImageName = "Fractal Design Torrent.png";
                }
                else if (sistem.ToplamFiyat >= 25000)
                {
                    fallbackImageName = "Corsair 5000D Airflow.png";
                }

                string fullPath = Path.Combine(baseDir, fallbackImageName);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            pbKasa.Image = Image.FromStream(fs);
                        }
                        loaded = true;
                    }
                    catch { }
                }
            }

            if (!loaded)
            {
                pbKasa.BackColor = Color.FromArgb(40, 40, 40);
            }
        }

        public void ApplyTheme(AppTheme theme)
        {
            this.BackColor = theme.Light;
            lblSistemAdi.ForeColor = theme.Dark;
            lblDonanimlar.ForeColor = theme.TextOnLight;
            lblFiyat.ForeColor = Color.DarkGoldenrod;
            
            btnSepeteEkle.BackColor = theme.Primary;
            btnSepeteEkle.ForeColor = theme.TextOnPrimary;
            btnSepeteEkle.FlatAppearance.BorderColor = theme.Dark;
            
            this.Invalidate();
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            Control parent = base.Parent;
            while (parent != null && !(parent is HazirSistemlerControl))
            {
                parent = parent.Parent;
            }

            if (parent is HazirSistemlerControl hazirSistemlerControl && karttakiSistem != null)
            {
                hazirSistemlerControl.SepeteEkle(karttakiSistem);
            }
        }

        private void HazirSistemKarti_Paint(object sender, PaintEventArgs e)
        {
            // Draw a subtle border
            using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1f))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }
}
