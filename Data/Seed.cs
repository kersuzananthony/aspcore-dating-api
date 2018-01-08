using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DatingAPI.Models;
using Newtonsoft.Json;

namespace DatingAPI.Data
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void SeedUsers()
        {
            _dataContext.Users.RemoveRange(_dataContext.Users);
            _dataContext.SaveChanges();

            var userData = File.ReadAllText("Data/user-seed-data.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;

                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _dataContext.Users.Add(user);
            }

            _dataContext.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}