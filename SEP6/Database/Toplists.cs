using System.Collections.Generic;

namespace SEP6.Database
{
    public class Toplists
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        

        public int UserId { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}