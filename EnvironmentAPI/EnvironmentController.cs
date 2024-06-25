
using AIWorldLibrary;
using EnvironmentAPI;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentAPI;
public record AgentSensorRegister(int AgentID, SensorPermissions SensorPermissions);
public record AgentMovementRegister(int AgentID, MovementPermissions MovePermissions);
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
        warehouseEnvironment.ClearEnvironment();
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

    //JUST FOR TESTING
    [HttpGet("GetEnvironment")]
    public ActionResult<WarehouseEnvironment> GetEnvironment()
    {
        return Ok(warehouseEnvironment);
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
    [HttpPost("RegisterAgent")]
    public ActionResult<int> RegisterAgent([FromBody] Agent agent)
    {
        if (ModelState.IsValid)
        {
            int agentID = warehouseEnvironment.RegisterAgent(agent);
            return Ok(agentID);
        }

        return BadRequest();
    }

    [HttpPost("RegisterSensorPermission")]
    public IActionResult RegisterAgentSensors([FromBody] AgentSensorRegister model)
    {
        if(ModelState.IsValid)
        {
            warehouseEnvironment.RegisterAgentSensorPermissions(model.AgentID, model.SensorPermissions); //might need to make this a boolean
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost("RegisterMovementPermission")]
    public IActionResult RegisterAgentMovement([FromBody] AgentMovementRegister model)
    {
        if (ModelState.IsValid)
        {
            warehouseEnvironment.RegisterAgentMovementPermissions(model.AgentID, model.MovePermissions);
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


    [HttpGet("GetSensorPermissions")]
    public ActionResult<SensorPermissions> GetSensorPermissions(int agentID)
    {
        if (!warehouseEnvironment.AgentToID.ContainsValue(agentID)) return BadRequest();
        
        return Ok(warehouseEnvironment.AgentIDToSensorCapabilities[agentID]);
    }

    [HttpGet("GetMovementPermissions")]
    public ActionResult<MovementPermissions> GetMovementPermissions(int agentID)
    {
        if (!warehouseEnvironment.AgentToID.ContainsValue(agentID)) return BadRequest();

        return Ok(warehouseEnvironment.AgentIDToMovementCapabilities[agentID]);
    }

/*
    [HttpGet("GetSuccessors")]
    public ActionResult<List<Succesor<EnvironmentState>>> GenerateSuccesors([FromBody] EnvironmentState state)
    {
        if(ModelState.IsValid)
        {
            //return Ok(warehouseEnvironment.GetSuccesors(state));
        }

        return BadRequest();
    }*/


}