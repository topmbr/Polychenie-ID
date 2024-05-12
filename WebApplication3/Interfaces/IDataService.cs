using WebApplication3.Controllers;

namespace WebApplication3.Interfaces
{
    public interface IDataService
    {
        Task<User> GetUser(int id);
    }
}
