using System;
using ExperimentTemplate;
using Eyetracker.EyeTribe;

namespace TestProjectJakob
{
    class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            var tracker = new EyeTribeTracker();
            var experiment = new Experiment(tracker)
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false,
            };

            experiment.AddTrial(new TestTrial1());
            experiment.DoCalibration();
            experiment.Run();
        }
    }
}
