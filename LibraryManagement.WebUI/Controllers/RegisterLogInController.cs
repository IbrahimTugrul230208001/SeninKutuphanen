using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public class RegisterLogInController : Controller
{
    UserManager _userManager = new UserManager(new EfUserRepository());
    private readonly IUserService _userService;
   
    public RegisterLogInController(IUserService userService)
    {
        _userService = userService;
    }
    public IActionResult RegisterPage()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RegisterPage(IFormCollection receivedUserInput)
    {
        string userName = Convert.ToString(receivedUserInput["TbxUserName"]);
        string password = Convert.ToString(receivedUserInput["TbxPassword"]);
        string confirmPassword = Convert.ToString(receivedUserInput["TbxPasswordAgain"]);

        if (password == confirmPassword)
        {
            _userManager.AddNewUser(userName, password);
            return RedirectToAction("LogInPage", "RegisterLogIn");
        }
        else
        {
            return View();
        }
    }

    public IActionResult LogInPage()
    {
            return View();
    }

    [HttpPost]
    public IActionResult LogIn([FromBody]User user)
    {
        if (_userManager.ValidateUser(user.UserName, user.Password))
        {
            _userService.UserName = user.UserName;
            _userService.ProfilePicture = _userManager.ProfilePictureImage(user.UserName);
            return Json(new { success = true, redirectUrl = Url.Action("LogInPage") });
        }
        else
        {
            return Json(new { success = false, redirectUrl = Url.Action("LogInPage")});
        }
    }
}
