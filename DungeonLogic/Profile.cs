using robotManager.Helpful;
using WholesomeDungeons.Helper;
using WholesomeDungeons.States;

namespace WholesomeDungeons.DungeonLogic {
    internal abstract class Profile {
        private int _currentStep;
        private int _totalSteps;
        protected Step[] Steps;

        protected Profile(string profileName = "Unnamed") {
            Name = profileName;
        }

        public string Name { get; }
        public string CurrentState { get; private set; } = "Idle profile";

        public virtual bool OverrideNeedToRun => GetCurrentStep()?.OverrideNeedToRun ?? false;

        protected virtual Step[] GetSteps() => new Step[0];

        public Step GetCurrentStep() => _currentStep < _totalSteps ? Steps?[_currentStep] : null;

        public bool IsFinished() => _currentStep >= _totalSteps;


        public void Load() {
            LogicRunner.CheckUpdate(this);
        }

        public void Reset() {
            Steps = GetSteps();
            _currentStep = 0;
            _totalSteps = Steps.Length;
            Logger.Log("DungeonLogic.Profile: Reset all Dungeon Steps");
        }

        protected virtual void UpdateSteps() 
        {
            if (Resurrection.ResetStepsBecauseRes == true)
            {
                for (int i = 0; i < Steps.Length; i++)
                {
                    Steps[i].IsCompleted = false;
                }
                _currentStep = 0;
                Helpers.StopAllMove(); //First Stop all old  Movement
                Reset(); //Reset all  Steps inside the Profile
            }
            Resurrection.ResetStepsBecauseRes = false; // Go to first gathering point again
        }

        protected virtual void ResetSteps() 
        {
            if (Bot.WholesomeDungeonsSettings.CurrentSetting.BotStopped == true)
            {
                if(!Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                {
                    for (int i = 0; i < Steps.Length; i++)
                    {
                        Steps[i].IsCompleted = false;
                    }
                    _currentStep = 0;
                }
                Bot.WholesomeDungeonsSettings.CurrentSetting.BotStopped = false;
            }
        }

        public bool Pulse() {
            if (IsFinished()) return true;

            UpdateSteps();
            ResetSteps();

            Step step = Steps[_currentStep];

            if (!step.IsCompleted) {
                CurrentState = $"Executing step {step.Name} ({_currentStep + 1}/{_totalSteps}).";
                if (step.Pulse()) _currentStep++;
            } else {
                Logger.Log($"[LogicRunner] Skipping step {step.Name} ({_currentStep + 1}/{_totalSteps}).");
                _currentStep++;
            }

            return IsFinished();
        }
    }
}