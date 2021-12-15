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
    }
}