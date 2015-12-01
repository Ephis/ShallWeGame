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


        //Get list of users friends by his id
        [Authorize]
        [Route("byUserid")]
        public async Task<IHttpActionResult> FriendsByUserId(IdViewModel model)
        {
            Account acc = GetAccount();

            IQueryable<Friends> friendsQuery =
                from f in _ctx.Friends
                where f.sender.id == model.id || f.reciver.id == model.id
                select f;

            List<Friends> friendList = friendsQuery.ToList();
            List<Account> accountList = new List<Account>();

            foreach (Friends f in friendList)
            {
                if (f.reciver.id != model.id)
                {
                    if (f.reciver.id == acc.id)
                    {
                        f.reciver.isUsersAccount = true;
                    }
                    accountList.Add(f.reciver);
                }
                else
                {
                    if (f.sender.id == acc.id)
                    {
                        f.sender.isUsersAccount = true;
                    }
                    accountList.Add(f.sender);
                }
            }
            


            return Ok(accountList);
        }


        [Authorize]
        [Route("getfriends")]
        public async Task<IHttpActionResult> GetAllFreinds()
        {
            Account acc = GetAccount();
            IQueryable<Friends> friendList = 
                from f in _ctx.Friends.Include("Sender").Include("Reciver")
                where f.reciver.id == acc.id || f.sender.id == acc.id
                select f;

            List<Friends> freindsWhereIamSenderList = friendList.ToList();

            List<Account> fList = new List<Account>();

            foreach (Friends friend in freindsWhereIamSenderList)
            {
                if (friend.reciver.id != acc.id)
                {
                    fList.Add(friend.reciver);
                }
                if (friend.sender.id != acc.id)
                {
                    fList.Add(friend.sender);
                }
            }


            return Ok(fList);
        }

        [Authorize]
        [Route("request")]
        public IHttpActionResult MakeRequest(IdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Account acc = GetAccount();

            if (acc.id == model.id)
            {
                return BadRequest("You cant add yourself");
            }

            FriendRequest request = new FriendRequest();
            request.sender = acc;
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
                from fr in _ctx.FreindRequests.Include("Sender")
                where (fr.reciver.id == user.id && 
                fr.requestStatus != RequestStatus.Rejected &&
                fr.requestStatus != RequestStatus.Accepted)
                select fr;
            List<FriendRequest> friendRequests = qurey.ToList();
            return Ok(friendRequests);
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
                from fr in _ctx.FreindRequests.Include("Sender").Include("Reciver")
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
            Friends freinds = new Friends(request.sender, user);
            _ctx.Friends.AddOrUpdate(freinds);
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
