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
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public CustomersController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet("{email}/check")]
        public async Task<ActionResult<String>> checkSpecificEmail(String email)
        {
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return customer.ToString();
            }

        }


        [HttpGet("{email}/buildings")]
        public async Task<ActionResult> getBuildings(String email)
        {
            string returnJson = "";
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var buildings = await _context.buildings.Where(b => b.customer_id == customer.id).ToListAsync();
                foreach (var building in buildings)
                {   

                     var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(building);
                        if (returnJson == "")
                        {
                            returnJson = jsonString;
                        }
                        else
                        {
                            returnJson = returnJson + ", " + jsonString;
                        }

                }
                returnJson = "[" + returnJson + "]";
                return Content(returnJson, "application/json");
            }
        }

        [HttpGet("{email}/batteries")]
        public async Task<ActionResult> getBatteries(String email)
        {
            string returnJson = "";
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var buildings = await _context.buildings.Where(b => b.customer_id == customer.id).ToListAsync();
                foreach (var building in buildings)
                {
                    var batteries = await _context.batteries.Where(b => b.building_id == building.id).ToListAsync();
                    foreach (var battery in batteries)
                    {
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(battery);
                        if (returnJson == "")
                        {
                            returnJson = jsonString;
                        }
                        else
                        {
                            returnJson = returnJson + ", " + jsonString;

                        }

                    }

                }
                returnJson = "[" + returnJson + "]";
                return Content(returnJson, "application/json");
            }
        }

        [HttpGet("{email}/info")]
        public async Task<ActionResult> getCustomer(String email)
        {
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(customer);
                return Content(jsonString, "application/json");
            }
        }

        [HttpPatch("{email}")]
        public async Task<IActionResult> PatchProfile(string email, [FromBody] JsonPatchDocument<Customer> customerPatch)
        {
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();

            customerPatch.ApplyTo(customer);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{email}/columns")]
        public async Task<ActionResult> getColumns(String email)
        {
            string returnJson = "";
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var buildings = await _context.buildings.Where(b => b.customer_id == customer.id).ToListAsync();
                foreach (var building in buildings)
                {
                    var batteries = await _context.batteries.Where(b => b.building_id == building.id).ToListAsync();
                    foreach (var battery in batteries)
                    {
                        var columns = await _context.columns.Where(c => c.battery_id == battery.id).ToListAsync();
                        foreach (var column in columns)
                        {
                            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(column);
                            if (returnJson == "")
                            {
                                returnJson = jsonString;
                            }
                            else
                            {
                                returnJson = returnJson + ", " + jsonString;

                            }
                        }
                    }
                }
                returnJson = "[" + returnJson + "]";
                return Content(returnJson, "application/json");
            }
        }
        [HttpGet("{email}/elevators")]
        public async Task<ActionResult> getElevators(String email)
        {
            string returnJson = "";
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var buildings = await _context.buildings.Where(b => b.customer_id == customer.id).ToListAsync();
                foreach (var building in buildings)
                {
                    var batteries = await _context.batteries.Where(b => b.building_id == building.id).ToListAsync();
                    foreach (var battery in batteries)
                    {
                        var columns = await _context.columns.Where(c => c.battery_id == battery.id).ToListAsync();
                        foreach (var column in columns)
                        {
                            var elevators = await _context.elevators.Where(e => e.column_id == column.id).ToListAsync();
                            foreach (var elevator in elevators)
                            {
                                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(elevator);
                                if (returnJson == "")
                                {
                                    returnJson = jsonString;
                                }
                                else
                                {
                                    returnJson = returnJson + ", " + jsonString;

                                }
                            }
                        }
                    }
                }
                returnJson = "[" + returnJson + "]";
                return Content(returnJson, "application/json");
            }
        }


        [HttpGet("email={email}&elevator_id={elevator_id}")]
        public async Task<IActionResult> GetElevatorInvervention(String email, int elevator_id){
    
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            var elevator = await _context.elevators.Where(e => e.id == elevator_id).FirstOrDefaultAsync();
            var column = await _context.columns.Where(c => c.id == elevator.column_id).FirstOrDefaultAsync();
            var battery = await _context.batteries.Where(b => b.id == column.battery_id).FirstOrDefaultAsync();
            var building = await _context.buildings.Where(b => b.id == battery.building_id).FirstOrDefaultAsync();
            var newIntervention = new Intervention{
                author = customer.id,
                building_id = building.id,
                battery_id = battery.id,
                column_id = column.id,
                elevator_id = elevator.id,
                customer_id = customer.id,
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(newIntervention);
            return Content(jsonString, "application/json");
        }

        [HttpGet("email={email}&column_id={column_id}")]
        public async Task<IActionResult> GetColumnIntervention(String email, int column_id){
    
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            
            var column = await _context.columns.Where(c => c.id == column_id).FirstOrDefaultAsync();
            var battery = await _context.batteries.Where(b => b.id == column.battery_id).FirstOrDefaultAsync();
            var building = await _context.buildings.Where(b => b.id == battery.building_id).FirstOrDefaultAsync();
            var newIntervention = new Intervention{
                author = customer.id,
                building_id = building.id,
                battery_id = battery.id,
                column_id = column.id,
                customer_id = customer.id,
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(newIntervention);
            return Content(jsonString, "application/json");
        }

        [HttpGet("email={email}&battery_id={battery_id}")]
        public async Task<IActionResult> GetBatteryIntervention(String email, int battery_id){
    
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            
            var battery = await _context.batteries.Where(b => b.id == battery_id).FirstOrDefaultAsync();
            var building = await _context.buildings.Where(b => b.id == battery.building_id).FirstOrDefaultAsync();
            var newIntervention = new Intervention{
                author = customer.id,
                building_id = building.id,
                battery_id = battery.id,
                customer_id = customer.id,
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(newIntervention);
            return Content(jsonString, "application/json");
        }

       
    }
}