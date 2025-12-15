using CleanArchMonolit.Application.Auth.Interfaces.Interface;
using PoliticalPartyEntity = CleanArchMonolit.Domain.PoliticalParty.Entities.PoliticalParty;

namespace CleanArchMonolit.Infrastructure.PoliticalParty.Repositories.PoliticalPartyRepository
{
    public interface IPoliticalPartyRepository : IRepository<PoliticalPartyEntity>
    {
    }
}
