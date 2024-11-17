using Microsoft.AspNetCore.Mvc;
using Project_HMS.Entities;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Project_HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class RoomController(DataAccess dataAccess) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms()
    {
        var sql = "SELECT * FROM Room;";
        var ds = dataAccess.ExecuteQuery(sql);
        var rooms = ds.Tables[0].AsEnumerable().Select(row => new Room
        {
            RId = row.Field<int>("RId"),
            Category = row.Field<string>("Category"),
            IsBooked = row.Field<string>("IsBooked"),
            RoomCost = row.Field<double>("RoomCost")
        }).ToList();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public ActionResult<Room> GetRoom(int id)
    {
        try
        {
            using (var connection = dataAccess.Sqlcon)
            {
                var sql = "SELECT * FROM Room WHERE RId = @RId;";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RId", id);

                var adapter = new SqlDataAdapter(command);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return NotFound();
                }

                var row = ds.Tables[0].Rows[0];
                var room = new Room
                {
                    RId = row.Field<int>("RId"),
                    Category = row.Field<string>("Category"),
                    IsBooked = row.Field<string>("IsBooked"),
                    RoomCost = row.Field<double>("RoomCost")
                };

                return Ok(room);
            }
        }
        catch (Exception exc)
        {
            return StatusCode(500, "Internal server error: " + exc.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles="Role.Owner")]
    public ActionResult AddRoom([FromBody] Room room)
    {
        var sql = $"insert into Room values ({room.RId}, '{room.Category}', '{room.IsBooked}', {room.RoomCost}, NULL);";
        var count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return CreatedAtAction(nameof(GetRoom), new { id = room.RId }, room);
        }
        return BadRequest();
    }

    [HttpPut("{id}")]
    [Authorize(Roles="Role.Owner")]
    public ActionResult UpdateRoom(int id, [FromBody] Room room)
    {
        var sql = $"update Room set Category = '{room.Category}', IsBooked = '{room.IsBooked}', RoomCost = {room.RoomCost} where RId = {id};";
        var count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles="Role.Owner")]
    public ActionResult DeleteRoom(int id)
    {
        var sql = $"delete from Room where RId = {id};";
        var count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Room>> SearchRooms(string category)
    {
        var sql = $"select * from Room where Category like '{category}%';";
        var ds = dataAccess.ExecuteQuery(sql);
        var rooms = ds.Tables[0].AsEnumerable().Select(row => new Room
        {
            RId = row.Field<int>("RId"),
            Category = row.Field<string>("Category"),
            IsBooked = row.Field<string>("IsBooked"),
            RoomCost = row.Field<double>("RoomCost")
        }).ToList();
        return Ok(rooms);
    }
}