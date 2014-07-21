using ExperimentTemplate;

namespace TestProjectJakob
{
    class TestTrial1 : Trial
    {
        private UserControl1 _screen;

        public TestTrial1()
        {
            TrackingRequired = true;
            Screen = (_screen = new UserControl1());
        }

        protected override void OnShown()
        {
            Tracker.GazeTick += Tracker_GazeTick;
        }

        void Tracker_GazeTick(object sender, Eyetracker.EyeEvents.GazeTickEventArgs e)
        {
            _screen.ShowGazePos(e.Position);
        }

        protected override void OnHiding()
        {
            Tracker.GazeTick -= Tracker_GazeTick;
        }
    }
}
