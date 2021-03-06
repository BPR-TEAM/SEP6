using System;
using System.Collections.Generic;

namespace SEP6.Database
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        
        
        public virtual List<Toplists> UserTopLists { get; set; }
        
        public virtual string Token { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual ICollection<User> Follows { get; set; }
        public virtual ICollection<User> Followers { get; set; }
        
    }
}