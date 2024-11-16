using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Project_HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicePackageController(DataAccess dataAccess) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ServicePackage>> GetServicePackages()
    {
        string sql = "select * from ServicePackage;";
        DataSet ds = dataAccess.ExecuteQuery(sql);
        var services = ds.Tables[0].AsEnumerable().Select(dataRow => new ServicePackage
        {
            SId = dataRow.Field<int>("SId"),
            SName = dataRow.Field<string>("SName"),
            SPackageCost = dataRow.Field<double>("SPackageCost")
        }).ToList();
        return Ok(services);
    }

    [HttpGet("{id}")]
    public ActionResult<ServicePackage> GetServicePackage(int id)
    {
        string sql = $"select * from ServicePackage where SId = {id};";
        DataSet ds = dataAccess.ExecuteQuery(sql);
        if (ds.Tables[0].Rows.Count == 0)
        {
            return NotFound();
        }
        var dataRow = ds.Tables[0].Rows[0];
        var service = new ServicePackage
        {
            SId = dataRow.Field<int>("SId"),
            SName = dataRow.Field<string>("SName"),
            SPackageCost = dataRow.Field<double>("SPackageCost")
        };
        return Ok(service);
    }

    [HttpPost]
    public ActionResult<ServicePackage> CreateServicePackage(ServicePackage servicePackage)
    {
        string sql = $"insert into ServicePackage values ({servicePackage.SId}, '{servicePackage.SName}', {servicePackage.SPackageCost});";
        int count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return CreatedAtAction(nameof(GetServicePackage), new { id = servicePackage.SId }, servicePackage);
        }
        return BadRequest("Service Package Insertion Failed.");
    }

    [HttpPut("{id}")]
    public ActionResult UpdateServicePackage(int id, ServicePackage servicePackage)
    {
        string sql = $"update ServicePackage set SName = '{servicePackage.SName}', SPackageCost = {servicePackage.SPackageCost} where SId = {id};";
        int count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return NoContent();
        }
        return BadRequest("Service Package Upgradation Failed.");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteServicePackage(int id)
    {
        string sql = $"delete from ServicePackage where SId = {id};";
        int count = dataAccess.ExecuteDML(sql);
        if (count == 1)
        {
            return NoContent();
        }
        return BadRequest("Service Package Deletion Failed.");
    }
}
