#nullable disable
using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Infrastructure.DataShared;
using CleanArchMonolit.Infrastruture.Data;
using CleanArchMonolit.Shared.DTO;
using CleanArchMonolit.Shared.DTO.Grid;
using CleanArchMonolit.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchMonolit.Infrastructure.Auth.Repositories.UserRepositories
{
    public class UserRepository : BaseRepository<User, AuthDbContext>, IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetDbSet()
                .Include(u => u.Profile)// se precisar do perfil no JWT
                .Include(x => x.UserPermissions)
                .ThenInclude(x => x.SystemPermission)
                .FirstOrDefaultAsync(u => u.Mail == email);
        }

        public async Task<User> GetById(int id)
        {
            return await GetDbSet().Include(x => x.UserPermissions).ThenInclude(x => x.SystemPermission).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CheckIfTaxIdExists(string taxId)
        {
            return await GetDbSet().AnyAsync(x => x.TaxId == taxId);
        }

        public async Task<GridResponseDTO<ReturnUsersGridDTO>> GetUserGrid(GetUsersGridDTO dto)
        {
            var resp = new GridResponseDTO<ReturnUsersGridDTO>();
            var users = GetDbSet().Where(x =>
                (!dto.UserId.HasValue || (dto.UserId.HasValue && dto.UserId.Value < 1) || (dto.UserId.HasValue && dto.UserId.Value == x.Id)) &&
                (!dto.ProfileId.HasValue || (dto.ProfileId.HasValue && dto.ProfileId.Value < 1) || (dto.ProfileId.HasValue && dto.ProfileId.Value == x.Id))
             ).Select(x => new ReturnUsersGridDTO()
             {
                 UserName = x.Username,
                 ProfileName = x.Profile.ProfileName,
             });

            var sortDir = false;
            if (string.IsNullOrWhiteSpace(dto.SortDirection) || dto.SortDirection.ToLower() == "asc")
            {
                sortDir = true;
            }

            users.LinqOrderBy(dto.SortBy, sortDir);
            resp.TotalItems = users.Count();
            resp.Page = dto.Page; 
            resp.PageSize = dto.PageSize;
            resp.Items = await users.Select(x => new ReturnUsersGridDTO()
            {
                ProfileName = x.ProfileName,
                UserName = x.UserName,
            }).Skip(dto.Skip).Take(dto.Take).ToListAsync();

            return resp;
        }

        public async Task<List<SelectPatternDTO>> GetSelectUser(string term, bool isAdmin)
        {
            return await GetDbSet().Where(x => (!isAdmin && (EF.Functions.Like(x.TaxId, $"%{term}%")
                || EF.Functions.Like(x.Username, $"%{term}%"))) ||
                (isAdmin && (EF.Functions.Like(x.TaxId, $"%{term}%") || EF.Functions.Like(x.Username, $"%{term}%")))
                    ).Select(x => new SelectPatternDTO()
                    {
                        Value = x.Id,
                        Info = $"{x.Username} (${x.TaxId})"
                    }).ToListAsync();
        }
    }
}
