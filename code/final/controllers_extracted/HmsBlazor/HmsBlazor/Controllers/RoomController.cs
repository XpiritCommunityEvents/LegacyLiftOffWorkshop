using Microsoft.AspNetCore.Mvc;
using Project_HMS.Entities;
using System.Data;

namespace Project_HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController(DataAccess dataAccess) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms()
    {
        var sql = "select * from Room;";
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
        var sql = $"select * from Room where RId = {id};";
        var ds = dataAccess.ExecuteQuery(sql);
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

    [HttpPost]
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
}
