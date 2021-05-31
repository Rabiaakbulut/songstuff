using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Model;

namespace WebApplication1.Pages
{
    public class Uyelik1Model : PageModel
    {
        [BindProperty]
        public UserModel Kullanici { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("/Index",new { il = Kullanici.Adres.Il,name=Kullanici.Ad,ülke=Kullanici.Adres.Ulke });

        }
    }
}
