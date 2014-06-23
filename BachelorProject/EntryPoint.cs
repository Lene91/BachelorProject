using BachelorProject.Trials;
using BachelorProject.Tutorial;
using ExperimentTemplate;
using Eyetracker.MouseTracker;
using Eyetracker.EyeTribe;
using System;
using System.Collections.Generic;
using System.Text;


namespace BachelorProject
{

    class EntryPoint
    {
        private static readonly Dictionary<int, string> AllConstraints = new Dictionary<int, string>();
        private static readonly List<string> AllNames = new List<string>();
        private static List<ExampleExercise> _allTrials;
        //private static int numberOfTrials = 3;

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
            ReadConstraints("constraints.txt"); // entsprechend Trialnumber entsprechenden Indexinhalt übergeben
            ReadNames("names.txt"); // shuffle()-Aufruf gibt neu sortierte Liste zurück
            _allTrials = new List<ExampleExercise> { new Trial1(), new Trial2(), new Trial3() };
            //int[] numberOfPersons = { 5, 4, 4 };


            // new TrialExampleExercise(Anzahl Personen, Constraints des Trials, Namen für Trial, Trial-Klasse, tracking, timeLimit, constraintHelper

            // TUTORIAL

            /*experiment.AddTrial(new Introduction(new IntroScreen1()));
            experiment.AddTrial(new Introduction(new IntroScreen2()));
            experiment.AddTrial(new Introduction(new IntroScreen3()));
            experiment.AddTrial(new Introduction(new IntroScreen4()));
            
            // Tutorial mit Ausprobieren
            var trial0 = new Trial0();
            var tutorialTrial = new TrialExampleExercise(5, AllConstraints[0], ShuffledNames(), trial0, true, false, false, false);
            experiment.AddTrial(tutorialTrial);
            // 3 Übungsaufgaben
            experiment.AddTrial(new TrialExampleExercise(3, AllConstraints[1], ShuffledNames(), new TutTrial1(), true, false, false, false));
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[2], ShuffledNames(), new TutTrial2(), true, false, false, false));
            experiment.AddTrial(new TrialExampleExercise(4, AllConstraints[3], ShuffledNames(), new TutTrial3(), true, false, false, false));
            experiment.AddTrial(new TrialStartScreen());
            */
            experiment.AddTrial(new CalibrationTrial());
            // PILOTSTUDIE
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[4], ShuffledNames(), new Trial1(), true, false, false, true));
            experiment.AddTrial(new TrialInterScreen());

            experiment.AddTrial(new CalibrationTrial());
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[5], ShuffledNames(), new Trial2(), true, false, false, true));
            experiment.AddTrial(new TrialInterScreen());

            experiment.AddTrial(new CalibrationTrial());
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[6], ShuffledNames(), new Trial3(), true, false, false, true));
            experiment.AddTrial(new TrialInterScreen());

            experiment.AddTrial(new CalibrationTrial());
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[7], ShuffledNames(), new Trial4(), true, false, false, true));
            experiment.AddTrial(new TrialInterScreen());

            experiment.AddTrial(new CalibrationTrial());
            experiment.AddTrial(new TrialExampleExercise(6, AllConstraints[8], ShuffledNames(), new Trial5(), true, false, false, true));
            experiment.AddTrial(new TrialInterScreen());

            experiment.AddTrial(new CalibrationTrial());
            experiment.AddTrial(new TrialExampleExercise(6, AllConstraints[9], ShuffledNames(), new Trial6(), true, false, false, true));
            
            
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

        private static void ReadConstraints(string filename)
        {
            string line;
            var file = new System.IO.StreamReader(@filename, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                var trialNumber = Convert.ToInt32(line.Substring(0, line.IndexOf(";", StringComparison.Ordinal)));
                var constraints = line.Substring(line.IndexOf(";", StringComparison.Ordinal) + 1);
                AllConstraints.Add(trialNumber, constraints);
            }
            file.Close();
        }

        private static void ReadNames(string filename)
        {
            string line;
            var file = new System.IO.StreamReader(@filename, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                AllNames.Add(line);
            }
            file.Close();
        }

        private static List<string> ShuffledNames()
        {
            // Source http://stackoverflow.com/questions/5383498/shuffle-rearrange-randomly-a-liststring
            var n = AllNames.Count;
            var rnd = new Random();
            while (n > 1)
            {
                var k = (rnd.Next(0, n) % n);
                n--;
                var value = AllNames[k];
                AllNames[k] = AllNames[n];
                AllNames[n] = value;
            }
            return AllNames;
        }

        private static List<ExampleExercise> ShuffledTrials()
        {
            var n = _allTrials.Count;
            var rnd = new Random();
            while (n > 1)
            {
                var k = (rnd.Next(0, n) % n);
                n--;
                var value = _allTrials[k];
                _allTrials[k] = _allTrials[n];
                _allTrials[n] = value;
            }
            return _allTrials;
        }
    }
}
