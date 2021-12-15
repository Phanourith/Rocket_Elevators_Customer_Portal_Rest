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
        public async Task<ActionResult<String>> checkSpecificEmail(String email){
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if(customer == null){
                return NotFound();
            }
            else{
                return customer.ToString();
            }
            
        }
        [HttpGet("{email}/batteries")]
        public async Task<ActionResult> getBatteries(String email){
            string returnJson = "";
            var customer = await _context.customers.Where(e => e.email_of_the_company_contact == email).FirstOrDefaultAsync();
            if(customer == null){
                return NotFound();
            }
            else{
                var buildings = await _context.buildings.Where(b => b.customer_id == customer.id).ToListAsync();
                foreach(var building in buildings){
                    var batteries = await _context.batteries.Where(b => b.building_id == building.id).ToListAsync();
                    foreach(var battery in batteries){
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(battery);
                        returnJson = returnJson + jsonString;
                    }
                }
                return Content(returnJson, "application/json");
            }
        }
        
    }
}