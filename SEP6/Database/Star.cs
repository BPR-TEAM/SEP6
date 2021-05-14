using System;
using System.Collections.Generic;
using SEP6.Database;

#nullable disable

namespace SEP6
{
    public partial class Star
    {
        public long MovieId { get; set; }
        public long PersonId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
