using CleanArchMonolit.Infrastructure.PoliticalParty.Data;
using CleanArchMonolit.Infrastructure.DataShared;
using Microsoft.AspNetCore.Http;
using PoliticalPartyEntity = CleanArchMonolit.Domain.PoliticalParty.Entities.PoliticalParty;

namespace CleanArchMonolit.Infrastructure.PoliticalParty.Repositories.PoliticalPartyRepository
{

    public class PoliticalPartyRepository : BaseRepository<PoliticalPartyEntity, PoliticalPartyDbContext>, IPoliticalPartyRepository
    {
        private readonly PoliticalPartyDbContext _context;
        public PoliticalPartyRepository(PoliticalPartyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
