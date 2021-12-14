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
        
    }
}