using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStar.Data.Helpers;
using PizzaStar.Models;
using PizzaStar.ViewModels;

namespace PizzaStar.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailSender _emailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, EmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult IsEmailInUse(string email)
        {
            if (_userManager.Users.Any(e => e.Email!.Equals(email)))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("register")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year, PhoneNumber = model.Phone };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    //Установки роли. Сама роль находится в таблице AspNetRoles
                    //если таблица пустая, получим ошибку. Обязательно заполняем роли!
                    result = await _userManager.AddToRoleAsync(user, "Client");
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Route("forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [Route("forgot-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(RequestForResetViewModel requestForResetViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(requestForResetViewModel);
            }
            var user = await _userManager.FindByEmailAsync(requestForResetViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Нет пользователя, с таким емейл адресом.");
                return View(requestForResetViewModel);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            if (_emailSender.SendResetPassword(user.Email, link, user.UserName))
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            else
            {
                ModelState.AddModelError("", "Произошла ошибка, попробуйте позже.");
            }
            return View(requestForResetViewModel);
        }
        [Route("forgot-password-confirmation")]
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [Route("reset-password")]
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (String.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(Login));
            }
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }
        [Route("reset-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {

                    if (error.Description.Equals("Invalid token."))
                    {
                        ModelState.AddModelError("", "Ваш токен либо отсутствует, либо устарел. Запросите восстановление снова.");
                    }
                    else
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(resetPassword);
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        [Route("reset-password-confirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
