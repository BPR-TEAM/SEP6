using System;
using System.Collections.Generic;

#nullable disable

namespace SEP6.DB
{
    public partial class Movie
    {
        public Movie()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Director> Directors { get; set; }
        
        public virtual ICollection<TopLists> TopListses { get; set; }
    }
}
