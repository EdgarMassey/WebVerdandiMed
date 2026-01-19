using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebVerdandiMedReg.Data;

namespace WebVerdandiMedReg.Pages.Membership
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // These are user-chosen filters from the URL (SupportsGet = true)
        [BindProperty(SupportsGet = true)]
        public string? Distrikt { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Avdelning { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Sort { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Desc { get; set; }

        // Dropdown options
        public List<string> DistriktOptions { get; private set; } = new();
        public List<string> AvdelningOptions { get; private set; } = new();

        // Result list
        public List<Data.Membership> Items { get; private set; } = new();

        // These are user-chosen filters from the URL (SupportsGet = true)
        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        public async Task OnGetAsync()
        {
            // 1) Build dropdown options (distinct values from DB)
            DistriktOptions = await _db.Memberships
                .AsNoTracking()
                .Select(m => m.Distrikt)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync()!;

            // Avdelning options depend on selected Distrikt (cascading)
            var avdQuery = _db.Memberships.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Distrikt))
                avdQuery = avdQuery.Where(m => m.Distrikt == Distrikt);

            AvdelningOptions = await avdQuery
                .Select(m => m.Avdelning)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync()!;

            var q = _db.Memberships.AsNoTracking().AsQueryable();

            // Existing filters (keep these if you use them)
            if (!string.IsNullOrWhiteSpace(Distrikt))
                q = q.Where(m => m.Distrikt == Distrikt);

            if (!string.IsNullOrWhiteSpace(Avdelning))
                q = q.Where(m => m.Avdelning == Avdelning);

            if (!string.IsNullOrWhiteSpace(Q))
            {
                var term = Q.Trim();

                q = q.Where(m =>
                    (m.Mednamn != null && m.Mednamn.Contains(term)) ||
                    (m.Mednummer != null && m.Mednummer.Contains(term))
                );
            }


            // Sorting
            q = (Sort, Desc) switch
            {
                ("Mednummer", false) => q.OrderBy(x => x.Mednummer),
                ("Mednummer", true) => q.OrderByDescending(x => x.Mednummer),

                ("Mednamn", false) => q.OrderBy(x => x.Mednamn),
                ("Mednamn", true) => q.OrderByDescending(x => x.Mednamn),

                ("Distrikt", false) => q.OrderBy(x => x.Distrikt),
                ("Distrikt", true) => q.OrderByDescending(x => x.Distrikt),

                ("Avdelning", false) => q.OrderBy(x => x.Avdelning),
                ("Avdelning", true) => q.OrderByDescending(x => x.Avdelning),

                _ => q.OrderBy(x => x.Mednummer) // default sort
            };

            List<Data.Membership> memberships = await q.ToListAsync();
            
            // Assign the result to Items property
            Items = memberships;
        }
    }
}




