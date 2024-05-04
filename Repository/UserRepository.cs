using ApiDevBP.DATA;
using ApiDevBP.Entities;
using ApiDevBP.Models;
using ApiDevBP.Repository.IRopository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiDevBP.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateUser(UserEntity user)
        {
            _db.userEntities.AddAsync(user);

            return Save();
        }

        public bool DeleteUser(UserEntity user)
        {
            _db.userEntities.Remove(user);

            return Save();
        }

        public bool UpdateUser(UserEntity user)
        {
            _db.userEntities.Update(user);

            return Save();
        }

        ICollection<UserEntity> IUserRepository.GetUsers()
        {
            return _db.userEntities.OrderBy(u => u.Name).ToList();
        }

        public ICollection<UserEntity> SearchUser(string name)
        {
            IQueryable<UserEntity> query = _db.userEntities;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.Name.Contains(name) || u.Lastname.Contains(name));
            }

            return query.ToList();
        }

        public UserEntity GetUserById(int userId)
        {
            return _db.userEntities.FirstOrDefault(u => u.Id.Equals(userId));
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
 
    }
}
