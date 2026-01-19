using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebVerdandiMedReg.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersModel : PageModel
    {
        private const string ROLE_POWERUSER = "PowerUser";
        private const string ROLE_SUPERADMIN = "SuperAdmin";

        private readonly UserManager<IdentityUser> _userManager;

        public UsersModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string? NewEmail { get; set; }

        [BindProperty]
        public string? NewPassword { get; set; }

        public List<UserRow> Users { get; private set; } = new();

        public class UserRow
        {
            public string Id { get; set; } = "";
            public string? Email { get; set; }
            public string? UserName { get; set; }
            public IList<string> Roles { get; set; } = new List<string>();
        }

        public async Task OnGetAsync()
        {
            var all = _userManager.Users.ToList();

            var rows = new List<UserRow>();
            foreach (var u in all)
            {
                rows.Add(new UserRow
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    Roles = await _userManager.GetRolesAsync(u)
                });
            }

            Users = rows.OrderBy(x => x.Email ?? x.UserName).ToList();
        }

        public async Task<IActionResult> OnPostCreateUserAsync()
        {
            if (string.IsNullOrWhiteSpace(NewEmail) || string.IsNullOrWhiteSpace(NewPassword))
                return RedirectToPage();

            var user = new IdentityUser
            {
                UserName = NewEmail,
                Email = NewEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMakePowerUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(ROLE_SUPERADMIN)) return RedirectToPage();

            await _userManager.AddToRoleAsync(user, ROLE_POWERUSER);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemovePowerUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var me = await _userManager.GetUserAsync(User);
            if (me != null && me.Id == user.Id) return RedirectToPage();

            await _userManager.RemoveFromRoleAsync(user, ROLE_POWERUSER);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Safety: don't delete yourself
            var me = await _userManager.GetUserAsync(User);
            if (me != null && me.Id == user.Id)
                return RedirectToPage();

            // Safety: don't delete SuperAdmin accounts
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(ROLE_SUPERADMIN))
                return RedirectToPage();

            await _userManager.DeleteAsync(user);
            return RedirectToPage();
        }
    }
}

