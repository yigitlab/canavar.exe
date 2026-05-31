using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductWizardApp.Data
{
    public class NewsArticle
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("urlToImage")]
        public string UrlToImage { get; set; } = string.Empty;
    }

    public class NewsApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("articles")]
        public List<NewsArticle> Articles { get; set; } = new();
    }

    public class NewsService
    {
        // Lütfen buraya kendi FreeNewsAPI anahtarınızı girin:
        private static readonly string API_KEY = "YOUR_FREENEWSAPI_KEY_HERE";
        private const string API_URL = "https://free-news.p.rapidapi.com/v1/search?q=teknoloji&lang=tr";

        private static readonly HttpClient _httpClient;

        static NewsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ProductWizardApp/1.0");
        }

        public static async Task<List<NewsArticle>> GetTechnologyNewsAsync()
        {
            List<NewsArticle> articles;
            if (API_KEY == "YOUR_FREENEWSAPI_KEY_HERE")
            {
                articles = GetMockNews();
            }
            else
            {
                try
                {
                    var response = await _httpClient.GetStringAsync(API_URL);
                    var result = JsonSerializer.Deserialize<NewsApiResponse>(response);
                    articles = result?.Articles ?? GetMockNews();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Haberler alınırken hata oluştu: " + ex.Message);
                    articles = GetMockNews();
                }
            }

            // Ensure images
            foreach (var item in articles)
            {
                if (string.IsNullOrEmpty(item.UrlToImage))
                {
                    item.UrlToImage = "https://images.unsplash.com/photo-1519389950473-47ba0277781c?auto=format&fit=crop&q=80&w=800"; // Tech placeholder
                }
            }
            return articles;
        }

        private static List<NewsArticle> GetMockNews()
        {
            return new List<NewsArticle>
            {
                new NewsArticle {
                    Title = "NVIDIA RTX 5090 Sızdırıldı: 32GB VRAM ve İnanılmaz Performans Artışı Bekleniyor",
                    Url = "https://www.donanimhaber.com",
                    UrlToImage = "https://images.unsplash.com/photo-1591488320449-011701bb6704?auto=format&fit=crop&q=80&w=800"
                },
                new NewsArticle {
                    Title = "Intel 15. Nesil Arrow Lake İşlemciler Oyun Testlerinde Rekor Kırdı",
                    Url = "https://www.shiftdelete.net",
                    UrlToImage = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&q=80&w=800"
                },
                new NewsArticle {
                    Title = "Samsung Galaxy S26 Ultra İlk Görüntüleri Sızdı: Tamamen Çerçevesiz Ekran!",
                    Url = "https://www.webtekno.com",
                    UrlToImage = "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?auto=format&fit=crop&q=80&w=800"
                },
                new NewsArticle {
                    Title = "AMD Ryzen 9950X3D ile 3D V-Cache Teknolojisi Yeni Zirveye Ulaşıyor",
                    Url = "https://www.technopat.net",
                    UrlToImage = "https://images.unsplash.com/photo-1620283085068-5a2104a37fbd?auto=format&fit=crop&q=80&w=800"
                },
                new NewsArticle {
                    Title = "Windows 12 Yapay Zeka Özellikleri ile Geliyor: İşletim Sistemi Baştan Yazılıyor",
                    Url = "https://www.chip.com.tr",
                    UrlToImage = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?auto=format&fit=crop&q=80&w=800"
                },
                new NewsArticle {
                    Title = "Apple M5 Çipli MacBook Pro Modelleri Çok Yakında: İnanılmaz Verimlilik",
                    Url = "https://www.log.com.tr",
                    UrlToImage = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?auto=format&fit=crop&q=80&w=800"
                },
                new NewsArticle {
                    Title = "Oyun Dünyasında Devrim: GTA 6 Çıkış Tarihi Netleşiyor!",
                    Url = "https://www.bolumsonucanavari.com",
                    UrlToImage = "https://images.unsplash.com/photo-1542751371-adc38448a05e?auto=format&fit=crop&q=80&w=800"
                }
            };
        }
    }
}
