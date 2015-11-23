﻿using System;
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
    [RoutePrefix("gamerequest")]
    public class GameRequestController : ApiController
    {

        private UserManager<ApplicationUser> _userManager;
        public GameContext _ctx;
        
        public GameRequestController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _ctx = new GameContext();
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
            gameRequest.invites = GetInviteListFromString(model.invites);
            gameRequest.gameToPlay = _ctx.Games.FirstOrDefault(g => g.id == model.gameId);
            gameRequest.owner = GetAccount();
            gameRequest.timeItBegins = GetDateTimeFromString(model.timeItBegins);
            gameRequest.timeItEnds = GetDateTimeFromString(model.timeItEnds);
            gameRequest.playersNeeded = model.playersNeeded;

            foreach (Invite inv in GetInviteListFromString(model.invites))
            {
                _ctx.Invites.AddOrUpdate(inv);
            }
            _ctx.GameRequests.AddOrUpdate(gameRequest);
            int x = await _ctx.SaveChangesAsync();
            

            return Ok(gameRequest);
        }

        [Authorize]
        [Route("mygamerequests")]
        public async Task<IHttpActionResult> GetGameRequests()
        {
            IQueryable<Invite> request =
                from i in _ctx.Invites
                where i.reciver.id == GetAccount().id
                select i;

            List<Invite> invites = request.ToList();

            IQueryable<GameRequest> query =
                from gr in _ctx.GameRequests
                where gr.invites.Any(r => invites.Any(i => i.id == r.id))
                select gr;


            return Ok(query.ToList());
        }


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

            foreach (String s in stringList)
            {
                Invite invite = new Invite();
                string[] st = s.Split(':');
                invite.reciver = _ctx.Accounts.FirstOrDefault(a => a.id == int.Parse(st[0]));
                invite.priority = int.Parse(st[1]);
                inviteList.Add(invite);
            }

            return inviteList;
        }

        private DateTime GetDateTimeFromString(String dateTimeAsString)
        {
            DateTime dateTime = new DateTime();

            return dateTime;
        }




    }
}