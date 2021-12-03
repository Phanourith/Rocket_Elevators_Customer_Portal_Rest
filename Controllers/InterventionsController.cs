using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocket_Elevators_REST_API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Rocket_Elevators_REST_API.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionsController : ControllerBase{
        private readonly ApplicationContext _context;
        public InterventionsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<Intervention>>> getAllInterventions(){
            return await _context.interventions.ToListAsync();
        }
        [HttpGet("pending")]
        public async Task<ActionResult<List<Intervention>>> getPendingInterventions(){
            var interventions = await _context.interventions.Where(i => i.status == "Pending").ToListAsync();
            if (interventions == null)
            {
                return NotFound();
            }
            return interventions;
        }
        [HttpGet("inprogress")]
        public async Task<ActionResult<List<Intervention>>> getInProgressInterventions(){
            var interventions = await _context.interventions.Where(i => i.status == "InProgress").ToListAsync();
            if (interventions == null)
            {
                return NotFound();
            }
            return interventions;
        }
        [HttpGet("completed")]
        public async Task<ActionResult<List<Intervention>>> getCompletedInterventions(){
            var interventions = await _context.interventions.Where(i => i.status == "Completed").ToListAsync();
            if (interventions == null)
            {
                return NotFound();
            }
            return interventions;
        }
        [HttpPut("inprogress/{id}")]
        public async Task<IActionResult> UpdateStatusInProgress(int id){
            var interventionToUpdate = await _context.interventions.FindAsync(id);
            if(interventionToUpdate == null){
                return NotFound($"Intervention with Id = {id} not found");
            }
            interventionToUpdate.status = "InProgress";
            interventionToUpdate.start_intervention = DateTime.Now;
            _context.interventions.Update(interventionToUpdate);
            _context.SaveChanges();
            return Content("Successfully updated status to In Progress " + interventionToUpdate.start_intervention);
        }
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> UpdateStatusComplete(int id){
            var interventionToUpdate = await _context.interventions.FindAsync(id);
            if(interventionToUpdate == null){
                return NotFound($"Intervention with Id = {id} not found");
            }
            interventionToUpdate.status = "Completed";
            interventionToUpdate.result = "Completed";
            interventionToUpdate.end_intervention = DateTime.Now;
            _context.interventions.Update(interventionToUpdate);
            _context.SaveChanges();
            return Content("Successfully updated status to In Progress " + interventionToUpdate.end_intervention);
        }
        [HttpPut("pending/{id}")]
        public async Task<IActionResult> UpdateStatusPending(int id){
            var interventionToUpdate = await _context.interventions.FindAsync(id);
            if(interventionToUpdate == null){
                return NotFound($"Intervention with Id = {id} not found");
            }
            interventionToUpdate.status = "Pending";
            interventionToUpdate.result = "Incomplete";
            _context.interventions.Update(interventionToUpdate);
            _context.SaveChanges();
            return Content("Successfully updated status to Pending ");
        }

    }

}