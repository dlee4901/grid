public class State<TStateId, TEvent> : StateBase
{
    public State() {}
}

public class State : State<string, string>
{
	public State() : base() {}
}