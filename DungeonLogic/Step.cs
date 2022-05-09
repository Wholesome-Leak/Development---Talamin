using System;

namespace WholesomeDungeons.DungeonLogic {
    internal abstract class Step {
        public bool IsCompleted;
        public virtual bool OverrideNeedToRun => false;

        protected Step(string stepName = "Unnamed") {
            Name = stepName;
        }

        public string Name { get; }

        public virtual bool Pulse() => throw new NotImplementedException();
    }
}