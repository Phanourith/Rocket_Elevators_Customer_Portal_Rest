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
    public class ColumnsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ColumnsController(ApplicationContext context)
        {
            _context = context;
        }

        //-----------------------------------------------------
        // ADDED ENDPOINTS:
        //-----------------------------------------------------


        // GET: api/Columns/5/status
        // get specific column's status
        [HttpGet("{id}/status")]
        public async Task<ActionResult<String>> GetColumnStatus(int id)
        {
            var column = await _context.columns.FindAsync(id);

            if (column == null)
            {
                return NotFound();
            }

            return "Column " + column.id + "'s status is: " + column.status;
        }

        // PATCH: api/Columns/5
        // update status (or any single field) of a column using the following format:
            // [{"op": "replace", "path": "/status", "value": "Offline"}]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchColumnStatus(int id, [FromBody]JsonPatchDocument<Column> columnPatch)
        {
            var column = await _context.columns.FindAsync(id);
            columnPatch.ApplyTo(column);

            await _context.SaveChangesAsync();

            return Content("Successfully updated column " + column.id);
        }

        [HttpGet("battery_id={battery_id}")]
        public async Task<ActionResult> getBatteries(int battery_id)
        {

            List<Column> ColumnList = new List<Column>();
            ColumnList = await _context.columns.Where(b => b.battery_id == battery_id).ToListAsync();

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

        private bool ColumnExists(int id)
        {
            return _context.columns.Any(e => e.id == id);
        }
    }
}