using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace webapp.Pages
{
    public class ChatRoomModel : PageModel
    {
        [BindProperty]
        [Display(Name = "暱稱：")]
        public string Nickname { get; set; }


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
