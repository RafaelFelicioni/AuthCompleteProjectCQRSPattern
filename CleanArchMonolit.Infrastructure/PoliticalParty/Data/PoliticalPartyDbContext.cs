using Microsoft.EntityFrameworkCore;
using PoliticalPartyEntity = CleanArchMonolit.Domain.PoliticalParty.Entities.PoliticalParty;

namespace CleanArchMonolit.Infrastructure.PoliticalParty.Data
{
    public class PoliticalPartyDbContext : DbContext
    {
        public PoliticalPartyDbContext(DbContextOptions<PoliticalPartyDbContext> options) : base(options) { }

        public DbSet<PoliticalPartyEntity> PoliticalParties { get; set; }
    }
}
