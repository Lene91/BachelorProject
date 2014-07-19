using BachelorProject.Trials;
using BachelorProject.Tutorial;
using ExperimentTemplate;
using Eyetracker.MouseTracker;
using Eyetracker.EyeTribe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace BachelorProject
{

    class EntryPoint
    {
        private static readonly Dictionary<int, string> AllConstraints = new Dictionary<int, string>();
        private static readonly List<string> AllNames = new List<string>();
        private static readonly List<string> AllHints = new List<string>();
        private static List<ExampleExercise> _allTrials;
        //private static List<int> _hintModi = new List<int> { 0, 0, 1, 1, 5, 5 };
        private static List<int> _hintModi = new List<int> { 8, 8, 8, 9, 9, 9 };
        //public static double _pupilSize = 0;
        //private static int numberOfTrials = 3;
        

        [STAThread]
        static void Main()
        {
            //var experiment = new Experiment(new MouseTracker())
            var experiment = new Experiment(new EyeTribeTracker())
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false,
                OpenEyesOutputFilename = "lene.oey"
            };


            // Dateien, die für alle Trials benötigt werden -> Container befüllen
            ReadConstraints("constraints.txt"); // entsprechend Trialnumber entsprechenden Indexinhalt übergeben
            ReadNames("names.txt"); // shuffle()-Aufruf gibt neu sortierte Liste zurück
            ReadHints("hints.txt");
            double _pupilSize = 0;
            _allTrials = new List<ExampleExercise> 
            { 
                new Trial1(_pupilSize), 
                new Trial2(_pupilSize), 
                new Trial3(_pupilSize), 
                new Trial4(_pupilSize), 
                new Trial5(_pupilSize), 
                new Trial6(_pupilSize) 
            };
            int[] numberOfPersons = { 6, 5, 5, 5, 6, 6 };
            


            //experiment.AddTrial(new TrialExampleExercise(6, AllConstraints[2], ShuffledNames(), new Trial1(_pupilSize), true, false, false, 5, "Testhinweis"));
           
            // new TrialExampleExercise(Anzahl Personen, Constraints des Trials, Namen für Trial, Trial-Klasse, tracking, timeLimit, constraintHelper, hintModus, hint
            // hintModus:   0 -> no hints
            //              1 -> 20s without click, not before 45s, if not wanted -> start again
            //              2 -> resetButton clicked, or after 2min
            //              3 -> already fulfilled constraint changes status
            //              4 -> last viewed constraint is highlighted
            //              5 -> 4 + corresponding persons highlighted
            //              6 -> hint when pupil size bigger than normal
            //              7 -> 4 or 5 and hint is delivered, when each constraint is looked at 2 times (?), and 2s waiting time are elapsed
            //              8 -> hint when constraint focused, constraint solved and again constraintlist focused (hint: popup window)
            //              9 -> hint when constraint focused, constraint solved and again constraintlist focused (hint: highlighting) 

            // TUTORIAL

            /*experiment.AddTrial(new Introduction(new IntroScreen1()));
            experiment.AddTrial(new Introduction(new IntroScreen2()));
            experiment.AddTrial(new Introduction(new IntroScreen3()));
            experiment.AddTrial(new Introduction(new IntroScreen4()));*/
            
            // Tutorial mit Ausprobieren
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[0], ShuffledNames(), new Trial0(_pupilSize), true, false, false, 8, "special hint"));

            // 1 Übungsaufgabe
            //experiment.AddTrial(new TrialExampleExercise(3, AllConstraints[1], ShuffledNames(), new TutTrial1(), true, false, false, 0, ""));
            experiment.AddTrial(new TrialExampleExercise(5, AllConstraints[1], ShuffledNames(), new TutTrial2(_pupilSize), true, false, false, 9, ""));
            //experiment.AddTrial(new TrialExampleExercise(4, AllConstraints[3], ShuffledNames(), new TutTrial3(_pupilSize), true, false, false, 1, ""));
            //experiment.AddTrial(new Introduction(new IntroScreen5()));
            
            // HAUPTSTUDIE

            _allTrials = ShuffledTrials();
            _hintModi = ShuffledHintModi();

            var numberOfTutTrials = 2;
            var numberOfTrials = _allTrials.Count;
            for (int i = 0; i < numberOfTrials; ++i)
            {
                // Kalibrierung
                experiment.AddTrial(new CalibrationTrial());

                //Trial
                var trial = _allTrials[i];
                var id = trial.GetId();
                var index = id - 1;
                var constraintIndex = index + numberOfTutTrials;
                //var rnd = new Random();
                //var hintModus = rnd.Next(0, 3);
                experiment.AddTrial(new TrialExampleExercise(numberOfPersons[index], AllConstraints[constraintIndex], ShuffledNames(), trial, true, true, false, _hintModi[index], AllHints[index]));
                
                //Zwischenscreen -> Fragebogen
                experiment.AddTrial(new TrialInterScreen());
            }

            //experiment.AddTrial(new TrialEndScreen());

            
            experiment.ConfigureTracker();
            experiment.DoCalibration();

            experiment.Run();
        }

        private static void ReadHints(string filename)
        {
            string line;
            var file = new System.IO.StreamReader(@filename, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                AllHints.Add(line);
            }
            file.Close();
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

        private static List<int> ShuffledHintModi()
        {
            var n = _hintModi.Count;
            var rnd = new Random();
            while (n > 1)
            {
                var k = (rnd.Next(0, n) % n);
                n--;
                var value = _hintModi[k];
                _hintModi[k] = _hintModi[n];
                _hintModi[n] = value;
            }
            return _hintModi;
        }
    }
}
