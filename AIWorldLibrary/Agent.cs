using System.Data.SqlTypes;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;
using AIWorldLibrary;

namespace AIWorldLibrary;


public abstract class Agent(string environmentAPIUrl)
{
    protected HttpClient Client = new HttpClient();
    protected readonly string EnvironmentAPIUrl = environmentAPIUrl;
    public int ID;
    public bool SensorRegistered = false;
    public bool MovementRegistered = false;
    public bool MakingMove = false;

    public async Task Register()
    {
        var result = await Client.PostAsJsonAsync(EnvironmentAPIUrl + "/RegisterAgent", this); //not sure if this works 

        if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception("Couldn't register");

        ID = JsonSerializer.Deserialize<int>(await result.Content.ReadAsStringAsync());
    }

    public virtual Task RegisterSensors()
    {
        SensorRegistered = true;
        return Task.CompletedTask;
    }
    public virtual Task RegisterMovements()
    {
        MovementRegistered = true;
        return Task.CompletedTask;
    }
    public abstract Task Sense();
    public abstract Task<int> SelectMoveID();
    public virtual Task<bool> MakeMove()
    {
        MakingMove = false;
        return Task.FromResult(true);
    }
    protected async Task<List<MoveReturn>?> GetAgentCurrentMoves()
    {

        var result = await Client.GetAsync(EnvironmentAPIUrl + $"/GetAgentCurrentMoves?agentID={ID}");
        result.EnsureSuccessStatusCode();
        var r = await result.Content.ReadFromJsonAsync<List<MoveReturn>>();
        return r;
    }
    protected async Task<bool> RegisterSensorPermission(BetterEnum permission)
    {
        var model = new
        {
            AgentID = ID,
            SensorPermissions = permission
        };
        var result = await Client.PostAsJsonAsync(EnvironmentAPIUrl + "/RegisterSensorPermission", model);

        if (result.StatusCode != System.Net.HttpStatusCode.OK) return false;

        return true;
    }
    protected async Task<bool> RegisterMovementPermission(BetterEnum permission)
    {
        var model = new
        {
            AgentID = ID,
            MovePermissions = permission
        };
        var result = await Client.PostAsJsonAsync(EnvironmentAPIUrl + "/RegisterMovementPermission", model);

        if (result.StatusCode != System.Net.HttpStatusCode.OK) return false;

        return true;
    }

    /*
    public Queue<AgentState<TEnvironmentState, TCost>> pathToGo = [];
    public Dictionary<TEnvironmentState, TCost> Visited { get; set; } = [];
    public abstract TCost GetCost(TEnvironmentState state, TEnvironmentState state2);
    public abstract void AddSuccessors(List<Succesor<TEnvironmentState>> succesors, TEnvironmentState GoalState);
    public abstract AgentState<TEnvironmentState, TCost> SelectState(int lookAhead);*/
}