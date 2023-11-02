using ESourcing.Core.Entities;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<AppUser> _userManager { get; }  //microsoft identity'den geliyor
        public SignInManager<AppUser> _signInManager { get; } //microsoft identity'den geliyor

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel,string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    //await _signInManager.SignOutAsync();  //cookie de bilgi varsa cıkıs yapılsın diye, signInManager giriş yapılmış kullanıcıların bilgilerinin tutuldugu yapı

                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, isPersistent: false, lockoutOnFailure: false);
                    // isPersistent kullanıcıyı hatırlaması için , lockoutOnFailure hatalı girişlerde kilitleme için.
                    if (result.Succeeded)
                    {
                        //return RedirectToAction("Index");
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email or password is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                }
            }
            return View(loginModel);
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(AppUserViewModel signUpModel)
        {
            if (ModelState.IsValid)
            {
                AppUser usr = new AppUser();
                usr.UserName = signUpModel.UserName;
                usr.Email = signUpModel.Email;
                usr.LastName = signUpModel.LastName;
                usr.PhoneNumber = signUpModel.PhoneNumber;
                usr.FirstName = signUpModel.FirstName;
                if (signUpModel.UserSelectTypeId == 1)
                {
                    usr.IsBuyer = true;
                    usr.IsSeller = false;
                }
                if (signUpModel.UserSelectTypeId == 2)
                {
                    usr.IsSeller = true;
                    usr.IsBuyer = false;

                }

                var result = await _userManager.CreateAsync(usr, signUpModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("Login");
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(signUpModel);
        }

        public IActionResult Logout()
        {
             _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
