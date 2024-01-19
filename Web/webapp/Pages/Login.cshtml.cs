using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace webapp.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Display(Name = "帳號")]
        public string UserName { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, UserName) },
                    CookieAuthenticationDefaults.AuthenticationScheme)));
            return RedirectToPage("Index");

        }
    }

}
