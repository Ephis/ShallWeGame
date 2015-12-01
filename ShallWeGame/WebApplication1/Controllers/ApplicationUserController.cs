using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.App_Start;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class ApplicationUserController : ApiController
    {
        [RoutePrefix("api/Account")]
        public class AccountController : ApiController
        {

            private AuthRepository _repo = null;

            private GameContext _ctx;

            private UserManager<ApplicationUser> _userManager;

            public AccountController()
            {
                _repo = new AuthRepository();
                _ctx = new GameContext();
                _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            }


            /// <summary>
            /// Create a new login
            /// </summary>
            /// <param name="login">String userName, String password, String email, String fName, String lName</param>
            /// <returns></returns>
            //POST api/Account/Register
            [ApiExplorerSettings(IgnoreApi = false)]
            [AllowAnonymous]
            [Route("Register")]
            public async Task<IHttpActionResult> Register(LoginViewModel login)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                login.firstLogin = true;
                IdentityResult result = await _repo.RegisterUser(login);
                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

                return Ok(result);
            }

            [Authorize]
            [Route("CurrentUser")]
            public async Task<IHttpActionResult> GetCurrentUser()
            {

                var user = User.Identity.GetUserName();
                var real = await _userManager.FindByNameAsync(user);
                var realUser = await _userManager.FindByIdAsync(real.Id);
                var account = _ctx.Accounts.Where(b => b.userId == realUser.Id);
                return Ok(account);
            }

            [Authorize]
            [Route("search")]
            public IHttpActionResult UserFromUserName(UserNameViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                IQueryable<Account> searchQuey =
                    from a in _ctx.Accounts
                    where a.name.Contains(model.userName)
                    select a;

                List<Account> accounts = new List<Account>();
                Account acc = getCurrentUser();
                foreach (Account account in searchQuey.ToList())
                {
                    if (account.id != acc.id)
                    {
                        accounts.Add(account);
                    }
                }

                return Ok(accounts);
            }

            private Account getCurrentUser()
            {
                var user = User.Identity.GetUserName();
                var real =  _userManager.FindByName(user);
                var realUser = _userManager.FindById(real.Id);
                Account account = _ctx.Accounts.FirstOrDefault(b => b.userId == realUser.Id);
                return account;
            }


            [AllowAnonymous]
            public IHttpActionResult AccountFromId(IdViewModel model)
            {
                return Ok(_ctx.Accounts.FirstOrDefault(a => a.id == model.id));
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _repo.Dispose();
                }
                base.Dispose(disposing);
            }

            private IHttpActionResult GetErrorResult(IdentityResult result)
            {
                if (result == null)
                {
                    return InternalServerError();
                }

                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        foreach (String error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        //Ingen fejlkode, derfor sendes der bare en badrequest
                        return BadRequest();
                    }
                }

                return null;

            }

        }
    }
}
