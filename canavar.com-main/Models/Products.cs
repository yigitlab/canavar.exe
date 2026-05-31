using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ProductWizardApp.Models
{
    [JsonPolymorphic(
        TypeDiscriminatorPropertyName = "$type",
        UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    [JsonDerivedType(typeof(CPU), "CPU")]
    [JsonDerivedType(typeof(GPU), "GPU")]
    [JsonDerivedType(typeof(RAM), "RAM")]
    [JsonDerivedType(typeof(Motherboard), "Motherboard")]
    [JsonDerivedType(typeof(Storage), "Storage")]
    [JsonDerivedType(typeof(PC_Case), "PC_Case")]
    [JsonDerivedType(typeof(Phone), "Phone")]
    [JsonDerivedType(typeof(Tablet), "Tablet")]
    [JsonDerivedType(typeof(Laptop), "Laptop")]
    [JsonDerivedType(typeof(Television), "Television")]
    [JsonDerivedType(typeof(Monitor), "Monitor")]
    [JsonDerivedType(typeof(PSU), "PSU")]
    [JsonDerivedType(typeof(PrebuiltSystem), "PrebuiltSystem")]
    public abstract class Product
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Ürün Adı")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Marka")]
        public string Brand { get; set; } = string.Empty;

        [DisplayName("Fiyat")]
        public decimal Price { get; set; }   // decimal — JSON'da 3450.0 ve 11507.50 gibi değerleri destekler

        [DisplayName("Görsel")]
        public string Image { get; set; } = string.Empty;

        public override string ToString() => $"{Brand} {Name}";
    }

    public class CPU : Product
    {
        [DisplayName("Çekirdek Sayısı")]
        public int Cores { get; set; }

        [DisplayName("Temel Frekans (GHz)")]
        public decimal BaseClockGHz { get; set; }

        [DisplayName("Soket")]
        public string Socket { get; set; } = string.Empty;

        // Yeni özellikler
        [DisplayName("İşlemci L2 Cache")]
        public string IslemciL2Cache { get; set; } = string.Empty;

        [DisplayName("İşlemci Nesil")]
        public string IslemciNesil { get; set; } = string.Empty;

        [DisplayName("İşlemci Numarası")]
        public string IslemciNumarasi { get; set; } = string.Empty;

        [DisplayName("İşlemci Teknolojisi")]
        public string IslemciTeknolojisi { get; set; } = string.Empty;

        [DisplayName("Tüketim Değeri")]
        public int TuketimDegeri { get; set; }

        [DisplayName("Uyumlu Soketler")]
        public string UyumluSoketler { get; set; } = string.Empty;
    }

    public class GPU : Product
    {
        [DisplayName("GPU Bellek (GB)")]
        public int VRAM_GB { get; set; }

        [DisplayName("Çekirdek Hızı (MHz)")]
        public decimal CoreClockMHz { get; set; }

        // Yeni özellikler
        [DisplayName("DirectX")]
        public string DirectX { get; set; } = string.Empty;

        [DisplayName("Fan Adedi")]
        public int FanAdedi { get; set; }

        [DisplayName("GPU Bellek Arayüzü")]
        public string GPUBellekArayuzu { get; set; } = string.Empty;

        [DisplayName("GPU Bellek Hızı")]
        public string GPUBellekHizi { get; set; } = string.Empty;

        [DisplayName("GPU Seri")]
        public string GPUSeri { get; set; } = string.Empty;

        [DisplayName("HDMI Çıkışı")]
        public int HDMICikisi { get; set; }

        [DisplayName("Max Çözünürlük")]
        public string MaxCozunurluk { get; set; } = string.Empty;

        [DisplayName("OpenGL")]
        public string OpenGL { get; set; } = string.Empty;

        [DisplayName("Minimum Güç Kaynağı")]
        public int MinimumGucKaynagi { get; set; }

        [DisplayName("Ray Tracing")]
        public bool HasRayTracing { get; set; }

        [DisplayName("DLSS/FSR")]
        public bool HasDLSS_FSR { get; set; }
    }

    public class RAM : Product
    {
        [DisplayName("Kapasite (GB)")]
        public int KapasiteGB { get; set; }

        [DisplayName("Hız (MHz)")]
        public int HizMHz { get; set; }

        [DisplayName("RAM Tipi")]
        public string RamTipi { get; set; } = string.Empty;

        // Yeni özellikler
        [DisplayName("CAS Hızı")]
        public string CASHizi { get; set; } = string.Empty;

        [DisplayName("RAM Profil Desteği")]
        public string RamProfilDestegi { get; set; } = string.Empty;

        [DisplayName("Voltaj")]
        public string Voltaj { get; set; } = string.Empty;
    }

    public class Motherboard : Product
    {
        [DisplayName("Soket")]
        public string Soket { get; set; } = string.Empty;

        [DisplayName("Kasa Tipi")]
        public string KasaTipi { get; set; } = string.Empty;

        [DisplayName("RAM Tipi")]
        public string RamTipi { get; set; } = string.Empty;

        // Yeni özellikler
        [DisplayName("Display Port Çıkışı")]
        public int DisplayPortCikisi { get; set; }

        [DisplayName("HDMI Çıkışı")]
        public int HDMICikisi { get; set; }

        [DisplayName("İşlemci Uyumu")]
        public string IslemciUyumu { get; set; } = string.Empty;

        [DisplayName("Kullanım Tipi")]
        public string KullanimTipi { get; set; } = string.Empty;

        [DisplayName("Max Ram Desteği (GB)")]
        public int MaxRamDestegi { get; set; }

        [DisplayName("Ses Kanal Sayısı")]
        public string SesKanalSayisi { get; set; } = string.Empty;

        [DisplayName("M.2 Yuvası")]
        public int M2Yuvasi { get; set; }

        [DisplayName("Sata 6Gb/s")]
        public int Sata6Gbs { get; set; }

        [DisplayName("Ses Çipi")]
        public string SesCipi { get; set; } = string.Empty;

        [DisplayName("USB Port Sayısı")]
        public int UsbPortCount { get; set; }

        [DisplayName("Bluetooth")]
        public bool HasBluetooth { get; set; }

        [DisplayName("Wi-Fi")]
        public bool HasWiFi { get; set; }
    }

    public class Storage : Product
    {
        [DisplayName("Kapasite (GB)")]
        public int Capacity_GB { get; set; }

        [DisplayName("Arayüz")]
        public string Interface { get; set; } = string.Empty;

        [DisplayName("Okuma Hızı (MB/s)")]
        public int ReadSpeed_MBs { get; set; }

        [DisplayName("Yazma Hızı (MB/s)")]
        public int WriteSpeed_MBs { get; set; }

        [DisplayName("Disk Türü")]
        public string StorageType { get; set; } = string.Empty; // NVMe, SATA vb.
    }

    public class PC_Case : Product
    {
        [DisplayName("Kasa Tipi")]
        public string FormFactor { get; set; } = string.Empty;

        [DisplayName("Güç Kaynağı Dahil")]
        public bool IncludesPowerSupply { get; set; }

        [DisplayName("Fan Sayısı")]
        public int FanCount { get; set; }

        [DisplayName("Sıvı Soğutma Desteği")]
        public bool HasLiquidCoolingSupport { get; set; }

        [DisplayName("USB Portları")]
        public string UsbPorts { get; set; } = string.Empty;
    }

    public class PSU : Product
    {
        [DisplayName("Güç Kapasitesi (W)")]
        public int Wattage { get; set; }

        [DisplayName("Verimlilik Sertifikası")]
        public string Efficiency { get; set; } = string.Empty;

        [DisplayName("Modülerlik")]
        public string Modularity { get; set; } = string.Empty;

        [DisplayName("Anahtarlı")]
        public string IsSwitched { get; set; } = string.Empty;

        [DisplayName("Sessiz")]
        public string IsQuiet { get; set; } = string.Empty;

        [DisplayName("Fan Ölçüsü")]
        public string FanSize { get; set; } = string.Empty;

        [DisplayName("Max Çalışma Sıcaklığı")]
        public string MaxTemp { get; set; } = string.Empty;

        [DisplayName("Yükseklik")]
        public string Height { get; set; } = string.Empty;

        [DisplayName("Genişlik")]
        public string Width { get; set; } = string.Empty;

        [DisplayName("Derinlik")]
        public string Depth { get; set; } = string.Empty;

        [DisplayName("ATX 3.0")]
        public string ATX30 { get; set; } = string.Empty;

        [DisplayName("PCIe 5.0")]
        public string PCIe50 { get; set; } = string.Empty;

        [DisplayName("PFC")]
        public string PFC { get; set; } = string.Empty;

        [DisplayName("Akım Koruması")]
        public string CurrentProtection { get; set; } = string.Empty;

        [DisplayName("RGB")]
        public string RGB { get; set; } = string.Empty;

        [DisplayName("Oyuncu")]
        public string IsGaming { get; set; } = string.Empty;

        [DisplayName("Ağırlık")]
        public string Weight { get; set; } = string.Empty;

        [DisplayName("80 PLUS Sertifikası")]
        public string Certification80Plus { get; set; } = string.Empty;

        [DisplayName("ATX Versiyonu")]
        public string ATXVersion { get; set; } = string.Empty;

        [DisplayName("EPS Versiyonu")]
        public string EPSVersion { get; set; } = string.Empty;

        [DisplayName("Giriş Voltajı")]
        public string InputVoltage { get; set; } = string.Empty;

        [DisplayName("Form Faktörü")]
        public string FormFactor { get; set; } = string.Empty;

        [DisplayName("Soğutma Yöntemi")]
        public string CoolingMethod { get; set; } = string.Empty;
    }

    public class Phone : Product
    {
        // ── Mevcut temel alanlar ─────────────────────────────────────
        [DisplayName("Ekran Boyutu")]
        public double ScreenSize { get; set; }   // inç

        [DisplayName("Batarya (mAh)")]
        public int Battery_mAh { get; set; }

        [DisplayName("Depolama (GB)")]
        public int Storage_GB { get; set; }

        [DisplayName("Kamera (MP)")]
        public int Camera_MP { get; set; }

        [DisplayName("Yonga Seti")]
        public string Chipset { get; set; } = string.Empty;

        // ── Yeni detaylı alanlar ─────────────────────────────────────
        [DisplayName("Ekran/Gövde Oranı (%)")]
        public double ScreenBodyRatio { get; set; }  // % (ekran/gövde oranı)

        [DisplayName("Ağırlık (g)")]
        public int Weight_g { get; set; }  // gram

        [DisplayName("RAM (GB)")]
        public int RAM_GB { get; set; }  // RAM (GB)

        [DisplayName("Ekran Çözünürlüğü")]
        public string ScreenResolution { get; set; } = string.Empty; // ör: "2340 x 1080"

        [DisplayName("Video Çözünürlüğü")]
        public string VideoResolution { get; set; } = string.Empty; // ör: "4K@60fps"

        [DisplayName("SAR Değeri (Baş)")]
        public double SAR_Head_10g { get; set; }  // W/kg (10g kafa SAR değeri)

        [DisplayName("İşletim Sistemi")]
        public string OS_Version { get; set; } = string.Empty; // ör: "Android 14"

        [DisplayName("Görsel URL")]
        public string ImageUrl { get; set; } = string.Empty; // fotoğraf URL'si
    }

    public class Tablet : Product
    {
        [DisplayName("Ekran Boyutu")]
        public double ScreenSize { get; set; }

        [DisplayName("Batarya (mAh)")]
        public int Battery_mAh { get; set; }

        [DisplayName("Depolama (GB)")]
        public int Storage_GB { get; set; }

        [DisplayName("Kalem Desteği")]
        public bool HasStylus { get; set; }
    }

    public class Laptop : Product
    {
        [DisplayName("Ekran Boyutu")]
        public double ScreenSize { get; set; }

        [DisplayName("RAM (GB)")]
        public int RAM_GB { get; set; }

        [DisplayName("Depolama (GB)")]
        public int Storage_GB { get; set; }

        [DisplayName("İşlemci")]
        public string Processor { get; set; } = string.Empty;

        [DisplayName("Ekran Kartı")]
        public string GPU { get; set; } = string.Empty;
    }

    public class Television : Product
    {
        [DisplayName("Ekran Boyutu")]
        public double ScreenSize { get; set; }

        [DisplayName("Çözünürlük")]
        public string Resolution { get; set; } = string.Empty;

        [DisplayName("Panel Tipi")]
        public string PanelType { get; set; } = string.Empty;

        [DisplayName("Yenileme Hızı (Hz)")]
        public int RefreshRate_Hz { get; set; }
    }

    public class Monitor : Product
    {
        [DisplayName("Ekran Boyutu")]
        public double ScreenSize { get; set; }

        [DisplayName("Çözünürlük")]
        public string Resolution { get; set; } = string.Empty;

        [DisplayName("Yenileme Hızı")]
        public string RefreshRate { get; set; } = string.Empty;

        [DisplayName("Panel Tipi")]
        public string PanelType { get; set; } = string.Empty;

        [DisplayName("Display Port")]
        public string HasDisplayPort { get; set; } = string.Empty;

        [DisplayName("HDMI")]
        public string HasHDMI { get; set; } = string.Empty;

        [DisplayName("Ekran Rengi")]
        public string ScreenColor { get; set; } = string.Empty;
    }

    public class PrebuiltSystem : Product
    {
        [DisplayName("İşlemci")]
        public string CPU { get; set; } = string.Empty;

        [DisplayName("Ekran Kartı")]
        public string GPU { get; set; } = string.Empty;

        [DisplayName("RAM")]
        public string RAM { get; set; } = string.Empty;

        [DisplayName("Depolama")]
        public string Storage { get; set; } = string.Empty;

        [DisplayName("Kasa")]
        public string Case { get; set; } = string.Empty;

        [DisplayName("Güç Kaynağı")]
        public string PSU { get; set; } = string.Empty;
    }
}

