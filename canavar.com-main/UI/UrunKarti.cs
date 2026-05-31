using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.Models;

namespace ProductWizardApp.UI;

public class UrunKarti : UserControl
{
    public Product? karttakiUrun;

    private PictureBox pictureBox1;

    private Label lblAd;

    private Label lblFiyat;

    private Button btnEkle;

    public UrunKarti()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.lblAd = new System.Windows.Forms.Label();
        this.lblFiyat = new System.Windows.Forms.Label();
        this.btnEkle = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
        base.SuspendLayout();
        this.pictureBox1.Location = new System.Drawing.Point(5, 5);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(180, 140);
        this.pictureBox1.TabIndex = 0;
        this.pictureBox1.TabStop = false;
        this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
        this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.pictureBox1.Click += new System.EventHandler(Card_Click);
        this.lblAd.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
        this.lblAd.ForeColor = System.Drawing.Color.White;
        this.lblAd.Location = new System.Drawing.Point(190, 10);
        this.lblAd.Name = "lblAd";
        this.lblAd.Size = new System.Drawing.Size(210, 60);
        this.lblAd.TabIndex = 1;
        this.lblAd.Text = "Ürün Adı";
        this.lblAd.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblAd.Click += new System.EventHandler(Card_Click);
        this.lblFiyat.AutoSize = true;
        this.lblFiyat.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
        this.lblFiyat.ForeColor = System.Drawing.Color.Gold;
        this.lblFiyat.Location = new System.Drawing.Point(190, 80);
        this.lblFiyat.Name = "lblFiyat";
        this.lblFiyat.Size = new System.Drawing.Size(100, 25);
        this.lblFiyat.TabIndex = 2;
        this.lblFiyat.Text = "0.00 TL";
        this.lblFiyat.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblFiyat.Click += new System.EventHandler(Card_Click);
        this.btnEkle.Location = new System.Drawing.Point(195, 120);
        this.btnEkle.Name = "btnEkle";
        this.btnEkle.Size = new System.Drawing.Size(200, 35);
        this.btnEkle.TabIndex = 3;
        this.btnEkle.Text = "\ud83d\uded2 Sepete Ekle";
        this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnEkle.FlatAppearance.BorderSize = 0;
        this.btnEkle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
        this.btnEkle.ForeColor = System.Drawing.Color.White;
        this.btnEkle.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
        this.btnEkle.UseVisualStyleBackColor = true;
        this.btnEkle.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnEkle.Click += new System.EventHandler(btnEkle_Click);
        base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
        base.Controls.Add(this.btnEkle);
        base.Controls.Add(this.lblFiyat);
        base.Controls.Add(this.lblAd);
        base.Controls.Add(this.pictureBox1);
        base.Margin = new System.Windows.Forms.Padding(10);
        base.Name = "UrunKarti";
        base.Size = new System.Drawing.Size(410, 165);
        base.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.Cursor = System.Windows.Forms.Cursors.Hand;
        base.Click += new System.EventHandler(Card_Click);
        ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public void BilgileriDoldur(Product urun)
    {
        karttakiUrun = urun;
        lblAd.Text = urun.Brand + " " + urun.Name;
        lblFiyat.Text = urun.Price.ToString("N0") + " TL";
        DataService.LoadProductImage(pictureBox1, urun);
    }

    private void Card_Click(object sender, EventArgs e)
    {
        if (karttakiUrun != null)
        {
            ProductDetailForm productDetailForm = new ProductDetailForm(karttakiUrun);
            productDetailForm.ShowDialog(this);
        }
    }

    private void btnEkle_Click(object sender, EventArgs e)
    {
        Control parent = base.Parent;
        while (parent != null && !(parent is PcBuilderControl))
        {
            parent = parent.Parent;
        }
        if (parent is PcBuilderControl pcBuilderControl && karttakiUrun != null)
        {
            pcBuilderControl.SepeteEkle(karttakiUrun);
        }
    }
}
