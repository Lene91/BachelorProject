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
        private static List<string> allNames = new List<string>();

        [STAThread]
        static void Main()
        {
            var experiment = new Experiment(new MouseTracker())
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false
            };

            readConstraints("constraints.txt");
            readNames("names.txt");
            shuffle();
            var numberOfPersons = 6;
            experiment.AddTrial(new TrialExampleExercise(numberOfPersons, allConstraints[1], allNames));

            experiment.ConfigureTracker();
            experiment.DoCalibration();

            experiment.Run();
        }

        private static void readConstraints(string filename)
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

        private static void readNames(string filename)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@filename, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                allNames.Add(line);
            }
            file.Close();
        }

        public static void shuffle()
        {
            // Source http://stackoverflow.com/questions/5383498/shuffle-rearrange-randomly-a-liststring
            int n = allNames.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                string value = allNames[k];
                allNames[k] = allNames[n];
                allNames[n] = value;
            }
        }
    }
}
