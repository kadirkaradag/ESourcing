using ESourcing.Core.Entities;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }

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
        public IActionResult Login(LoginViewModel loginModel)
        {
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
                usr.PhoneNumber=signUpModel.PhoneNumber;
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


    }
}
