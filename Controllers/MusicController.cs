using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]  //   url ye ekle ->    /api/Music
    [ApiController]
    public class MusicController : ControllerBase
    {
        public JsonMusicService JsonMusicService;
        public MusicController(JsonMusicService jsonmusicservice)
        {
            JsonMusicService = jsonmusicservice;
        }
        [HttpGet]
        public IEnumerable<MusicModel> Get(int id)
        {
            if (id != 0)
            {
                List<MusicModel> list = new List<MusicModel> ();
                list.Add(JsonMusicService.GetProjectById(id));
                IEnumerable<MusicModel> en = list;
                return en;
            }
            else
            {
                return JsonMusicService.GetProjects();
            }
        }
    }
   
}
