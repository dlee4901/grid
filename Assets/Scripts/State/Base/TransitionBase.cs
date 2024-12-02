// public class TransitionBase<TStateId>
// {
//     public TStateId From;
// 	public TStateId To;
//     public IStateMachine Fsm;

//     public TransitionBase(TStateId from, TStateId to) {From = from; To = to;}
//     public virtual void Init() {}
//     public virtual void OnEnter() {}
// }

// public class TransitionBase : TransitionBase<string>
// {
//     public TransitionBase(string from, string to) : base(from, to) {}
// }