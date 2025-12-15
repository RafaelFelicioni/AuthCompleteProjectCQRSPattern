using CleanArchMonolit.Domain.Auth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Interfaces.Token
{
    public interface IGenerateTokenService
    {
        string GenerateToken(User user);
    }
}
