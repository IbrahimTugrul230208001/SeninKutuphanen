using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

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
            string email = user.Email;
            string userName = user.UserName;
            string password = user.NewPassword;
            string confirmPassword = user.NewPasswordAgain;

            if (password != confirmPassword)
            {
                return Json(new { success = false, message = "Şifreler eşleşmiyor." });
            }

            if (await _userManager.ValidateEmailAsync(email) == false)
            {
                return Json(new { success = false, message = "Email zaten kayıtlı." });
            }

            if (await _userManager.ValidateUserNameAsync(userName) == false)
            {
                return Json(new { success = false, message = "Kullanıcı adı mevcut Başka bir isim deneyiniz." });
            }

            string verificationCode = new Random().Next(100000, 999999).ToString();
            _userService.VerificationCode = verificationCode;
            await SendEmailAsync(email, verificationCode);
            return Json(new { success = true, redirectUrl = Url.Action("Verification", "Verification") });
        }
        catch (ApplicationException ex)
        {
            Console.WriteLine($"Application Error: {ex.Message}");
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            return Json(new { success = false, message = "An unexpected error occurred while registering the user." });
        }
    }

    private async Task SendEmailAsync(string email, string code)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Senin Kütüphanen", "seninkutuphanen@outlook.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Doğrulama Kodu";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Senin Kütüphanen</h2>
                    <h3>Doğrulama Kodunuz</h3>
                    <p>Merhaba,</p>
                    <p>Giriş yapabilmek için aşağıdaki doğrulama kodunu kullanın:</p>
                    <h3 style='background-color: #f4f4f4; padding: 10px; border-radius: 5px; text-align: center;'>
                        {code}
                    </h3>
                    <p>Bu kodun süresi 10 dakikadır.</p>
                    <p>Teşekkürler, <br> Your Company</p>
                </body>
                </html>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            string smtpHost = "smtp.outlook.com";
            int smtpPort = 587;
            string emailAddress = "okurkoprusu@outlook.com";
            string emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            using var client = new SmtpClient();
            client.Connect(smtpHost, smtpPort, false); // false for non-SSL if using STARTTLS
            client.Authenticate(emailAddress, emailPassword);
            await client.SendAsync(message); // Send email asynchronously
            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as necessary
            Console.WriteLine($"An error occurred while sending email: {ex.Message}");
        }

    }
}


