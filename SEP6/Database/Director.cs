#nullable disable

namespace SEP6.Database
{
    public partial class Director
    {
        public long MovieId { get; set; }
        public long PersonId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
