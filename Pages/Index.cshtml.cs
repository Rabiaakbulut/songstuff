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
        public string Extract { get; set; }
        [BindProperty]
        public string Extract2 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Status { get; set; }


        public void OnGet()
        {
            if (string.IsNullOrWhiteSpace(il))
            {
                il = "istanbul";
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "misafir";
            }
            if (string.IsNullOrWhiteSpace(ülke))
            {
                //ülke = "AU";
                ülke = "Turkey";
            }
            try
            {
                Extract = ExtractData();
            }
            catch
            {
                Extract = "";
            }
            Extract2 = ExtractData2();
            Musics = JsonMusicService.GetProjects();

        }
        public string ExtractData()
        {
            WebClient client = new WebClient();
            string strPgeCode = client.DownloadString($"https://api.openweathermap.org/data/2.5/weather?q={il},{ülke}&units=metric&appid=bd1f05d25562baa887604e2af83bfccf");
            //wiki
            //string url = string.Concat("https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles=","turkey");
            //string strPgeCode = client.DownloadString(url);

            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPgeCode);
            string temp = dobj["main"]["temp"].ToString();
            //string temp = dobj["query"]["pages"]["11125639"]["extract"].ToString();//wikipedia datası
            //string temp = dobj["weather"][0]["main"].ToString(); //hava durumunda bulutlu,güneşli...
            return temp;
        }
        public string ExtractData2()
        {
            WebClient client = new WebClient();
            string strPgeCode = client.DownloadString("https://musicomm.azurewebsites.net/api/comments");
            //wiki
            //string url = string.Concat("https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles=%22,%22turkey%22);
            //string strPgeCode = client.DownloadString(url);

            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPgeCode);

            string temp0 = dobj[0]["sender"].ToString();
            string temp1 = dobj[1]["sender"].ToString();
            string temp2 = dobj[2]["sender"].ToString();
            string temp3 = temp0 +", "+ temp1 +", "+ temp2;
            return temp3;
        }
    }
}
