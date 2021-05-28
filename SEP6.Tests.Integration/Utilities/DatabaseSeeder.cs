using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SEP6.DB;

namespace SEP6.Tests.Integration
{
    public class DatabaseSeeder
    {
        public static MoviesDbContext MoviesDb;

        public static void SeedDatabase(MoviesDbContext db,bool isEmpty = false)
        {
            MoviesDb = db;
            MoviesDb.Database.EnsureCreated();
            Clear(MoviesDb.Users);
            Clear(MoviesDb.TopLists);
            MoviesDb.SaveChanges();

            if (isEmpty) return;
            
            SeedUsers();
            SeedMovies();
            MoviesDb.SaveChanges();
        }

        private static void SeedUsers()
        {
            var user = new User()
            {
                Id = 1,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "string",
                Username = "string",
                Token = "ssss",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[0]
            };
            var userFollower = new User()
            {
                Id = 2,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "string",
                Username = "stringy",
                Token = "hhhh",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[0]
            };
            user.Followers.Add(userFollower);
            MoviesDb.Users.AddRange(user,
                    userFollower);
        }

        private static void SeedMovies()
        {
            MoviesDb.AddRange(new Movie()
            {
                Title = "yo",
                Id = 1,
                Year = 2000
            });
        }
        private static void Clear<T>(DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}