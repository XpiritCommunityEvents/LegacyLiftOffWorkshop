using Microsoft.AspNetCore.Mvc;
using Project_HMS.Entities;
using System.Data;

namespace Project_HMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(DataAccess dataAccess) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        try
        {
            string sql = "select * from Booking;";
            DataSet ds1 = await Task.Run(() => dataAccess.ExecuteQuery(sql));
            var customers = ds1.Tables[0].AsEnumerable().Select(dataRow => new Customer
            {
                CName = dataRow.Field<string>("CName"),
                CPhone = dataRow.Field<string>("CPhone"),
                CAdd = dataRow.Field<string>("CAdd"),
                CNID = dataRow.Field<string>("CNID")
            }).ToList();
            return Ok(customers);
        }
        catch (Exception exc)
        {
            return StatusCode(500, "Internal server error: " + exc.Message);
        }
    }

    [HttpPut("{cnid}")]
    public async Task<IActionResult> UpdateCustomer(string cnid, [FromBody] Customer customer)
    {
        if (customer == null || string.IsNullOrEmpty(customer.CName) || string.IsNullOrEmpty(customer.CAdd) || string.IsNullOrEmpty(customer.CNID) || string.IsNullOrEmpty(customer.CPhone))
        {
            return BadRequest("To Update please fill all the information.");
        }

        try
        {
            string query = $"update Booking set CName = '{customer.CName}', CPhone = '{customer.CPhone}', CAdd = '{customer.CAdd}', CNID = '{customer.CNID}' where CNID = {cnid};";
            int count = await Task.Run(() => dataAccess.ExecuteDML(query));
            if (count == 1)
            {
                return Ok("Customer Info Updated Successfully.");
            }
            else
            {
                return StatusCode(500, "Customer Upgradation Failed.");
            }
        }
        catch (Exception exc)
        {
            return StatusCode(500, "Internal server error: " + exc.Message);
        }
    }
}