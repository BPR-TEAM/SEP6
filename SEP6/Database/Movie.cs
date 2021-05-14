using System.Collections.Generic;

#nullable disable

namespace SEP6.Database
{
    public partial class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public byte[] Year { get; set; }
        
        
        public virtual ICollection<Toplists> Toplistses { get; set; }
    }
}
