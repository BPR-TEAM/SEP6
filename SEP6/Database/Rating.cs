using System;
using System.Collections.Generic;
using SEP6.Database;

#nullable disable

namespace SEP6
{
    public partial class Rating
    {
        public long MovieId { get; set; }
        public double Rating1 { get; set; }
        public long Votes { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
