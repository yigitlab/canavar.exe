namespace ProductWizardApp;

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ProductWizardApp.Data;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        EnsureExtracted();
        ApplicationConfiguration.Initialize();
        var dataService = new DataService();
        Application.Run(new Form1(dataService));
    }

    private static void EnsureExtracted()
    {
        string targetDir = DataService.GetAppDataDir();
        string dbPath = Path.Combine(targetDir, "products.db");

        // Eğer veritabanı veya resimler yoksa gömülü zip dosyasından çıkartalım
        if (!File.Exists(dbPath) || !Directory.Exists(Path.Combine(targetDir, "Images")))
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string? resourceName = assembly.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("payload.zip"));

                if (resourceName != null)
                {
                    using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                        {
                            string tempZipPath = Path.Combine(targetDir, "payload.tmp.zip");
                            using (FileStream fs = new FileStream(tempZipPath, FileMode.Create))
                            {
                                stream.CopyTo(fs);
                            }

                            // Zip dosyasını hedef klasöre ayıkla
                            ZipFile.ExtractToDirectory(tempZipPath, targetDir, overwriteFiles: true);

                            // Geçici dosyayı sil
                            File.Delete(tempZipPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama kaynakları yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}