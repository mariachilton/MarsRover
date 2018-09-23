using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MarsRover.Models
{
    /// <summary>
    /// MarsRoverItem class
    /// </summary>
    public class MarsRoverItem
    {
        /// <summary>
        /// Rover unique identifier
        /// </summary>
        [Key,Required]
        public int RoverId { get; set; }

        /// <summary>
        /// Rover name
        /// </summary>
        [Required]
        public string RoverName { get; set; }

        /// <summary>
        /// Rover current position on X coordinates
        /// </summary>
        [Required]
        public int CurrentX { get; set; }

        /// <summary>
        /// Rover current position on Y coordinates
        /// </summary>
        [Required]
        public int CurrentY { get; set; }

        /// <summary>
        /// Rover current direction (N,S,E,W)
        /// </summary>
        [Required]
        public string CurrentDirection { get; set; }

    }
}
