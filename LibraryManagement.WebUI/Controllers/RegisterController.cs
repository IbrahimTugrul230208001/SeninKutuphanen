using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MailKit.Security;
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
           // await _userManager.AddNewUserAsync(email,userName,password);
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
    public async Task SendEmailAsync(string email, string code)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Senin Kütüphanen", "seninkutuphanen@outlook.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Verification Code";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verification Code</title>
    <style>
        body {{
            background-color: #121212;
            color: #ffffff;
            font-family: Arial, sans-serif;
            text-align: center;
            padding: 20px;
        }}
        .container {{
            max-width: 400px;
            margin: 0 auto;
            background-color: #1e1e1e;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(255, 255, 255, 0.1);
        }}
        h3 {{
            color: #bb86fc;
        }}
        p {{
            font-size: 16px;
        }}
        .code {{
            font-size: 24px;
            font-weight: bold;
            color: #03dac6;
            background-color: #333;
            padding: 10px;
            border-radius: 5px;
            display: inline-block;
            margin-top: 10px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h3>Your Verification Code</h3>
        <p>Use this code to verify your account:</p>
        <div class=""code"">{code}</div>
    </div>
</body>
</html>
"
            };

            message.Body = bodyBuilder.ToMessageBody();

            // Retrieve the API key securely (e.g., from environment variables)
            string key = "EwzkqAaPmTv9jnDS";

            using var client = new SmtpClient();

            // Connect to the SMTP server with STARTTLS
            await client.ConnectAsync("smtp-relay.brevo.com", 587, SecureSocketOptions.StartTls);
            Console.WriteLine("Connected to SMTP server.");

            await client.AuthenticateAsync("879650001@smtp-brevo.com", key);
            Console.WriteLine("Authenticated successfully.");

            // Send the email
            await client.SendAsync(message);
            Console.WriteLine("Email sent successfully.");

            // Disconnect from the server
            await client.DisconnectAsync(true);
            Console.WriteLine("Disconnected from SMTP server.");
        }
        catch (SmtpCommandException ex)
        {
            Console.WriteLine($"SMTP Command Error: {ex.Message} - Status Code: {ex.StatusCode}");
            // Log the full exception for debugging
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        catch (SmtpProtocolException ex)
        {
            Console.WriteLine($"SMTP Protocol Error: {ex.Message}");
            // Log the full exception for debugging
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
            // Log the full exception for debugging
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }

}


