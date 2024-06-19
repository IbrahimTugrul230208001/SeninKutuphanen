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
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RegisterUser(IFormCollection userData)
    {
        string userName = userData["TbxUserName"].ToString();
        string password = userData["TbxPassword"].ToString();
        string confirmPassword = userData["TbxPasswordAgain"].ToString();

        if (password == confirmPassword)
        {
            _userManager.AddNewUser(userName, password);
            return RedirectToAction("LogIn", "LogIn");
        }
        else
        {
            return View("Register");
        }
    }
}
