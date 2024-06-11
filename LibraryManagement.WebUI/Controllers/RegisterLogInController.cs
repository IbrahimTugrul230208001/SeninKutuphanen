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
    public IActionResult LogInPage(IFormCollection receivedUserInput)
    {
        string userName = Convert.ToString(receivedUserInput["TbxUserName"]);
        string password = Convert.ToString(receivedUserInput["TbxPassword"]);

        if (_userManager.ValidateUser(userName, password))
        {
            _userService.UserName = userName;
            _userService.ProfilePicture = _userManager.ProfilePictureImage(userName);
            return RedirectToAction("ProfileIndex", "UserProfile");
        }
        else
        {
            return View();
        }
    }
}
