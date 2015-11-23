using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.App_Start
{
    public class AuthRepository : IDisposable
    {
        private GameContext _ctx;

        private UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            _ctx = new GameContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(LoginViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.username,
                Email = model.email,
            };
            user.firstLogin = model.firstLogin;

            var result = await _userManager.CreateAsync(user, model.password);

            return result;
        }

        public async Task<ApplicationUser> FindUser(string username, string password)
        {
            ApplicationUser user = (ApplicationUser)await _userManager.FindAsync(username, password);

            return user;
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}