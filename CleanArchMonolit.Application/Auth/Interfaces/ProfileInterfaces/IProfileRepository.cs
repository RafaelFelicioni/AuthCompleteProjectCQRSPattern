using CleanArchMonolit.Application.Auth.Interfaces.Interface;
using CleanArchMonolit.Domain.Auth.Entities;

namespace CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories
{
    public interface IProfileRepository : IRepository<Profiles>
    {
        Task<Profiles> GetById(int id);
    }
}
