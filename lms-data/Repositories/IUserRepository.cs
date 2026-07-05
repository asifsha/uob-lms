using System;
using lms_data.Entities;

namespace lms_data.Repositories
{

    public interface IUserRepository : IRepository<UserEntity>
    {

        Task<UserEntity?> GetByUsernameAsync(string username);

        Task<bool> ExistsAsync(string username);
    }

}
