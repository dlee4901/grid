// using System;

// public class Transition<TStateId>
// {
//     public Transition(TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition) {}
//     public virtual void Init() {}
//     public virtual void OnEnter() {}
// }

// public class Transition : Transition<string>
// {
//     public Transition(string from, string to, Func<Transition<string>, bool> condition) : base(from, to, condition) {}
// }