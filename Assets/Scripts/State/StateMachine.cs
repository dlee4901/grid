// using System.Collections.Generic;

// public class StateMachine<TOwnId, TStateId, TEvent> : StateBase<TOwnId>, IStateMachine
// {
//     private class StateBundle
//     {
//         public StateBase<TStateId> state;
//         public List<TransitionBase<TStateId>> transitions;
//         public Dictionary<TEvent, List<TransitionBase<TStateId>>> triggerToTransitions;

//         public void AddTransition(TransitionBase<TStateId> transition)
//         {
//             transitions ??= new List<TransitionBase<TStateId>>();
//             transitions.Add(transition);
//         }

//         public void AddTriggerTransition(TEvent trigger, TransitionBase<TStateId> transition)
//         {
//             triggerToTransitions ??= new Dictionary<TEvent, List<TransitionBase<TStateId>>>();
//             List<TransitionBase<TStateId>> transitionsOfTrigger;
//             if (!triggerToTransitions.TryGetValue(trigger, out transitionsOfTrigger))
//             {
//                 transitionsOfTrigger = new List<TransitionBase<TStateId>>();
// 				triggerToTransitions.Add(trigger, transitionsOfTrigger);
//             }
//             transitionsOfTrigger.Add(transition);
//         }
//     }

//     public IStateMachine ParentFsm => Fsm;
//     private bool IsRootFSM => Fsm == null;

//     private Dictionary<TStateId, StateBundle> stateBundles = new();
//     private (TStateId state, bool hasState) startState = (default, false);

//     public StateMachine() {}

//     private StateBundle GetOrCreateStateBundle(TStateId name)
//     {
//         StateBundle bundle;

//         if (!stateBundles.TryGetValue(name, out bundle))
//         {
//             bundle = new StateBundle();
//             stateBundles.Add(name, bundle);
//         }

//         return bundle;
//     }

//     public void SetStartState(TStateId name)
//     {
//         startState = (name, true);
//     }

//     public void AddState(TStateId name, StateBase<TStateId> state)
//     {
//         state.Fsm = this;
//         state.Name = name;
//         state.Init();

//         StateBundle bundle = GetOrCreateStateBundle(name);
// 		bundle.state = state;

//         if (stateBundles.Count == 1 && !startState.hasState)
//         {
//             SetStartState(name);
//         }
//     }

//     private void InitTransition(TransitionBase<TStateId> transition)
//     {
//         transition.Fsm = this;
//         transition.Init();
//     }

//     public void AddTransition(TransitionBase<TStateId> transition)
//     {
//         InitTransition(transition);
//         StateBundle bundle = GetOrCreateStateBundle(transition.From);
//         bundle.AddTransition(transition);
//     }

//     public void AddTwoWayTransition()
//     {

//     }
// }

// public class StateMachine<TStateId, TEvent> : StateMachine<TStateId, TStateId, TEvent>
// {
//     public StateMachine() : base() {}
// }

// public class StateMachine<TStateId> : StateMachine<TStateId, TStateId, string>
// {
//     public StateMachine() : base() {}
// }

// public class StateMachine : StateMachine<string, string, string>
// {
//     public StateMachine() : base() {}
// }