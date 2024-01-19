using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace webapp.Pages
{

    [IgnoreAntiforgeryToken]
    public class SESSIONModel : PageModel
    {
        [BindProperty]
        [Display(Name = "說點什麼：")]
        public string Message { get; set; }

        public void OnGet()
        {
        }
        
        public void OnPost()
        {
            HttpContext.Session.SetString("Message", Message);
        }
    }
}
