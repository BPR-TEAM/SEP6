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

namespace SEP6.Tests.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<MoviesDbContext> _dbMock;
        private readonly Mock<DbSet<User>> _dbSetMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;

        public AuthControllerTest()
        {
            _dbMock = new Mock<MoviesDbContext>();
            _dbSetMock = new Mock<DbSet<User>>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            var userToBeAuthenticated = new User()
            {
                Id = 0,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "string",
                Username = "stringy",
                Token = "0=asdhfhgfd",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[0]
            };
            
            var data = new List<User>
            {
                userToBeAuthenticated
            }.AsQueryable();
            
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        [Fact]
        public void RegisterTest()
        {
            var controller = new AuthController(_loggerMock.Object,_dbMock.Object);
            _dbMock.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMock.Setup(t => t.SaveChanges());

            User testUser = new User()
            {
                Birthday = "16/04/1988",
                Name = "Habibi",
                Username = "Habibi420",
                Email = "habibi@habibiairlines.com",
                Password = "hashmebabyonemoretime"
            };

            var result = (OkObjectResult) controller.SignUp(testUser);

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.NotNull((string)result.Value);
            _dbMock.Verify(t=>t.SaveChanges(),Times.Once);
        }

        [Fact]
        public void LoginTest()
        {
            var controller = new AuthController(_loggerMock.Object,_dbMock.Object);
            _dbMock.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMock.Setup(t => t.SaveChanges());

            User testUser = new User()
            {
                Id = 0,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "string",
                Username = "stringy",
                Token = "0=asdhfhgfd",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[0]
            };

            var result = (UnauthorizedObjectResult) controller.Login(testUser);

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.NotNull((string)result.Value);
            _dbMock.Verify(t=>t.SaveChanges(),Times.Once);
        }

        [Fact]
        public void LogoutTest()
        {
            var controller = new AuthController(_loggerMock.Object,_dbMock.Object);
            _dbMock.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMock.Setup(t => t.SaveChanges());

            User testUser = new User()
            {
                Id = 0,
                Name = "string",
                Username = "stringy",
                Email = "string",
                Password = "string",
                Token = "0=asdhfhgfd"
            };

            var result = (UnauthorizedObjectResult) controller.Logout(testUser.Token);

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.NotNull((string)result.Value);
            _dbMock.Verify(t=>t.SaveChanges(),Times.Once);
        }
    }
}