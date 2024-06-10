using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public class RegisterLogInController : Controller
{
    private readonly IUserService _userProfileService;
    private readonly LibraryManager _libraryManager;

    public RegisterLogInController(IUserService userProfileService)
    {
        _userProfileService = userProfileService;
        _libraryManager = new LibraryManager(new EntityFrameworkCore());
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
            _libraryManager.AddNewUser(userName, password);
            return RedirectToAction("LogInPage", "RegisterLogIn");
        }
        else
        {
            return View();
        }
        return View();
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

        if (_libraryManager.ValidateUser(userName, password))
        {
            _userProfileService.UserName = userName;
            _userProfileService.ProfilePicture = _libraryManager.ProfilePictureImage(userName);
            return RedirectToAction("ProfileIndex", "UserProfile");
        }
        else
        {
            return View();
        }
    }
}
