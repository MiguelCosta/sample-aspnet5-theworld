using System;
using System.ComponentModel.DataAnnotations;

namespace TheWorld.ViewModels
{
    public class StopViewModel
    {
        [Required]
        public DateTime Arrival { get; set; }

        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
    }
}
