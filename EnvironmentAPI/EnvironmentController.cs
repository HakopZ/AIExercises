
using AIWorldLibrary;
using EnvironmentAPI;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class EnvironmentController : ControllerBase
{
    public ILogger MyLogger { get; set; }
    public WarehouseEnvironment warehouseEnvironment { get; set; } = new WarehouseEnvironment();
    public EnvironmentController(ILogger logger)
    {
        MyLogger = logger;
        DefaultGraph();
    }
    private void DefaultGraph()
    {
        warehouseEnvironment.Clear();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i != 4 || j != 4)
                {
                    warehouseEnvironment.TryAddVertex(new(i, j), SpotState.Empty);
                }
                else
                {
                    warehouseEnvironment.TryAddVertex(new(i, j), SpotState.Item);
                }
            }
        }
    }

    [HttpPost("LoadEnvironment")]
    public IActionResult LoadOfficeEnvironment([FromBody] WarehouseEnvironment environment)
    {
        if (ModelState.IsValid)
        {
            warehouseEnvironment = environment;
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpGet("MakeMove")]
    public ActionResult<EnvironmentState> MakeMove([FromBody] EnvironmentState state)
    {
        if(ModelState.IsValid)
        {
            return Ok(warehouseEnvironment.MakeMove(state));
        }

        return BadRequest();
    }

    [HttpGet("GetSuccessors")]
    public ActionResult<List<Succesor<EnvironmentState>>> GenerateSuccesors([FromBody] EnvironmentState state)
    {
        if(ModelState.IsValid)
        {
            return Ok(warehouseEnvironment.GetSuccesors(state));
        }

        return BadRequest();
    }

    
}