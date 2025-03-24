using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Services;

namespace learningASP.NET_CORE.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _userName;
        private readonly IAIServices _aiService;
        LibraryManager _libraryManager = new(new EfLibraryRepository());

        public ChatBotController(IUserService userService,IAIServices AIService)
        {
            _userService = userService;
            _userName = _userService.UserName;
            _aiService = AIService;
        }

    
        public IActionResult ChatBot()
        {
            ViewData["UserName"] = _userName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Chat([FromBody]ChatRequest chatRequest, CancellationToken cancellationToken)
        {
            if (chatRequest == null || string.IsNullOrEmpty(chatRequest.Prompt))
            {
                return BadRequest("Invalid request data.");
            }
           
            // Use the AIService to get a message stream
            await _aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken);           

            // You can choose how to return the response - here, returning as JSON
            return Ok();
        }
    }
}
