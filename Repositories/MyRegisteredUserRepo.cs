using DECKMASTER.Data;
using DECKMASTER.Models;

namespace DECKMASTER.Repositories
{
    public class MyRegisteredUserRepo
    {
        private readonly ApplicationDbContext _db;

        public MyRegisteredUserRepo(ApplicationDbContext db)
        {
            this._db = db;
        }

        public void RegisterUser(MyRegisteredUser registerUser)
        {
            _db.MyRegisteredUsers.Add(registerUser);
            _db.SaveChanges();

        }
        public MyRegisteredUser GetRegisteredUser(string email)
        {
            var user = _db.MyRegisteredUsers.FirstOrDefault(u => u.Email == email);
            return user;

        }

        public string GetFullNameByEmail(string email)
        {
            var user = _db.MyRegisteredUsers.FirstOrDefault(u => u.Email == email);
            return user != null ? $"{user.FirstName} {user.LastName}" : null;
        }

    }
}
