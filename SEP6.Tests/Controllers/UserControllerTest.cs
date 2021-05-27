using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SEP6.Controllers;
using SEP6.DB;
using Xunit;
using ILogger = Castle.Core.Logging.ILogger;

namespace SEP6.Tests.Controllers
{
    public class UserControllerTest
    {
        
        private readonly Mock<MoviesDbContext> _dbMOck;
        private readonly Mock<DbSet<User>> _dbSetMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        

        public UserControllerTest()
        {
            _dbMOck = new Mock<MoviesDbContext>();
            _dbSetMock = new Mock<DbSet<User>>();
            _loggerMock = new Mock<ILogger<UserController>>();

            var userToFollow = new User()
            {
                Id = 2,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "string",
                Username = "stringy",
                Token = "ssss",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[0]
            };
            
            var data = new List<User>
            {
                new User() { Id = 1,
                    Birthday =  "string",
                    Country =  "string",
                    Email =  "string",
                    Name =  "string",
                    Password =  "string",
                    Username =  "string",
                    Token = "ssss",
                    Followers = new List<User>(){userToFollow},
                    Follows = new List<User>(),
                    PasswordSalt = new byte[0]
                },
                userToFollow
            }.AsQueryable();
            
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        [Fact]
        public void Follow_FollowUser()
        {
            var controller = new UserController(_loggerMock.Object,_dbMOck.Object);
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMOck.Setup(t => t.SaveChanges());

            var result = (OkObjectResult) controller.Follow("1=ssss", "stringy");

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.NotNull((string)result.Value);
            _dbMOck.Verify(t=>t.SaveChanges(),Times.Once);
        }
        
        [Fact]
        public void Follow_UnfollowUser()
        {
            var controller = new UserController(_loggerMock.Object,_dbMOck.Object);
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMOck.Setup(t => t.SaveChanges());
            var userToFollow = "stringy";
            
            //Follows
            controller.Follow("1=ssss", userToFollow);
            //Unfollows
            var result = (OkObjectResult) controller.Follow("1=ssss", "stringy");

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.Equal($"string doesn't follow {userToFollow} anymore!",(string)result.Value);
            _dbMOck.Verify(t=>t.SaveChanges(),Times.Exactly(2));
        }

        [Fact]
        public void Follow_WrongToken()
        {
            var controller = new UserController(_loggerMock.Object,_dbMOck.Object);
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMOck.Setup(t => t.SaveChanges());

            var result = (UnauthorizedObjectResult) controller.Follow("1=sssd", "stringy");

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.Unauthorized, (HttpStatusCode) result.StatusCode);
            Assert.Equal("Token expired",(string)result.Value);
            _dbMOck.Verify(t=>t.SaveChanges(),Times.Never);
        }
        
        [Fact]
        public void Follow_NotFound()
        {
            var controller = new UserController(_loggerMock.Object,_dbMOck.Object);
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMOck.Setup(t => t.SaveChanges());

            var userToFollow = "stringd";
            var result = (NotFoundObjectResult) controller.Follow("1=ssss", userToFollow);

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode) result.StatusCode);
            Assert.Equal($"User {userToFollow} not found",(string)result.Value);
            _dbMOck.Verify(t=>t.SaveChanges(),Times.Never);
        }
    }
}