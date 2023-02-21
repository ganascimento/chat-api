using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GADev.Chat.Application.Business;
using GADev.Chat.Application.DataVO.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GADev.Chat.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AppController : Controller
    {
        private readonly ITalkBusiness _talkBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IInvitationBusiness _invitationBusiness;

        public AppController(ITalkBusiness talkBusiness, IUserBusiness userBusiness, IInvitationBusiness invitationBusiness)
        {
            _talkBusiness = talkBusiness;
            _userBusiness = userBusiness;
            _invitationBusiness = invitationBusiness;
        }

        [HttpGet]
        [Route("init")]
        public async Task<IActionResult> GetInitData() {
            var _identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(_identity.FindFirst("userId").Value);
            
            var talk = await _talkBusiness.GetInitialData(userId);

            if (talk != null) return Ok(talk);

            return BadRequest();
        }

        [HttpGet]
        [Route("user/{name}")]
        public async Task<IActionResult> GetInvitesByUserName(string name) {
            var _identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(_identity.FindFirst("userId").Value);

            var users = await _invitationBusiness.GetInvitesByUserName(name, userId);

            if (users != null && users.Count > 0) return Ok(users);
            
            return BadRequest();
        }
    }
}