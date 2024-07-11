// Make interface

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Models;

namespace server.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> GetByID(string id);
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string email);
        Task Create(User newUser);
        Task Update(string id, User userIn);
        Task Remove(string id);
    }
}

