using ExperimentTemplate;
using Eyetracker.MouseTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BachelorProject
{
    class EntryPoint
    {
        private static Dictionary<int, string> allConstraints = new Dictionary<int, string>();

        [STAThread]
        static void Main()
        {
            var experiment = new Experiment(new MouseTracker())
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false
            };

            readFile("constraints.txt");
            experiment.AddTrial(new TrialExampleExercise(allConstraints[1]));

            experiment.ConfigureTracker();
            experiment.DoCalibration();

            experiment.Run();
        }

        private static void readFile(string filename)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@filename,Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                var trialNumber = Convert.ToInt32(line.Substring(0,line.IndexOf(";")));
                var constraints = line.Substring(line.IndexOf(";")+1);
                allConstraints.Add(trialNumber, constraints);
            }
            file.Close();
        }
    }
}
