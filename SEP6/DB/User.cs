using System;
using System.Collections.Generic;

#nullable disable

namespace SEP6.DB
{
    public partial class User
    {
        public User()
        {
        }

        public int Id { get; set; }
        public string Birthday { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public virtual string Token { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual ICollection<User> Followers { get; set; }
        public virtual ICollection<User> Follows { get; set; }
        public virtual ICollection<TopLists> TopLists { get; set; }
    }
}
