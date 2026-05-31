using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ProductWizardApp.Models;

namespace ProductWizardApp.Data
{
    public class DataService
    {
        private AppDbContext _context;

        public static string GetAppDataDir()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dir = Path.Combine(appData, "canavar.com");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public static string GetImagesDir()
        {
            // 1. Önce AppData dizinine bak (standalone mod için ana dizin)
            string appDataDir = GetAppDataDir();
            string t = Path.Combine(appDataDir, "Images", "Products");
            if (Directory.Exists(t)) return t;

            // 2. Çalışma dizinine (root) bak
            string cur = Directory.GetCurrentDirectory();
            t = Path.Combine(cur, "Images", "Products");
            if (Directory.Exists(t)) return t;

            // 3. Exe yanına bak
            string b = AppDomain.CurrentDomain.BaseDirectory;
            t = Path.Combine(b, "Images", "Products");
            if (Directory.Exists(t)) return t;

            // 4. Klasör yapısına göre yukarı çık (fallback)
            t = Path.GetFullPath(Path.Combine(b, "..", "..", "..", "Images", "Products"));
            return Directory.Exists(t) ? t : string.Empty;
        }

        public static void LoadProductImage(PictureBox pb, Product p)
        {
            pb.Image?.Dispose(); pb.Image = null;
            string dir = GetImagesDir();
            if (string.IsNullOrEmpty(dir)) { SetPlaceholder(pb); return; }

            if (!string.IsNullOrEmpty(p.Image))
            {
                string fullPath = Path.Combine(dir, p.Image);
                if (!File.Exists(fullPath))
                {
                    try
                    {
                        var files = Directory.GetFiles(dir);
                        fullPath = files.FirstOrDefault(f => string.Equals(Path.GetFileName(f), p.Image, StringComparison.OrdinalIgnoreCase)) ?? fullPath;
                    }
                    catch { }
                }

                if (File.Exists(fullPath))
                {
                    try
                    {
                        using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            using (var img = Image.FromStream(fs))
                            {
                                pb.Image = new Bitmap(img);
                            }
                        }
                        return;
                    }
                    catch { }
                }
            }

            // Fallback: Name based match
            string s1 = $"{p.Brand} {p.Name}".ToLowerInvariant().Trim();
            string s2 = p.Name.ToLowerInvariant().Trim();
            try
            {
                foreach (var f in Directory.GetFiles(dir))
                {
                    string ext = Path.GetExtension(f).ToLowerInvariant();
                    if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".webp") continue;
                    string fn = Path.GetFileNameWithoutExtension(f).ToLowerInvariant().Trim();
                    if (fn == s1 || fn == s2)
                    {
                        using (var fs = new FileStream(f, FileMode.Open, FileAccess.Read))
                        {
                            using (var img = Image.FromStream(fs))
                            {
                                pb.Image = new Bitmap(img);
                            }
                        }
                        return;
                    }
                }
            }
            catch { }
            SetPlaceholder(pb);
        }

        private static void SetPlaceholder(PictureBox pb)
        {
            int w = Math.Max(pb.Width, 90), h = Math.Max(pb.Height, 90);
            var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), 0, 0, w, h);
            g.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 1, 1, w - 2, h - 2);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("Görsel\nYok", new Font("Segoe UI", 8, FontStyle.Italic), Brushes.Gray, new RectangleF(0, 0, w, h), sf);
            pb.Image = bmp;
        }

        public DataService()
        {
            _context = new AppDbContext();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Veritabanının ve tabloların var olduğundan emin ol
            _context.Database.EnsureCreated();
        }

        public List<T> GetProductsByType<T>() where T : Product
        {
            // AsNoTracking ile okumak sadece listeleme için performansı artırır
            return _context.Products.OfType<T>().AsNoTracking().ToList();
        }
    }
}

