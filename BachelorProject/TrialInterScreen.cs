using ExperimentTemplate;
using Logging;
using Eyetracker.EyeTribe;

namespace BachelorProject
{
    class TrialInterScreen : Trial
    {
        private static readonly InterScreen screen = new InterScreen();
        //private static readonly Logger Log = Logger.GetLogger(typeof(TrialInterScreen));

        public TrialInterScreen()
        {
            Name = "TrialInterScreen";
            TrackingRequired = false;
            Screen = screen;
        }

        protected override void OnShowing()
        {
            Tracker.SendMessage("SHOW INTERSCREEN");
            if (Tracker is EyeTribeTracker)
                Tracker.Disconnect();
        }

        protected override void OnHidden()
        {
            Tracker.SendMessage("STOP INTERSCREEN");
        }
    }
}
