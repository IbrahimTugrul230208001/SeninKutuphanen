using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : Controller
{
    UserManager _userManager = new UserManager(new EfUserRepository());
    private readonly IUserService _userService;
   
    public RegisterController(IUserService userService)
    {
        _userService = userService;
    }
    public IActionResult RegisterPage()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RegisterUser(IFormCollection receivedUserInput)
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
}
