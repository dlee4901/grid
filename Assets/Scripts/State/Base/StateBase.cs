public class StateBase<TStateId>
{
    public TStateId Name;
    public IStateMachine Fsm;

    public StateBase() {}
    public virtual void Init() {}
    public virtual void OnEnter() {}
    public virtual void OnLogic() {}
    public virtual void OnExit() {}
}

public class StateBase : StateBase<string>
{
    public StateBase() : base() {}
}