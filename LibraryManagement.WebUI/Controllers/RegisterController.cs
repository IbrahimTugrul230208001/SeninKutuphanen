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
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        try
        {
            string userName = user.UserName;
            string password = user.NewPassword;
            string confirmPassword = user.NewPasswordAgain;

            if (password == confirmPassword)
            {
                await _userManager.AddNewUserAsync(userName, password);
                return Json(new { success = true, redirectUrl = Url.Action("LogIn", "LogIn") });
            }
            else
            {
                return Json(new { success = false, message = "Passwords do not match" });
            }
        }
        catch (ApplicationException ex)
        {
            // Log the exception and return the error message
            Console.WriteLine($"Application Error: {ex.Message}");
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception and return a generic error message
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            return Json(new { success = false, message = "An unexpected error occurred while registering the user." });
        }
    }


}
