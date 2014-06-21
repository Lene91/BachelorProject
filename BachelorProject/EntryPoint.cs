using ExperimentTemplate;
using Eyetracker.MouseTracker;
using Eyetracker.EyeTribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Shapes;
using Eyetracker;
using System.Threading;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Text.RegularExpressions;


namespace BachelorProject
{

    class EntryPoint
    {
        private static Dictionary<int, string> allConstraints = new Dictionary<int, string>();
        private static List<string> allNames = new List<string>();
        private static List<ExampleExercise> allTrials;
        private static int numberOfTrials = 3;


        [STAThread]
        static void Main()
        {
            var experiment = new Experiment(new MouseTracker())
            //var experiment = new Experiment(new EyeTribeTracker())
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false
            };


            // Dateien, die für alle Trials benötigt werden -> Container befüllen
            readConstraints("constraints.txt"); // entsprechend Trialnumber entsprechenden Indexinhalt übergeben
            readNames("names.txt"); // shuffle()-Aufruf gibt neu sortierte Liste zurück
            allTrials = new List<ExampleExercise>() { new Trial1(), new Trial2(), new Trial3() };
            int[] numberOfPersons = { 5, 4, 4 };


            // new TrialExampleExercise(Anzahl Personen, Constraints des Trials, Namen für Trial, Trial-Klasse, tracking, timeLimit, constraintHelper

            // TUTORIAL

            experiment.AddTrial(new Introduction(new IntroScreen1()));
            experiment.AddTrial(new Introduction(new IntroScreen2()));
            experiment.AddTrial(new Introduction(new IntroScreen3()));
            experiment.AddTrial(new Introduction(new IntroScreen4()));
            
            // Tutorial mit Ausprobieren
            var trial0 = new Trial0();
            var tutorialTrial = new TrialExampleExercise(5, allConstraints[0], shuffledNames(), trial0, true, false, false);
            experiment.AddTrial(tutorialTrial);
            // 3 Übungsaufgaben
            experiment.AddTrial(new TrialExampleExercise(3, allConstraints[1], shuffledNames(), new TutTrial1(), true, false, false));
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[2], shuffledNames(), new TutTrial2(), true, false, false));
            experiment.AddTrial(new TrialExampleExercise(4, allConstraints[3], shuffledNames(), new TutTrial3(), true, false, false));
            experiment.AddTrial(new TrialStartScreen());


            // PILOTSTUDIE
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[4], shuffledNames(), new Trial1(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(4, allConstraints[5], shuffledNames(), new Trial2(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(6, allConstraints[6], shuffledNames(), new Trial3(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[7], shuffledNames(), new Trial4(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[8], shuffledNames(), new Trial5(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[9], shuffledNames(), new Trial6(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[10], shuffledNames(), new Trial7(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[11], shuffledNames(), new Trial8(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[12], shuffledNames(), new Trial9(), true, false, false));
            experiment.AddTrial(new TrialInterScreen());
            experiment.AddTrial(new TrialExampleExercise(6, allConstraints[13], shuffledNames(), new Trial10(), true, false, false));
            
            //allTrials = shuffledTrials();

            //experiment.AddTrial(new TrialExampleExercise(6, allConstraints[11], shuffledNames(), new Trial11(), true, true));
            //experiment.AddTrial(new TrialInterScreen());
            //experiment.AddTrial(new TrialExampleExercise(4, allConstraints[1], shuffledNames(), new Trial1(), true, true));
            /*experiment.AddTrial(new TrialExampleExercise(5, allConstraints[2], shuffledNames(), new Trial2(), true));
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[3], shuffledNames(), new Trial3(), true));
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[4], shuffledNames(), new Trial4(), true));
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[5], shuffledNames(), new Trial5(), true));
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[6], shuffledNames(), new Trial6(), true));
            experiment.AddTrial(new TrialExampleExercise(6, allConstraints[7], shuffledNames(), new Trial7(), true));
            experiment.AddTrial(new TrialExampleExercise(4, allConstraints[8], shuffledNames(), new Trial8(), true));
            experiment.AddTrial(new TrialExampleExercise(5, allConstraints[9], shuffledNames(), new Trial9(), true));
            experiment.AddTrial(new TrialExampleExercise(6, allConstraints[10], shuffledNames(), new Trial10(), true));
            */

            /*for (int i = 0; i < numberOfTrials; ++i)
            {
                var trial = allTrials[i];
                var id = trial.getID();
                var index = id - 1;
                experiment.AddTrial(new TrialExampleExercise(numberOfPersons[index], allConstraints[id], shuffledNames(), trial, true));
            }*/

            experiment.AddTrial(new TrialEndScreen());


            experiment.ConfigureTracker();
            experiment.DoCalibration();

            experiment.Run();
        }

        private static void readConstraints(string filename)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@filename, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                var trialNumber = Convert.ToInt32(line.Substring(0, line.IndexOf(";")));
                var constraints = line.Substring(line.IndexOf(";") + 1);
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

        private static List<string> shuffledNames()
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
            return allNames;
        }

        private static List<ExampleExercise> shuffledTrials()
        {
            int n = allTrials.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                var value = allTrials[k];
                allTrials[k] = allTrials[n];
                allTrials[n] = value;
            }
            return allTrials;
        }
    }
}
