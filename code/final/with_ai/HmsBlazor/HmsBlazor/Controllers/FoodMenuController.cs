using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_HMS.Entities;
using System.Data;

namespace Project_HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class FoodMenuController(DataAccess dataAccess) : ControllerBase
{ 
    [HttpGet]
    public ActionResult<IEnumerable<FoodPackage>> GetAllFoodPackages()
    {
        string sql = "select * from FoodPackage;";
        DataSet ds1 = dataAccess.ExecuteQuery(sql);
        var foods = ds1.Tables[0].AsEnumerable().Select(dataRow => new FoodPackage
        {
            FId = dataRow.Field<int>("FId").ToString(),
            FName = dataRow.Field<string>("FName"),
            FPackageCost = dataRow.Field<double>("FPackageCost").ToString()
        }).ToList();
        return Ok(foods);
    }

    [HttpGet("{id}")]
    public ActionResult<FoodPackage> GetFoodPackageById(string id)
    {
        string sql = $"select * from FoodPackage where FId = {id};";
        DataSet ds = dataAccess.ExecuteQuery(sql);
        if (ds.Tables[0].Rows.Count == 0)
        {
            return NotFound();
        }
        var dataRow = ds.Tables[0].Rows[0];
        var food = new FoodPackage
        {
            FId = dataRow.Field<int>("FId").ToString(),
            FName = dataRow.Field<string>("FName"),
            FPackageCost = dataRow.Field<double>("FPackageCost").ToString()
        };
        return Ok(food);
    }

    [HttpPost]
    public ActionResult CreateFoodPackage([FromBody] FoodPackage foodPackage)
    {
        string query = $"insert into FoodPackage values ({foodPackage.FId}, '{foodPackage.FName}', {foodPackage.FPackageCost});";
        int count = dataAccess.ExecuteDML(query);
        if (count == 1)
        {
            return CreatedAtAction(nameof(GetFoodPackageById), new { id = foodPackage.FId }, foodPackage);
        }
        return BadRequest("Food Package Insertion Failed.");
    }

    [HttpPut("{id}")]
    public ActionResult UpdateFoodPackage(string id, [FromBody] FoodPackage foodPackage)
    {
        string query = $"update FoodPackage set FName = '{foodPackage.FName}', FPackageCost = {foodPackage.FPackageCost} where FId = {id};";
        int count = dataAccess.ExecuteDML(query);
        if (count == 1)
        {
            return NoContent();
        }
        return BadRequest("Food Package Upgradation Failed.");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteFoodPackage(string id)
    {
        string sql = $"delete from FoodPackage where FId = '{id}';";
        int count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return NoContent();
        }
        return BadRequest("Food Package Deletion Failed.");
    }
}
