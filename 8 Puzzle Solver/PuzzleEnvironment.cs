using AIWorldLibrary;

public class PuzzleEnvironment : AIEnvironment<int>
{
    public override bool isStatic => true;

    public override bool isDeterministic => true;

    public override IVertex<int> GoalState => 

    public override int FindLookAheadAmount(int state)
    {
        return -1;
    }

    public override List<Succesor<int>> GetSuccesors(int state)
    {
        throw new NotImplementedException();
    }

    public override int MakeMove(int state)
    {
        throw new NotImplementedException();
    }
}