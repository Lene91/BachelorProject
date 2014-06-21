using ExperimentTemplate;
using Logging;

namespace BachelorProject
{
    class TrialStartScreen : Trial
    {
        private static readonly StartScreen screen = new StartScreen();
        //private static readonly Logger Log = Logger.GetLogger(typeof(TrialStartScreen));

        public TrialStartScreen()
        {
            Name = "TrialStartScreen";
            TrackingRequired = false;
            Screen = screen;
        }

        protected override void OnShowing()
        {
            Tracker.SendMessage("SHOW STARTSCREEN");
        }

        protected override void OnHidden()
        {
            Tracker.SendMessage("STOP STARTSCREEN");
        }
    }
}
