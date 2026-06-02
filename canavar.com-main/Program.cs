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

        // 1. Eğer AppData'da veritabanı yoksa ama yerel çalışma dizininde (veya exe yanında) products.db varsa,
        // hocanın projeyi Visual Studio'dan doğrudan çalıştırdığı anlar için bunu AppData'ya kopyalayalım.
        if (!File.Exists(dbPath))
        {
            string localDb = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.db");
            if (File.Exists(localDb))
            {
                try { File.Copy(localDb, dbPath, overwrite: true); } catch { }
            }
            else
            {
                string curDb = Path.Combine(Directory.GetCurrentDirectory(), "products.db");
                if (File.Exists(curDb))
                {
                    try { File.Copy(curDb, dbPath, overwrite: true); } catch { }
                }
            }
        }

        // 2. payload.zip mantığı (GitHub'dan doğrudan çalıştırma senaryosu için) iptal edilmiştir.
        // Resimler ve products.db zaten repository'de bulunacaktır.
    }
}