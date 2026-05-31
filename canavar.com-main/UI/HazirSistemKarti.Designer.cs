namespace ProductWizardApp.UI
{
    partial class HazirSistemKarti
    {
        /// <summary> 
        /// Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        /// <param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        /// içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbKasa = new System.Windows.Forms.PictureBox();
            this.lblSistemAdi = new System.Windows.Forms.Label();
            this.lblDonanimlar = new System.Windows.Forms.Label();
            this.lblFiyat = new System.Windows.Forms.Label();
            this.btnSepeteEkle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbKasa)).BeginInit();
            this.SuspendLayout();
            // 
            // pbKasa
            // 
            this.pbKasa.Location = new System.Drawing.Point(10, 40);
            this.pbKasa.Name = "pbKasa";
            this.pbKasa.Size = new System.Drawing.Size(230, 160);
            this.pbKasa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbKasa.TabIndex = 0;
            this.pbKasa.TabStop = false;
            // 
            // lblSistemAdi
            // 
            this.lblSistemAdi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSistemAdi.Location = new System.Drawing.Point(5, 5);
            this.lblSistemAdi.Name = "lblSistemAdi";
            this.lblSistemAdi.Size = new System.Drawing.Size(240, 30);
            this.lblSistemAdi.TabIndex = 1;
            this.lblSistemAdi.Text = "Sistem Adı";
            this.lblSistemAdi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDonanimlar
            // 
            this.lblDonanimlar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDonanimlar.Location = new System.Drawing.Point(10, 210);
            this.lblDonanimlar.Name = "lblDonanimlar";
            this.lblDonanimlar.Size = new System.Drawing.Size(230, 110);
            this.lblDonanimlar.TabIndex = 2;
            this.lblDonanimlar.Text = "Donanım Özellikleri";
            // 
            // lblFiyat
            // 
            this.lblFiyat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblFiyat.Location = new System.Drawing.Point(10, 325);
            this.lblFiyat.Name = "lblFiyat";
            this.lblFiyat.Size = new System.Drawing.Size(230, 30);
            this.lblFiyat.TabIndex = 3;
            this.lblFiyat.Text = "0 TL";
            this.lblFiyat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSepeteEkle
            // 
            this.btnSepeteEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSepeteEkle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSepeteEkle.Location = new System.Drawing.Point(50, 365);
            this.btnSepeteEkle.Name = "btnSepeteEkle";
            this.btnSepeteEkle.Size = new System.Drawing.Size(150, 35);
            this.btnSepeteEkle.TabIndex = 4;
            this.btnSepeteEkle.Text = "Sepete Ekle";
            this.btnSepeteEkle.UseVisualStyleBackColor = true;
            this.btnSepeteEkle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSepeteEkle.Click += new System.EventHandler(this.btnSepeteEkle_Click);
            // 
            // HazirSistemKarti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSepeteEkle);
            this.Controls.Add(this.lblFiyat);
            this.Controls.Add(this.lblDonanimlar);
            this.Controls.Add(this.lblSistemAdi);
            this.Controls.Add(this.pbKasa);
            this.Name = "HazirSistemKarti";
            this.Size = new System.Drawing.Size(250, 410);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HazirSistemKarti_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pbKasa)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbKasa;
        private System.Windows.Forms.Label lblSistemAdi;
        private System.Windows.Forms.Label lblDonanimlar;
        private System.Windows.Forms.Label lblFiyat;
        private System.Windows.Forms.Button btnSepeteEkle;
    }
}
