using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_HMS.Entities;

namespace Project_HMS.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ReservationController(DataAccess dataAccess) : ControllerBase
{
    [HttpPost("confirm-reservation")]
    public ActionResult ConfirmReservation([FromBody] Booking booking)
    {
        if (string.IsNullOrEmpty(booking.CheckIn) ||
            string.IsNullOrEmpty(booking.CheckOut) ||
            string.IsNullOrEmpty(booking.CName) ||
            string.IsNullOrEmpty(booking.Address) ||
            string.IsNullOrEmpty(booking.CPhone) ||
            string.IsNullOrEmpty(booking.NID))
        {
            return BadRequest("For reservation please fill all the information.");
        }

        var query = $"insert into Booking values ({booking.BId}, {booking.RId}, '{booking.CheckIn}', '{booking.CheckOut}', {booking.FId}, {booking.SId}, '{booking.CName}', '{booking.Address}', '{booking.CPhone}', '{booking.NID}', {booking.Total}, {booking.Advance}, {booking.Remaining});";
        var count = dataAccess.ExecuteDML(query);
        if (count == 1)
        {
            var update = $"UPDATE Room SET IsBooked = 'Yes', BId = {booking.BId} WHERE RId = {booking.RId};";
            var count1 = dataAccess.ExecuteDML(update);
            if (count1 == 1)
            {
                return Ok("Reservation confirmed and room status updated.");
            }
            else
            {
                return StatusCode(500, "Data upgradation failed in room table.");
            }
        }
        else
        {
            return StatusCode(500, "Data insertion failed in booking table.");
        }
    }

    [HttpPost("calculate-total")]
    public ActionResult<double> CalculateTotal([FromBody] CalculationRequest request)
    {
        double total = 0;
        var D = Convert.ToDouble(request.Days);

        var sql1 = $"select RoomCost from room where RId = {request.RId};";
        var dt1 = dataAccess.ExecuteQueryTable(sql1);
        var RCost = Convert.ToDouble(dt1.Rows[0]["RoomCost"]);
        total += RCost * D;

        var sql2 = $"select FPackageCost from FoodPackage where FId = {request.FId};";
        var dt2 = dataAccess.ExecuteQueryTable(sql2);
        var FCost = Convert.ToDouble(dt2.Rows[0]["FPackageCost"]);
        total += FCost * D;

        var sql3 = $"select SPackageCost from ServicePackage where SId = {request.SId};";
        var dt3 = dataAccess.ExecuteQueryTable(sql3);
        var SCost = Convert.ToDouble(dt3.Rows[0]["SPackageCost"]);
        total += SCost;

        return Ok(total);
    }

    [HttpPost("calculate-remaining")]
    public ActionResult<double> CalculateRemaining([FromBody] RemainingRequest request)
    {
        var T = request.Total;
        var A = request.Advance;
        var remain = T - A;
        return Ok(remain);
    }
}