using System;
using System.Collections.Generic;

#nullable disable

namespace SEP6.DB
{
    public partial class TopLists
    {
        public TopLists()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
