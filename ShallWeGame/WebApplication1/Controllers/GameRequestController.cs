using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/gamerequest")]
    public class GameRequestController : ApiController
    {

        private UserManager<ApplicationUser> _userManager;
        public GameContext _ctx;
        
        public GameRequestController()
        {
            _ctx = new GameContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        [Authorize]
        [Route("create")]
        public async Task<IHttpActionResult> CreateGameRequest(GameRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GameRequest gameRequest = new GameRequest();
            gameRequest.titel = model.titel;
            gameRequest.gameToPlay = _ctx.Games.FirstOrDefault(g => g.id == model.gameId);
            gameRequest.owner = GetAccount();
            gameRequest.playersNeeded = model.playersNeeded;

            _ctx.GameRequests.AddOrUpdate(gameRequest);
            if (model.invites != null)
            {
                List<Invite> invites = GetInviteListFromString(model.invites);
                foreach (Invite inv in invites)
                {
                    _ctx.Invites.AddOrUpdate(inv);
                    InviteRequest inviteRequest = new InviteRequest();
                    inviteRequest.gameRequest = gameRequest;
                    inviteRequest.invite = inv;
                    _ctx.InviteRequests.AddOrUpdate(inviteRequest);
                }
            }

            await _ctx.SaveChangesAsync();
            

            return Ok(gameRequest);
        }

        [Authorize]
        [Route("mygamerequests")]
        public async Task<IHttpActionResult> GetGameRequests()
        {

            IQueryable<GameRequest> query = getUsersGameRequests();

            return Ok(query.ToList());
        }

//        [Authorize]
//        [Route("accept")]
//        public async Task<IHttpActionResult> AcceptGameRequest(IdViewModel inviteRequestId)
//        {
//            Account acc = GetAccount();
//            
//            GameRequest gameRequest = _ctx.GameRequests.FirstOrDefault(gr => gr.invites.Any(i => i.id ==inviteRequestId.id && i.reciver == acc));
//            if (gameRequest == null)
//            {
//                BadRequest("Your where not invited for that game");
//            }
//            Invite invite = gameRequest.invites.FirstOrDefault(i => i.reciver == acc);
//            invite.inviteStatus = RequestStatus.Accepted;
//            _ctx.Invites.AddOrUpdate(invite);
//            await _ctx.SaveChangesAsync();
//
//            return Ok(invite);
//        }
//
//        [Authorize]
//        [Route("reject")]
//        public async Task<IHttpActionResult> RejectGameRequest(IdViewModel inviteRequestId)
//        {
//            Account acc = GetAccount();
//
//            GameRequest gameRequest = _ctx.GameRequests.FirstOrDefault(gr => gr.invites.Any(i => i.id == inviteRequestId.id && i.reciver == acc));
//            if (gameRequest == null)
//            {
//                BadRequest("Your where not invited for that game");
//            }
//            Invite invite = gameRequest.invites.FirstOrDefault(i => i.reciver == acc);
//            invite.inviteStatus = RequestStatus.Rejected;
//            _ctx.Invites.AddOrUpdate(invite);
//            await _ctx.SaveChangesAsync();
//
//            return Ok(invite);
//        }

        [Authorize]
        [Route("games")]
        public IHttpActionResult GetGames()
        {
            IQueryable<Game> query =
                from g in _ctx.Games
                select g;

            return Ok(query.ToList());
        }

        [Authorize]
        private Account GetAccount()
        {
            var user = User.Identity.GetUserName();
            ApplicationUser real = (ApplicationUser)_userManager.FindByNameAsync(user).Result;
            return _ctx.Accounts.FirstOrDefault(a => a.userId == real.Id);
        }

        private List<Invite> GetInviteListFromString(String inviteString)
        {
            List<Invite> inviteList = new List<Invite>();
            string[] stringList = inviteString.Split(';');

            for (int i = 0; i < stringList.Length-1; i++)
            {
                Invite invite = new Invite();
                string[] st = stringList[i].Split(':');
                int reciverId = int.Parse(st[0]);
                invite.reciver = _ctx.Accounts.FirstOrDefault(a => a.id == reciverId);
                invite.priority = int.Parse(st[1]);
                inviteList.Add(invite);
            }

            return inviteList;
        }

//        private DateTime GetDateTimeFromString(String dateTimeAsString)
//        {
//            DateTime dateTime = new DateTime();
//            dateTime = DateTime.ParseExact(dateTimeAsString, "yyyy-MM-dd HH:mm:ss,fff",
//                                            System.Globalization.CultureInfo.InvariantCulture);
//            return dateTime;
//        }

        private IQueryable<GameRequest> getUsersGameRequests()
        {
            Account acc = GetAccount();
            IQueryable<Invite> requestQuery =
                from i in _ctx.Invites
                where i.reciver.id == acc.id
                select i;

            List<Invite> invites = requestQuery.ToList();
            var inviteIds = invites.Select(i => i.id).ToList();

            IQueryable<InviteRequest> inviteQuery =
                from ir in _ctx.InviteRequests
                where inviteIds.Contains(ir.id)
                select ir;

            List<InviteRequest> inviteRequests = inviteQuery.ToList();
            var inviteRequestIds = inviteRequests.Select(ir => ir.id).ToList();

            IQueryable<GameRequest> query =
                from gr in _ctx.GameRequests
                where inviteRequestIds.Contains(gr.id)
                select gr;


            return query;
        } 




    }
}
