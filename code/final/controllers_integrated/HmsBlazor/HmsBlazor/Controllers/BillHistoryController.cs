using Microsoft.AspNetCore.Mvc;
using Project_HMS.Entities;
using System.Data;

namespace Project_HMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class BillHistoryController(DataAccess dataAccess) : ControllerBase
{
    [HttpGet("bookings")]
    public ActionResult<List<Booking>> GetBookings()
    {
        try
        {
            string sql = "SELECT * FROM Booking;";
            DataSet ds1 = dataAccess.ExecuteQuery(sql);
            var bookings = ds1.Tables[0].AsEnumerable().Select(dataRow => new Booking
            {
                BId = dataRow.Field<int>("BId"),
                RId = dataRow.Field<int>("RId"),
                CName = dataRow.Field<string>("CName"),
                CPhone = dataRow.Field<string>("CPhone"),
                CheckIn = dataRow.Field<DateTime>("CheckIn").ToString("yyyy-MM-dd"),
                CheckOut = dataRow.Field<DateTime>("CheckOut").ToString("yyyy-MM-dd"),
                Advance = dataRow.Field<double>("Advance"),
                Remaining = dataRow.Field<double>("Remaining"),
                Total = dataRow.Field<double>("Total")
            }).ToList();
            return Ok(bookings);
        }
        catch (Exception exc)
        {
            return StatusCode(500, "Internal server error: " + exc.Message);
        }
    }

    [HttpPost("update-booking")]
    public ActionResult UpdateBooking(int bId, int rId, double total)
    {
        try
        {
            string updateRoom = $"UPDATE Room SET IsBooked = 'No' WHERE RId = {rId}";
            int count1 = dataAccess.ExecuteDML(updateRoom);
            if (count1 != 1)
            {
                return StatusCode(500, "Data Upgradation Failed In Room Table.");
            }

            string updateBooking = $"UPDATE Booking SET Advance = {total}, Remaining = 0 WHERE BId = {bId}";
            int count2 = dataAccess.ExecuteDML(updateBooking);
            if (count2 != 1)
            {
                return StatusCode(500, "Data Upgradation Failed In Booking Table.");
            }

            return Ok("Data Updated Successfully.");
        }
        catch (Exception exc)
        {
            return StatusCode(500, "Internal server error: " + exc.Message);
        }
    }
}