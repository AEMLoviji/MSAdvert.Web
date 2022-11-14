using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdvertWeb.Models.Accounts;
using AdvertWeb.Models.AdvertManagement;

namespace AdvertWeb.Controllers;

public class AccountsController : Controller
{
    private readonly CognitoSignInManager<CognitoUser> _signInManager;
    private readonly CognitoUserManager<CognitoUser> _userManager;
    private readonly CognitoUserPool _cognitoUserPool;

    public AccountsController(SignInManager<CognitoUser> signInManager,
        UserManager<CognitoUser> userManager,
        CognitoUserPool cognitoUserPool)
    {
        _userManager = userManager as CognitoUserManager<CognitoUser>;
        _signInManager = signInManager as CognitoSignInManager<CognitoUser>;
        _cognitoUserPool = cognitoUserPool;
    }

    public IActionResult Signup() => View();

    [HttpPost]
    public async Task<IActionResult> Signup(SignupModel input)
    {
        if (ModelState.IsValid)
        {
            var user = _cognitoUserPool.GetUser(input.Email);
            if (user.Status != null)
            {
                ModelState.AddModelError("UserExists", "User with this email already exists");
                return View(input);
            }

            user.Attributes.Add(CognitoAttribute.Name.AttributeName, input.Email);

            var createdUserResult = await _userManager.CreateAsync(user, input.Password);
            if (createdUserResult.Succeeded)
            {
                return RedirectToAction(nameof(Confirm));
            }

            foreach (var error in createdUserResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        return View(input);
    }

    public IActionResult Confirm() => View();

    [HttpPost]
    public async Task<IActionResult> Confirm(ConfirmModel input)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is null)
            {
                ModelState.AddModelError("NotFound", "A user with the given email address was not found");
                return View(input);
            }

            var confirmSignupResult = await _userManager.ConfirmSignUpAsync(user, input.Code, true);
            if (confirmSignupResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in confirmSignupResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        return View(input);
    }

    [HttpGet]
    public IActionResult Signin() => View();

    [HttpPost]
    public async Task<IActionResult> Signin(SigninModel input)
    {
        if (ModelState.IsValid)
        {
            var singInResult = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, false);
            if (singInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("LoginError", "Email and Password don't match");
        }

        return View(input);
    }
}
