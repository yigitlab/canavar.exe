using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.Models;

namespace ProductWizardApp.UI;

public class PcBuilderControl : UserControl
{
    private Panel panel1;

    private Button btnMonitor;

    private Button btnPsu;

    private Button btnKasa;

    private Button btnSsd;

    private Button btnRam;

    private Button btnEkranKarti;

    private Button btnIslemci;

    private Button btnAnakart;

    private FlowLayoutPanel flowLayoutPanel1;

    private Label lblToplamTutar;

    private ListBox lstSepet;

    private Button btnSil;

    private TextBox txtTaksitler;

    private readonly DataService _dataService;

    private List<Product> sepetUrunleri = new List<Product>();

    private decimal toplamTutar = default(decimal);

    private string seciliKasaBoyutu = "";

    private string seciliAnakartBoyutu = "";

    private int mevcutPSUWatt = 0;

    private int gerekenGPUWatt = 0;

    private string seciliSoket = "";

    private string seciliRamTipi = "";

    public PcBuilderControl(DataService dataService)
    {
        _dataService = dataService;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.panel1 = new System.Windows.Forms.Panel();
        this.btnMonitor = new System.Windows.Forms.Button();
        this.btnPsu = new System.Windows.Forms.Button();
        this.btnKasa = new System.Windows.Forms.Button();
        this.btnSsd = new System.Windows.Forms.Button();
        this.btnRam = new System.Windows.Forms.Button();
        this.btnEkranKarti = new System.Windows.Forms.Button();
        this.btnIslemci = new System.Windows.Forms.Button();
        this.btnAnakart = new System.Windows.Forms.Button();
        this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
        this.lblToplamTutar = new System.Windows.Forms.Label();
        this.lstSepet = new System.Windows.Forms.ListBox();
        this.btnSil = new System.Windows.Forms.Button();
        this.txtTaksitler = new System.Windows.Forms.TextBox();
        base.SuspendLayout();
        this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel1.Width = 220;
        this.panel1.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
        this.panel1.Padding = new System.Windows.Forms.Padding(5);
        this.SetupButton(this.btnAnakart, "Anakart", 0, new System.EventHandler(btnAnakart_Click));
        this.SetupButton(this.btnIslemci, "İşlemci", 1, new System.EventHandler(btnIslemci_Click));
        this.SetupButton(this.btnEkranKarti, "Ekran Kartı", 2, new System.EventHandler(btnEkranKarti_Click));
        this.SetupButton(this.btnRam, "RAM", 3, new System.EventHandler(btnRam_Click));
        this.SetupButton(this.btnSsd, "SSD", 4, new System.EventHandler(btnSsd_Click));
        this.SetupButton(this.btnKasa, "Kasa", 5, new System.EventHandler(btnKasa_Click));
        this.SetupButton(this.btnPsu, "Güç Kaynağı", 6, new System.EventHandler(btnPsu_Click));
        this.SetupButton(this.btnMonitor, "Monitör", 7, new System.EventHandler(btnMonitor_Click));
        panel.Dock = System.Windows.Forms.DockStyle.Right;
        panel.Width = 360;
        panel.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
        panel.Padding = new System.Windows.Forms.Padding(10);
        this.lblToplamTutar.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblToplamTutar.Height = 45;
        this.lblToplamTutar.ForeColor = System.Drawing.Color.Gold;
        this.lblToplamTutar.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
        this.lblToplamTutar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.lblToplamTutar.Text = "Toplam: 0 TL";
        panel.Controls.Add(this.lblToplamTutar);
        this.btnSil.Dock = System.Windows.Forms.DockStyle.Top;
        this.btnSil.Height = 35;
        this.btnSil.Text = "\ud83d\uddd1\ufe0f Seçiliyi Sil";
        this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSil.ForeColor = System.Drawing.Color.White;
        this.btnSil.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
        this.btnSil.Click += new System.EventHandler(btnSil_Click);
        panel.Controls.Add(this.btnSil);
        this.lstSepet.Dock = System.Windows.Forms.DockStyle.Top;
        this.lstSepet.Height = 200;
        this.lstSepet.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.lstSepet.ForeColor = System.Drawing.Color.White;
        this.lstSepet.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.lstSepet.Font = new System.Drawing.Font("Segoe UI", 9f);
        panel.Controls.Add(this.lstSepet);
        System.Windows.Forms.Label value = new System.Windows.Forms.Label
        {
            Text = "Ödeme Planı:",
            Dock = System.Windows.Forms.DockStyle.Top,
            Height = 25,
            ForeColor = System.Drawing.Color.LightGray,
            TextAlign = System.Drawing.ContentAlignment.BottomLeft
        };
        panel.Controls.Add(value);
        this.txtTaksitler.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtTaksitler.Multiline = true;
        this.txtTaksitler.ReadOnly = true;
        this.txtTaksitler.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.txtTaksitler.ForeColor = System.Drawing.Color.LightGray;
        this.txtTaksitler.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.txtTaksitler.Font = new System.Drawing.Font("Consolas", 9f);
        panel.Controls.Add(this.txtTaksitler);
        this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.flowLayoutPanel1.AutoScroll = true;
        this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
        this.Dock = System.Windows.Forms.DockStyle.Fill;
        base.Controls.Add(this.flowLayoutPanel1);
        base.Controls.Add(this.panel1);
        base.Controls.Add(panel);
        this.panel1.ResumeLayout(false);
        base.ResumeLayout(false);
    }

    private void SetupButton(Button btn, string text, int order, EventHandler handler)
    {
        btn.Text = text;
        btn.Height = 50;
        btn.Dock = DockStyle.Top;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.ForeColor = Color.White;
        btn.BackColor = Color.FromArgb(63, 63, 70);
        btn.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        btn.Margin = new Padding(0, 0, 0, 5);
        btn.Click += handler;
        btn.Cursor = Cursors.Hand;
        panel1.Controls.Add(btn);
        btn.BringToFront();
    }

    public void SepeteEkle(Product urun)
    {
        if (urun is CPU cPU)
        {
            if (!string.IsNullOrEmpty(seciliSoket) && cPU.Socket != seciliSoket)
            {
                MessageBox.Show("Soket Uyumsuz! Beklenen: " + seciliSoket);
                return;
            }
            seciliSoket = cPU.Socket;
        }
        if (urun is Motherboard motherboard)
        {
            if (!string.IsNullOrEmpty(seciliSoket) && motherboard.Soket != seciliSoket)
            {
                MessageBox.Show("Soket Uyumsuz! Beklenen: " + seciliSoket);
                return;
            }
            seciliSoket = motherboard.Soket;
            string kasaTipi = motherboard.KasaTipi;
            if (!string.IsNullOrEmpty(seciliKasaBoyutu) && seciliKasaBoyutu == "ITX" && kasaTipi != "ITX")
            {
                MessageBox.Show("Hata: Bu anakart seçtiğiniz küçük kasaya sığmaz!");
                return;
            }
            seciliAnakartBoyutu = kasaTipi;
            seciliRamTipi = motherboard.RamTipi;
        }
        if (urun is PC_Case pC_Case)
        {
            if (!string.IsNullOrEmpty(seciliAnakartBoyutu) && pC_Case.FormFactor == "ITX" && seciliAnakartBoyutu != "ITX")
            {
                MessageBox.Show("Hata: Seçili anakartınız bu kasaya sığmayacak kadar büyük!");
                return;
            }
            seciliKasaBoyutu = pC_Case.FormFactor;
        }
        if (urun is GPU gPU)
        {
            if (mevcutPSUWatt > 0 && mevcutPSUWatt < gPU.MinimumGucKaynagi)
            {
                MessageBox.Show($"Uyarı: Güç kaynağınız ({mevcutPSUWatt}W), bu kart için önerilen ({gPU.MinimumGucKaynagi}W) değerinden düşük!");
                return;
            }
            gerekenGPUWatt = gPU.MinimumGucKaynagi;
        }
        if (urun is PSU pSU)
        {
            if (gerekenGPUWatt > 0 && pSU.Wattage < gerekenGPUWatt)
            {
                MessageBox.Show("Uyarı: Bu güç kaynağı sepetteki ekran kartı için yetersiz kalabilir!");
                return;
            }
            mevcutPSUWatt = pSU.Wattage;
        }
        if (urun is RAM rAM && !string.IsNullOrEmpty(seciliRamTipi) && rAM.RamTipi != seciliRamTipi)
        {
            MessageBox.Show($"Uyarı: RAM tipi ({rAM.RamTipi}) anakart desteğiyle ({seciliRamTipi}) uyumsuz olabilir!");
            return;
        }
        lstSepet.Items.Add(urun.Name + " - " + urun.Price.ToString("N0") + " TL");
        sepetUrunleri.Add(urun);
        toplamTutar += urun.Price;
        lblToplamTutar.Text = "Toplam Tutar: " + toplamTutar.ToString("N0") + " TL";
        TaksitTablosunuGuncelle();
    }

    private void UrunleriListele(List<Product> liste)
    {
        flowLayoutPanel1.Controls.Clear();
        foreach (Product item in liste)
        {
            UrunKarti urunKarti = new UrunKarti();
            urunKarti.BilgileriDoldur(item);
            flowLayoutPanel1.Controls.Add(urunKarti);
        }
    }

    private void btnAnakart_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<Motherboard>().Cast<Product>().ToList());
    }

    private void btnIslemci_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<CPU>().Cast<Product>().ToList());
    }

    private void btnEkranKarti_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<GPU>().Cast<Product>().ToList());
    }

    private void btnRam_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<RAM>().Cast<Product>().ToList());
    }

    private void btnSsd_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<Storage>().Cast<Product>().ToList());
    }

    private void btnKasa_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<PC_Case>().Cast<Product>().ToList());
    }

    private void btnPsu_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<PSU>().Cast<Product>().ToList());
    }

    private void btnMonitor_Click(object sender, EventArgs e)
    {
        UrunleriListele(_dataService.GetProductsByType<ProductWizardApp.Models.Monitor>().Cast<Product>().ToList());
    }

    private void btnSil_Click(object sender, EventArgs e)
    {
        int selectedIndex = lstSepet.SelectedIndex;
        if (selectedIndex == -1)
        {
            MessageBox.Show("Lütfen listeden silmek istediğiniz ürünü seçin.");
            return;
        }
        try
        {
            Product product = sepetUrunleri[selectedIndex];
            toplamTutar -= product.Price;
            sepetUrunleri.RemoveAt(selectedIndex);
            lstSepet.Items.RemoveAt(selectedIndex);
            lblToplamTutar.Text = "Toplam Tutar: " + toplamTutar.ToString("N0") + " TL";
            TaksitTablosunuGuncelle();
            if (sepetUrunleri.Count == 0)
            {
                seciliSoket = "";
                seciliRamTipi = "";
                seciliKasaBoyutu = "";
                seciliAnakartBoyutu = "";
                mevcutPSUWatt = 0;
                gerekenGPUWatt = 0;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
        }
    }

    public void TaksitTablosunuGuncelle()
    {
        txtTaksitler.Clear();
        if (toplamTutar == 0m)
        {
            txtTaksitler.Text = "Sepet boş, taksit hesaplanamadı.";
            return;
        }
        int[] array = new int[5] { 1, 3, 6, 9, 12 };
        txtTaksitler.AppendText("--- ÖDEME PLANI ---" + Environment.NewLine);
        int[] array2 = array;
        foreach (int num in array2)
        {
            double num2 = 1.0;
            switch (num)
            {
                case 6:
                    num2 = 1.05;
                    break;
                case 9:
                    num2 = 1.08;
                    break;
                case 12:
                    num2 = 1.1;
                    break;
            }
            double num3 = (double)toplamTutar * num2;
            double num4 = num3 / (double)num;
            string text = $"{num} Taksit: {num4:N2} TL x {num} (Toplam: {num3:N2} TL)";
            txtTaksitler.AppendText(text + Environment.NewLine);
        }
    }
}
