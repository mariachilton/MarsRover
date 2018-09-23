using System;
using Microsoft.AspNetCore.Mvc;
using MarsRover.Models;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace MarsRover.Controllers
{
    /// <summary>
    /// MarsRoverController manage MarsRoverItem objects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MarsRoverController : ControllerBase
    {
        private readonly MarsRoverContext _context;

        /// <summary>
        /// Constructor intializes the _context to access db.
        /// </summary>
        /// <param name="context"> DB instance </param>
        public MarsRoverController(MarsRoverContext context) => _context = context;

        /// <summary>
        /// Retrieves the current position of the Mars Rover
        /// </summary>
        /// <param name="RoverId">Rover unique identifier</param>
        /// <returns>Mars Rover {RoverID, RoverName, CurrentX, CurrentY, CurrentDirection}</returns>
        [HttpGet]
        [Route("Retrieve")]
        public ActionResult<MarsRoverItem> GetById([Required]int RoverId)
        {
            MarsRoverItem item = _context.MarsRoverItems.Find(RoverId);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        /// <summary>
        /// Creates a new Mars Rover
        /// </summary>
        /// <param name="RoverId">Rover unique identifier</param>
        /// <param name="RoverName">Rover name</param>
        /// <returns>An Action result: 400 BadRequest when fails or 200 OK when success</returns>
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([Required]int RoverId, [Required]string RoverName)
        {
            MarsRoverItem item = _context.MarsRoverItems.Find(RoverId);
            if (item != null)
            {
                return BadRequest("Id already exists");
            }
            item = new MarsRoverItem
                {
                    RoverId = RoverId,
                    RoverName = RoverName,
                    CurrentDirection = "N",
                    CurrentX = 0,
                    CurrentY = 0
                };
                _context.MarsRoverItems.Add(item);
                _context.SaveChanges();
                return Ok();
        }

        /// <summary>
        /// Renames an existing Mars Rover
        /// </summary>
        /// <param name="RoverId">Rover unique identifier</param>
        /// <param name="RoverName">New rover name</param>
        /// <returns>404 NotFound when object doesn't exist or 200 OK when success</returns>
        [HttpPatch]
        [Route("Rename")]
        public IActionResult Rename([Required]int RoverId, [Required]string RoverName)
        {
            MarsRoverItem item = _context.MarsRoverItems.Find(RoverId);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                item.RoverName = RoverName;
                _context.MarsRoverItems.Update(item);
                _context.SaveChanges();
                return Ok();
            }
        }

        /// <summary>
        /// Moves Mars Rover
        /// </summary>
        /// <param name="RoverId">Rover unique identifier</param>
        /// <param name="MovementInstruction">Instructions to move rover </param>
        /// <remarks>The possible letters for instructions are “L”, “R”, and “M”. “L” 
        ///  and “R” makes the rover spin 90 degrees left or right respectively, without moving from its current spot. “M”
        ///  moves the rover forward one grid point, and maintains the same heading.</remarks>
        /// <returns>A 400 BadRequest when fails or 200 OK when success</returns>
        [HttpPatch]
        [Route("Move")]
        public IActionResult Move([Required]int RoverId, [Required]string MovementInstruction)
        {
            var regex = new Regex("^(L|M|R|l|m|r)*$");
            if (!regex.IsMatch(MovementInstruction))
            {
                return BadRequest("Invalid movement instruction");
            }
            else
            {
                MarsRoverItem item = _context.MarsRoverItems.Find(RoverId);
                if (item == null)
                {
                    return NotFound();
                }
                else
                {
                    ApplyMovement(MovementInstruction, item);
                    _context.MarsRoverItems.Update(item);
                    _context.SaveChanges();
                    return Ok();
                }
            }
        }

        /// <summary>
        /// Applies logic to move MarsRover
        /// </summary>
        /// <param name="MovementInstruction">The possible letters are “L”, “R”, and “M”. “L” 
        ///  and “R” makes the rover spin 90 degrees left or right respectively, without moving from its current spot. “M”
        ///  moves the rover forward one grid point, and maintains the same heading.</param>
        /// <param name="item">MarsRoverItem object to move</param>
        private void ApplyMovement(string MovementInstruction, MarsRoverItem item)
        {
            Direction direction = (Direction)Enum.Parse(typeof(Direction), item.CurrentDirection);
            foreach (char c in MovementInstruction)
            { 
                switch (c)
                {
                    case 'L':
                    case 'l':
                        if(direction == Direction.N)
                        {
                            direction = Direction.W;
                        }
                        else
                        {
                            direction--;
                        }
                        break;
                    case 'R':
                    case 'r':
                        if (direction == Direction.W)
                        {
                            direction = Direction.N;
                        }
                        else
                        {
                            direction++;
                        }
                        break;
                    case 'M':
                    case 'm':
                        switch (direction)
                        {
                            case Direction.N:
                                item.CurrentY++;
                                break;
                            case Direction.S:
                                item.CurrentY--;
                                break;
                            case Direction.E:
                                item.CurrentX++;
                                break;
                            case Direction.W:
                                item.CurrentX--;
                                break;

                        }
                        break;
                }
            }
            item.CurrentDirection = direction.ToString();
        }

        /// <summary>
        /// Enum to define a value for each direction where the MarsRover item can spin (N,S,E,W)
        /// </summary>
        public enum Direction{
            /// <summary>
            /// N = North
            /// </summary>
            N = 0,

            /// <summary>
            /// E = East
            /// </summary>
            E,

            /// <summary>
            /// S = South
            /// </summary>
            S,

            /// <summary>
            /// W = West
            /// </summary>
            W
            }
    }
}
