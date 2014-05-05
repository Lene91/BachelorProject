using ExperimentTemplate;
using Eyetracker.MouseTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class EntryPoint
    {
        [STAThread]
        static void Main()
        {
            var experiment = new Experiment(new MouseTracker())
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false
            };

            experiment.AddTrial(new TrialExampleExercise());
            experiment.ConfigureTracker();
            experiment.DoCalibration();

            experiment.Run();
        }
    }
}
