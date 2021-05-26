using System;
using System.Collections.Generic;

#nullable disable

namespace SEP6.DB
{
    public partial class Rating
    {
        public int MovieId { get; set; }
        public float Rating1 { get; set; }
        public int Votes { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
