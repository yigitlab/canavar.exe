using System;

namespace ProductWizardApp.Models
{
    public class Parca
    {
        public string Ad { get; set; } = string.Empty;
        public int Fiyat { get; set; }
        public string Kategori { get; set; } = string.Empty;
        public string ResimYolu { get; set; } = string.Empty;
        public string Ozellik { get; set; } = string.Empty; // Örn: "AM4", "LGA1700", "DDR4" gibi

        public override string ToString()
        {
            return $"{Kategori}: {Ad} - {Fiyat:N0} TL";
        }
    }
}
