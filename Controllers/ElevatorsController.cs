using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocket_Elevators_REST_API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Rocket_Elevators_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ElevatorsController(ApplicationContext context)
        {
            _context = context;
        }

        //-----------------------------------------------------
        // ADDED ENDPOINTS:
        //-----------------------------------------------------


        // GET: api/Elevators/5/status
        // get specific elevator's status
        [HttpGet("{id}/status")]
        public async Task<ActionResult<String>> GetElevatorStatus(int id)
        {
            var elevator = await _context.elevators.FindAsync(id);

            if (elevator == null)
            {
                return NotFound();
            }

            return "Elevator " + elevator.id + "'s status is: " + elevator.status;
        }

        // GET: api/Elevators/offline
        // get all elevators that are not "Online"
        [HttpGet("offline")]
        public async Task<ActionResult<List<Elevator>>> GetOfflineElevators()
        {
            var elevator = await _context.elevators
                .Where(c => c.status != "Online").ToListAsync();

            if (elevator == null)
            {
                return NotFound();
            }

            return elevator;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Elevator>> getElevator(int id){
            var elevator = await _context.elevators.FindAsync(id);
            if(elevator == null){
                return NotFound();
            }
            return elevator;
        }

        // PATCH: api/Elevators/5
        // update status (or any single field) of an elevator using the following format:
            // [{"op": "replace", "path": "/status", "value": "Offline"}]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchElevatorStatus(int id, [FromBody]JsonPatchDocument<Elevator> elevatorPatch)
        {
            var elevator = await _context.elevators.FindAsync(id);
            elevatorPatch.ApplyTo(elevator);

            await _context.SaveChangesAsync();

            return Content("Successfully updated elevator " + elevator.id);
        }

        [HttpGet("column_id={column_id}")]
        public async Task<ActionResult> getBatteries(int column_id)
        {

            List<Elevator> ColumnList = new List<Elevator>();
            ColumnList = await _context.elevators.Where(b => b.column_id == column_id).ToListAsync();

            if (ColumnList == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ColumnList);
            }
        }



        //-----------------------------------------------------
        // END
        //-----------------------------------------------------

        private bool ElevatorExists(int id)
        {
            return _context.elevators.Any(e => e.id == id);
        }
    }
}