using ApiDevBP.Entities;
using ApiDevBP.Models;
using ApiDevBP.Models.DTO;

namespace ApiDevBP.Repository.IRopository
{
    public interface IUserRepository
    {

        bool CreateUser(UserEntity user);

        bool DeleteUser(UserEntity user);

        bool UpdateUser(UserEntity user);

        ICollection<UserEntity> GetUsers();

        UserEntity GetUserById(int userId);

        ICollection<UserEntity> SearchUser(string name);

        bool Save();

    }
}
