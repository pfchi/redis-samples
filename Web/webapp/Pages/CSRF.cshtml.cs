using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace webapp.Pages
{
    [AutoValidateAntiforgeryToken]
    public class CSRFModel : PageModel
    {
        [BindProperty]
        [Display(Name = "說點什麼：")]
        public string Message { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
        }
    }
}
