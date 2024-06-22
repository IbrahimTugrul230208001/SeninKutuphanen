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
    public async Task<IActionResult> RegisterUser([FromBody]User user)
    {
        string userName = user.UserName;
        string password = user.NewPassword;
        string confirmPassword = user.NewPasswordAgain;

        if (password == confirmPassword)
        {
            await _userManager.AddNewUserAsync(userName, password);
            return RedirectToAction("LogIn", "LogIn");
        }
        else
        {
            return View("Register");
        }
    }
}
