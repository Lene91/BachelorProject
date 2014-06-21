using ExperimentTemplate;
using Logging;

namespace BachelorProject
{
    class TrialEndScreen : Trial
    {
        private static readonly EndScreen screen = new EndScreen();
        //private static readonly Logger Log = Logger.GetLogger(typeof(TrialEndScreen));

        public TrialEndScreen()
        {
            Name = "TrialEndScreen";
            TrackingRequired = false;
            Screen = screen;
        }

        protected override void OnShowing()
        {
            Tracker.SendMessage("SHOW ENDSCREEN");
        }

        protected override void OnHidden()
        {
            Tracker.SendMessage("STOP ENDSCREEN");
        }
    }
}
