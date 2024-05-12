using System.Threading.Tasks;
using ProjectNamespace.Models;

namespace ProjectNamespace.Services
{
    public interface IDbService
    {
        Task<User> GetUser(int id);
        Task AddUser(User user);
    }
}

