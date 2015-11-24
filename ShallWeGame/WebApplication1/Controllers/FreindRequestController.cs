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

    [RoutePrefix("api/friends")]
    public class FreindRequestController : ApiController
    {
        private GameContext _ctx;
        private UserManager<ApplicationUser> _userManager;

        public FreindRequestController()
        {
            _ctx = new GameContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        [Authorize]
        [Route("getfriends")]
        public async Task<IHttpActionResult> GetAllFreinds()
        {
            Account acc = GetAccount();
            IQueryable<Freinds> freindList = 
                from f in _ctx.Freinds
                where f.reciver.id == acc.id || f.sender.id == acc.id
                select f;
            IQueryable<Freinds> freindsWhereIamSender =
                from fr in freindList
                where fr.reciver.id != acc.id
                select fr;

            List<Freinds> freindsWhereIamSenderList = freindsWhereIamSender.ToList();

            List<Account> fList = new List<Account>();

            foreach (Freinds freind in freindsWhereIamSenderList)
            {
                fList.Add(freind.reciver);
            }

            IQueryable<Freinds> freindsWhereIamReciver =
                from fr in freindList
                where fr.reciver.id != acc.id
                select fr;

            List<Freinds> freindsWhereIamReciverList = freindsWhereIamReciver.ToList();
            foreach (Freinds freind in freindsWhereIamReciverList)
            {
                fList.Add(freind.sender);
            }


            return Ok(fList);
        }

        [Authorize]
        [Route("request")]
        public IHttpActionResult CreateRequest(IdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            FriendRequest request = new FriendRequest();
            request.sender = GetAccount();
            request.reciver =(Account) _ctx.Accounts.FirstOrDefault(a => a.id == model.id);
            request.requestStatus = RequestStatus.Standby;
            _ctx.FreindRequests.AddOrUpdate(request);
            _ctx.SaveChanges();

            return Ok(request);
        }

        [Authorize]
        [Route("myrequests")]
        public IHttpActionResult MyRequests()
        {
            Account user = GetAccount();
            IQueryable<FriendRequest> qurey =
                from fr in _ctx.FreindRequests
                where (fr.reciver.id == user.id && 
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
            Account user = GetAccount();

            IQueryable<FriendRequest> query =
                from fr in _ctx.FreindRequests
                where fr.id == model.id &&
                      fr.reciver.id == user.id
                select fr;

            List<FriendRequest> requests = query.ToList();

            if (requests.Count == 0)
            {
                return BadRequest("");
            }

            FriendRequest request = requests.First();
            request.requestStatus = RequestStatus.Accepted;
            _ctx.FreindRequests.AddOrUpdate(request);
            Freinds freinds = new Freinds(request.sender, user);
            _ctx.Freinds.AddOrUpdate(freinds);
            _ctx.SaveChanges();

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
            Account user = GetAccount();
            IQueryable<FriendRequest> query =
                from fr in _ctx.FreindRequests
                where fr.id == model.id &&
                      fr.reciver.id == user.id
                select fr;


            List<FriendRequest> requests = query.ToList();

            if (requests.Count == 0)
            {
                return BadRequest("");
            }
            FriendRequest request = requests.First();
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
