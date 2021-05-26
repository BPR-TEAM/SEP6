using System;
using System.Collections.Generic;

#nullable disable

namespace SEP6.DB
{
    public partial class Person
    {
        public Person()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public short? Birth { get; set; }

        public virtual ICollection<Director> Directors { get; set; }
    }
}
