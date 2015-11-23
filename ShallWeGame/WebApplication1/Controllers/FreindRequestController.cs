using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{

    [RoutePrefix("api/freinds")]
    public class FreindRequestController : ApiController
    {
        private GameContext _ctx;
        private UserManager<ApplicationUser> _userManager;

        public FreindRequestController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _ctx = new GameContext();
        }


        [Route("request")]
        public IHttpActionResult CreateRequest(FreindRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            FreindRequest request = new FreindRequest();
            request.sender = GetAccount();
            request.reciver =(Account) _ctx.Accounts.FirstOrDefault(a => a.id == model.reciverId);
            request.requestStatus = RequestStatus.Standby;
            return Ok(request);
        }

        [Authorize]
        [Route("myrequests")]
        public IHttpActionResult MyRequests()
        {
            Account user = GetAccount();
            IQueryable<FreindRequest> qurey =
                from fr in _ctx.FreindRequests
                where (fr.reciver.id == user.id ||
                fr.sender.id == user.id && 
                fr.requestStatus != RequestStatus.Rejected || 
                fr.requestStatus != RequestStatus.Accepted)
                select fr;

            return Ok(qurey.ToList());
        }

        [Authorize]
        [Route("accept")]
        public IHttpActionResult AcceptRequest(IdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IQueryable<FreindRequest> query =
                from fr in _ctx.FreindRequests
                where fr.id == model.id &&
                      fr.reciver.id == GetAccount().id
                select fr;


            List<FreindRequest> requests = query.ToList();

            if (requests.Count == 0)
            {
                return BadRequest("");
            }

            FreindRequest request = requests.First();
            request.requestStatus = RequestStatus.Accepted;
            _ctx.FreindRequests.AddOrUpdate(request);
            Account user = GetAccount();
            user.freindsList.Add(request.sender);
            Account sender = request.sender;
            sender.freindsList.Add(user);
            _ctx.Accounts.AddOrUpdate(user);
            _ctx.Accounts.AddOrUpdate(sender);
            _ctx.SaveChangesAsync();

            return Ok();
        }


        [Authorize]
        [Route("reject")]
        public IHttpActionResult RejectRequest(IdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IQueryable<FreindRequest> query =
                from fr in _ctx.FreindRequests
                where fr.id == model.id &&
                      fr.reciver.id == GetAccount().id
                select fr;


            List<FreindRequest> requests = query.ToList();

            if (requests.Count == 0)
            {
                return BadRequest("");
            }
            FreindRequest request = requests.First();
            request.requestStatus = RequestStatus.Rejected;
            _ctx.FreindRequests.AddOrUpdate(request);
            _ctx.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        private Account GetAccount()
        {
            var user = User.Identity.GetUserName();
            ApplicationUser real = (ApplicationUser)_userManager.FindByNameAsync(user).Result;
            return _ctx.Accounts.FirstOrDefault(a => a.userId == real.Id);
        }

    }
}
