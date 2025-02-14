
using System.Runtime.CompilerServices;
using AIWorldLibrary;
using EnvironmentAPI;
using Environments;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentAPI;
public record AgentSensorRegister(int AgentID, WarehouseSensorPermissions SensorPermissions);
public record AgentMovementRegister(int AgentID, WarehouseMovementPermissions MovePermissions);
public record MoveModel(int AgentID, int MoveID);

[ApiController]
public class EnvironmentController : ControllerBase
{
    public ILogger MyLogger { get; set; }
    public WarehouseEnvironment MywarehouseEnvironment;
    public EnvironmentController(ILogger logger, WarehouseEnvironment myWarehouseEnviroment)
    {
        MyLogger = logger;
        MywarehouseEnvironment = myWarehouseEnviroment;
    }
    private bool IsPowerOfTwo(int x)
    {
        return (x & (x - 1)) == 0;
    }

    /*
    //JUST FOR TESTING
    [HttpGet("GetEnvironment")]
    public ActionResult<WarehouseEnvironment> GetEnvironment()
    {
        return Ok(MywarehouseEnvironment);
    }*/


    [HttpPost("LoadEnvironment")]
    public IActionResult LoadOfficeEnvironment([FromBody] WarehouseEnvironment environment)
    {
        if (ModelState.IsValid)
        {
            MywarehouseEnvironment = environment;
            return Ok();
        }

        return BadRequest();
    }
    [HttpPost("RegisterAgent")]
    public ActionResult<int> RegisterAgent()
    {
        int agentID = MywarehouseEnvironment.RegisterAgent();
        return Ok(agentID);
    }

    [HttpPost("RegisterSensorPermission")]
    public IActionResult RegisterAgentSensor([FromBody] AgentSensorRegister model)
    {
        if (!ModelState.IsValid
        || !IsPowerOfTwo(model.SensorPermissions.Value)
        || !MywarehouseEnvironment.SensorCapabilities.ContainsKey(model.SensorPermissions)
        || !MywarehouseEnvironment.AgentIDs.Contains(model.AgentID))
        {
            return BadRequest();
        }

        MywarehouseEnvironment.RegisterAgentSensorPermission(model.AgentID, model.SensorPermissions); //might need to make this a boolean
        return Ok();

    }

    [HttpPost("RegisterMovementPermission")]
    public IActionResult RegisterAgentMovement([FromBody] AgentMovementRegister model)
    {
        if (!ModelState.IsValid
        || !IsPowerOfTwo(model.MovePermissions.Value)
        || !MywarehouseEnvironment.MovementCapabilities.ContainsKey(model.MovePermissions)
        || !MywarehouseEnvironment.AgentIDs.Contains(model.AgentID))
        {
            return BadRequest();
        }

        MywarehouseEnvironment.RegisterAgentMovementPermission(model.AgentID, model.MovePermissions);
        return Ok();


    }

    [HttpGet("GetAgentCurrentMoves")]
    public ActionResult<List<MoveReturn>> GetAgentCurrentMoves(int agentID)
    {
        if (!MywarehouseEnvironment.AgentIDs.Contains(agentID)) return BadRequest();

        var r = MywarehouseEnvironment.GetMoves(agentID);
        return Ok(r);
    }

    [HttpPost("GetSensorData")]
    public ActionResult<ISensorData> GetSensorData([FromBody] AgentSensorRegister model)
    {
        if (!ModelState.IsValid || !MywarehouseEnvironment.AgentIDs.Contains(model.AgentID)
        || !MywarehouseEnvironment.AgentIDToSensorCapabilities.TryGetValue(model.AgentID, out WarehouseSensorPermissions? sensor)
        || !sensor.HasFlag(model.SensorPermissions))
        {
            return BadRequest();
        }
        var result = MywarehouseEnvironment.GetSensorData(model.AgentID, model.SensorPermissions); 
        
        return Ok(result);
    }

    
        [HttpPost("MakeMove")]
        public ActionResult<bool> MakeMove([FromBody] MoveModel moveModel)
        {
            if(ModelState.IsValid)
            {
                return Ok(MywarehouseEnvironment.MakeMove(moveModel.MoveID, moveModel.AgentID));
            }

            return BadRequest();
        }
    

    [HttpGet("GetSensorPermissions")]
    public ActionResult<List<WarehouseSensorPermissions>> GetSensorPermissions(int agentID)
    {
        if (!MywarehouseEnvironment.AgentIDs.Contains(agentID)) return BadRequest();

        return Ok(MywarehouseEnvironment.GetSensorPermissions(agentID));
    }

    [HttpGet("GetMovementPermissions")]
    public ActionResult<WarehouseMovementPermissions> GetMovementPermissions(int agentID)
    {
        if (!MywarehouseEnvironment.AgentIDs.Contains(agentID)) return BadRequest();

        return Ok(MywarehouseEnvironment.GetMovementPermissions(agentID));
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