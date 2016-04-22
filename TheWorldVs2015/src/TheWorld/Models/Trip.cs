using System;
using System.Collections.Generic;

namespace TheWorld.Models
{
    public class Trip
    {
        public DateTime Created { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Stop> Stops { get; set; }

        public string UserName { get; set; }
    }
}
