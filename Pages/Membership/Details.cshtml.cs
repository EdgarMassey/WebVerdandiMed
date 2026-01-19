using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebVerdandiMedReg.Data;

namespace WebVerdandiMedReg.Pages.Membership
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public Data.Membership? Member { get; private set; }

        public async Task<IActionResult> OnGetAsync(string mednummer)
        {
            if (string.IsNullOrWhiteSpace(mednummer))
                return NotFound();

            Member = await _db.Memberships
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Mednummer == mednummer);

            if (Member == null)
                return NotFound();

            return Page();
        }
    }
}
