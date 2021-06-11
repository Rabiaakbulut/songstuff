using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public JsonMusicService JsonMusicService;
        public IEnumerable<MusicModel> Musics;

        public IndexModel(ILogger<IndexModel> logger, JsonMusicService jsonmusicservice)
        {
            _logger = logger;
            JsonMusicService = jsonmusicservice;
        }

        [BindProperty(SupportsGet =true)]
        public string il { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ülke { get; set; }
        [BindProperty(SupportsGet = true)]
        public string name { get; set; }

        [BindProperty]
        public string Extract { get; set; } //sıcaklık
        [BindProperty]
        public string Extract2 { get; set; }//karşı grup üyeleri
        [BindProperty(SupportsGet = true)]
        public string Status { get; set; }//Müzik ekleme, güncellem, silme için durum bilgisi
        public string videoLink { get; set; } = string.Empty; //sorgu sonucu döndürülen iframe src'si                                                              
        public int JsonKayıtSayısı { get; set; } //json dosyasında kaç adet müzik kaydı var

        //radiobuttondan gelen genre, feeling, enery, duration değerleri
        [BindProperty]  
        public string genre { get; set; } 
        [BindProperty]
        public string feeling { get; set; }
        [BindProperty]
        public string energy { get; set; }
        [BindProperty]
        public string duration { get; set; }
        //genre, feeling, enery, duration değerleri girilmemesi durumunda uyarı
        [BindProperty]
        public string genreUyarı { get; set; }
        [BindProperty]
        public string feelingUyarı { get; set; }
        [BindProperty]
        public string energyUyarı { get; set; }
        [BindProperty]
        public string durationUyarı { get; set; }


        public void OnGet()
        {
            videoLink = "https://www.youtube.com/embed/VJDJs9dumZI"; //site açıldığında çıkacak ilk src

            if (string.IsNullOrWhiteSpace(il))   //Sıcaklık bilgisi için default konum
                il = "istanbul";
            if (string.IsNullOrWhiteSpace(ülke))
                ülke = "Turkey";          

            if (string.IsNullOrWhiteSpace(name)) //Merhaba misafir
                name = "misafir";
            

            try
            {
                Extract = ExtractData(); //hava sıcaklığı
            }
            catch
            {
                Extract = "";
            }
            try
            {
                Extract2 = ExtractData2(); //karşı takım üyeleri
            }
            catch
            {
                Extract2 = "";
            }

        }
        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(il))   //Sıcaklık bilgisi için default konum
                il = "istanbul";
            if (string.IsNullOrWhiteSpace(ülke))
                ülke = "Turkey";

            if (string.IsNullOrWhiteSpace(name)) //Merhaba misafir
                name = "misafir";

            try
            {
                Extract = ExtractData(); //hava sıcaklığı
            }
            catch
            {
                Extract = "";
            }
            try
            {
                Extract2 = ExtractData2(); //karşı takım üyeleri
            }
            catch
            {
                Extract2 = "";
            }

           var projects = JsonMusicService.GetProjects(); //json müzik dosyasında kaç adet kayıt var
           JsonKayıtSayısı = projects.Max(x => x.id);

            WebClient client = new WebClient(); //json müzik kayıtlarına erişim
            string strPgeCode = client.DownloadString("https://songstuff.azurewebsites.net/api/Music");
            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPgeCode);

            for (var i = 0; i < JsonKayıtSayısı; i++) //json dosyasında radibutton değerlerine göre sorgu
            {
                if (dobj[i]["genre"].ToString() == genre &&
                    dobj[i]["feeling"].ToString() == feeling  &&
                    dobj[i]["energy"].ToString() == energy  &&
                    dobj[i]["duration"].ToString() == duration )
                {
                    videoLink = dobj[i]["url"].ToString();
                }
            }

            if (genre == null)
                genreUyarı = "Please select genre!";  //tüm özelliklerin seçilmesi için uyarı mesajları
            if (feeling == null)
                feelingUyarı = "Please select feeling!";
            if (energy == null)
                energyUyarı = "Please select energy!";
            if (duration == null)
                durationUyarı = "Please select duration!";
        }
        public string ExtractData() //Apiden sıcaklık bilgisi çekme
        {
            WebClient client = new WebClient();
            string strPgeCode = client.DownloadString($"https://api.openweathermap.org/data/2.5/weather?q={il},{ülke}&units=metric&appid=bd1f05d25562baa887604e2af83bfccf");
          
            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPgeCode);
            string temp = dobj["main"]["temp"].ToString();
            //string temp = dobj["weather"][0]["main"].ToString(); //hava durumunda bulutlu,güneşli...
            return temp;
        }
        public string ExtractData2()//Apiden karşı grup üyelerinin adlarını çekme
        {
            WebClient client = new WebClient();
            string strPgeCode = client.DownloadString("https://musicomm.azurewebsites.net/api/comments");
          
            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPgeCode);

            string temp0 = dobj[0]["sender"].ToString();
            string temp1 = dobj[1]["sender"].ToString();
            string temp2 = dobj[2]["sender"].ToString();
            string temp3 = temp0 +", "+ temp1 +", "+ temp2;
            return temp3;
        }
    }
}
