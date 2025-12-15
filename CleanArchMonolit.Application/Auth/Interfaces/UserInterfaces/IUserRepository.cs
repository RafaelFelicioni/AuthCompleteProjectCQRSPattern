using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Interfaces.Interface;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.DTO;
using CleanArchMonolit.Shared.DTO.Grid;

namespace CleanArchMonolit.Application.Auth.Repositories.UserRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetById(int id);
        Task<GridResponseDTO<ReturnUsersGridDTO>> GetUserGrid(GetUsersGridDTO dto);
        Task<List<SelectPatternDTO>> GetSelectUser(string term, bool isAdmin);
        Task<bool> CheckIfTaxIdExists(string taxId);
    }
}