using ExperimentTemplate;
using Eyetracker.MouseTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Shapes;


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
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false
            };


            // Dateien, die für alle Trials benötigt werden -> Container befüllen
            readConstraints("constraints.txt"); // entsprechend Trialnumber entsprechenden Indexinhalt übergeben
            readNames("names.txt"); // shuffle()-Aufruf gibt neu sortierte Liste zurück
            allTrials = new List<ExampleExercise>() {new Trial1(), new Trial2(), new Trial3()};
            int[] numberOfPersons = {5,4,4};


            // Tutorial
            experiment.AddTrial(new Introduction(new IntroScreen1()));
            experiment.AddTrial(new Introduction(new IntroScreen2()));
            experiment.AddTrial(new Introduction(new IntroScreen3()));
            experiment.AddTrial(new Introduction(new IntroScreen4()));
            var trial0 = new Trial0(getTutorialText(0));
            var tutorialTrial = new TrialExampleExercise(4, allConstraints[0], shuffledNames(), trial0, false);
            /*experiment.AddTrial(tutorialTrial);
            tutorialTrial.setTutorialText(getTutorialText(1));
            experiment.AddTrial(tutorialTrial);
            tutorialTrial.setTutorialText(getTutorialText(2));
            experiment.AddTrial(tutorialTrial);
            tutorialTrial.setTutorialText(getTutorialText(3));
            experiment.AddTrial(tutorialTrial);
            tutorialTrial.setTutorialText(getTutorialText(4));
            experiment.AddTrial(tutorialTrial);*/

            tutorialTrial.setTutorialText(getTutorialText(5));
            experiment.AddTrial(tutorialTrial);


            allTrials = shuffledTrials();
            for (int i = 0; i < numberOfTrials; ++i)
            {
                var trial = allTrials[i];
                var id = trial.getID();
                var index = id - 1;
                experiment.AddTrial(new TrialExampleExercise(numberOfPersons[index], allConstraints[id], shuffledNames(), trial, true));
            }
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

        private static string getTutorialText(int page)
        {
            string tutorialText = "";
            if (page == 0)
                tutorialText = "Die Aufgabe kann nur gelöst werden, wenn alle Personen am Tisch sitzen. Platziere dafür den Kreis einer Person nah am Tisch, sodass die Ränder sich überschneiden.";
            else if (page == 1)
                tutorialText = "Möchten zwei Personen nebeneinander sitzen, so darf sich keine Person zwischen ihnen befinden. Sitzen nur zwei Personen am Tisch, so sitzen diese immer nebeneinander.";
            else if (page == 2)
                tutorialText = "Wenn sich zwei Personen ein Essen teilen möchten, müssen sie am Tisch sitzen und die Kreise der Personen müssen sich überschneiden.";
            else if (page == 3)
                tutorialText = "Möchte eine Person auf dem Schoß einer anderen sitzen, so musst du die eine Person auf die andere ziehen. Der Kreis der Person, die auf dem Schoß sitzt, verkleinert sich automatisch.";
            else if (page == 4)
                tutorialText = "Jetzt kannst du das Tool noch ein wenig ausprobieren und falls du keine weiteren Fragen hast, so gelangst du mit dem Drücken der Leertaste zu ein paar Übungsaufgaben.";
            else if (page == 5)
                tutorialText = "Die Aufgabe kann nur gelöst werden, wenn alle Personen am Tisch sitzen. Platziere dafür den Kreis einer Person nah am Tisch, sodass die Ränder sich überschneiden. \n \n Möchten zwei Personen nebeneinander sitzen, so darf sich keine Person zwischen ihnen befinden. Sitzen nur zwei Personen am Tisch, so sitzen diese immer nebeneinander. \n \n Wenn sich zwei Personen ein Essen teilen möchten, müssen sie am Tisch sitzen und die Kreise der Personen müssen sich überschneiden. Möchte eine Person auf dem Schoß einer anderen sitzen, so musst du die eine Person auf die andere ziehen. Der Kreis der Person, die auf dem Schoß sitzt, verkleinert sich automatisch. \n \n Jetzt kannst du das Tool noch ein wenig ausprobieren und falls du keine weiteren Fragen hast, so gelangst du mit dem Drücken der Leertaste zu ein paar Übungsaufgaben.";
            return tutorialText;

        }
    }
}
