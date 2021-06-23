using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class AdminModel : PageModel
    {
        [BindProperty]
        public string KullanýcýAdý { get; set; }
        [BindProperty]
        public string Þifre { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostForm()
        {
            if (KullanýcýAdý == "admin" && Þifre == "123")
            {
                return RedirectToPage("/Users/Admin185746qwa"); //Kullanýcý bilgileri doðruysa admin sayfasýna yönlen
            }
            else
            {
                return RedirectToPage("");//yanlýþsa ayný yerde kal
            }
        }
    }
}

