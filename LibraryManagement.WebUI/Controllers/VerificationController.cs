using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace learningASP.NET_CORE.Controllers
{
    public class VerificationController : Controller
    {
        UserManager _userManager = new UserManager(new EfUserRepository());
        private readonly IUserService _userService;

        public VerificationController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Verification()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail()
        {
            string email = _userService.Email;
            string userName = _userService.UserName;
            string password = _userService.Password;
            string verificationCode = _userService.VerificationCode;
            await _userManager.AddNewUserAsync(email, userName, password);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendVerificationCode()
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Senin Kütüphanen", "seninkutuphanen@outlook.com"));
                message.To.Add(new MailboxAddress("", _userService.Email));
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
                        {VerificationCode()}
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
            return View("Verification");
        }
        public async Task<string> VerificationCode()
        {
            string verificationCode = new Random().Next(100000, 999999).ToString();
            return verificationCode;
        }
    }
}
