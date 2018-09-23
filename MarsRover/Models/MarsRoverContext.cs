using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MarsRover.Models
{
    /// <summary>
    /// MarsRoverContext to handle DB instance
    /// </summary>
    public class MarsRoverContext : DbContext
    {
        /// <summary>
        /// MarsRoverContext constructor
        /// </summary>
        /// <param name="options"></param>
        public MarsRoverContext(DbContextOptions<MarsRoverContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// MarsRoverItems DBSet
        /// </summary>
        public DbSet<MarsRoverItem> MarsRoverItems { get; set; }
    }
}
