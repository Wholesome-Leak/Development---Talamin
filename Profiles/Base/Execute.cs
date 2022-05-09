using System;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;

namespace WholesomeDungeons.Profiles.Base {
    internal class Execute : Step {
        private readonly Action _action;
        private readonly Func<bool> _checkCompletion;

        public Execute(Action action, Func<bool> checkCompletion = null, string stepName = "Execute") : base(stepName) {
            _action = action;
            _checkCompletion = checkCompletion;
        }

        public override bool Pulse() {
            _action();
            IsCompleted = _checkCompletion?.Invoke() ?? true;
            return IsCompleted;
        }
    }
}