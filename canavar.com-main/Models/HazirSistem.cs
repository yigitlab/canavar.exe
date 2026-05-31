using System;
using System.Collections.Generic;

namespace ProductWizardApp.Models
{
    public class HazirSistem
    {
        public string SistemAdi { get; set; } = string.Empty;
        public List<Parca> Parcalar { get; set; } = new List<Parca>();
        public int ToplamFiyat { get; set; }
        public string ResimYolu { get; set; } = string.Empty;
    }
}
