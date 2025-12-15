using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Infrastructure.DataShared;
using CleanArchMonolit.Infrastruture.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchMonolit.Infrastructure.Auth.Repositories.ProfileRepositories
{
    public class ProfileRepository : BaseRepository<Profiles, AuthDbContext>, IProfileRepository
    {
        private readonly AuthDbContext _context;

        public ProfileRepository(AuthDbContext context) : base(context)
        {
            _context = context;
        }

        public virtual async Task<Profiles> GetById(int id)
        {
            return await GetDbSet().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
