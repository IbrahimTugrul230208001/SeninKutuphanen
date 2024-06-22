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
    public async Task<IActionResult> RegisterUser(IFormCollection userData)
    {
        string userName = userData["TbxUserName"].ToString();
        string password = userData["TbxPassword"].ToString();
        string confirmPassword = userData["TbxPasswordAgain"].ToString();

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
