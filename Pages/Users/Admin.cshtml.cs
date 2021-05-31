using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Pages.Users
{
    public class AdminModel : PageModel
    {
        public JsonMusicService JsonProjectService;
        public AdminModel(JsonMusicService jsonmusicservice)
        {
            JsonProjectService = jsonmusicservice;
        }
        [BindProperty]
        public MusicModel Music { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostForm()
        {
            if (Music.id == 0)
            {
                JsonProjectService.AddProject(Music);
                return RedirectToPage("/Index", new { Status = "AddSuccess" });
            }
            else
            {
                JsonProjectService.UpdateProject(Music);
                return RedirectToPage("/Index", new { Status = "UpdateSuccess" });
            }
        }
        public void OnPostGetProjectById()
        {
            Music = JsonProjectService.GetProjectById(Music.id);
        }
        public IActionResult OnPostDeleteByID()
        {
            if (Music.id != 0)
            {
                JsonProjectService.DeleteProject(Music.id);
                return RedirectToPage("/Index", new { Status = "DeleteSuccess" });
            }
            else
                return RedirectToPage("/Index", new { Status = "DeleteError" });

        }

    }
}

