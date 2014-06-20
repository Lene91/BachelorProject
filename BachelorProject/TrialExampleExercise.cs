using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExperimentTemplate;
using System.Diagnostics;
using Logging;
using Eyetracker;
using Eyetracker.EyeEvents;
using Eyetracker.EyeEvents.FixationDetectionStrategies;
using Eyetracker.Eyelink;
using Eyetracker.MouseTracker;
using System.Windows;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BachelorProject
{

    public interface IAoiUpdate
    {
        void UpdateAoi(Circle person);
    }

    class TrialExampleExercise : Trial, IAoiUpdate
    {
        private readonly ExampleExercise screen;
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialExampleExercise));
        private static int counter = 0;
        private bool constraintsThreadIsRunning = false;
        private string constraints;
        private List<string> names;
        private int trialID;
        private double screenWidth = 1200;
        private double screenHeight = 800;
        private double offsetX;
        private double offsetY;
        private bool timeLimit;
        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, ExampleExercise trial, bool tracking, bool timeLimit, bool constraintHelp)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = tracking;
            Screen = trial;
            screen = trial;
            this.constraints = constraints;
            this.names = names;
            this.trialID = trial.getID();
            this.timeLimit = timeLimit;

            offsetX = (SystemParameters.FullPrimaryScreenWidth - screenWidth) / 2;
            offsetY = (SystemParameters.FullPrimaryScreenHeight - screenHeight) / 2;
            screen.Initialize(this, numberOfPersons, constraints, names, Tracker, constraintHelp);
            CreateAois(screen.GetPersons());
        }

        private void CreateAois(List<Circle> persons)
        {
            foreach (var person in persons)
            {
                AOIs.Add(new MyAOI(person));
            }
        }

        private void DeleteAois()
        {
            AOIs.Clear();
        }

        protected override void OnShowing()
        {
            Tracker.SendMessage("TRIAL_START " + trialID);
            Log.Info("Showing screen " + counter + " and Trial " + trialID + ".");
            if (timeLimit)
            {
                timer = new System.Timers.Timer(300000); // 5 Minuten = 300000
                timer.Elapsed += new System.Timers.ElapsedEventHandler(endTrial);
                timer.AutoReset = false;
                timer.Enabled = true;
            }

            if (Tracker != null)
            {
                Tracker.GazeTick += Tracker_GazeTick;
                Tracker.FixationStart += Tracker_FixationStart;
                //Tracker.FixationEnd += Tracker_FixationEnd;
            }
        }


        private void Tracker_GazeTick(object sender, Eyetracker.EyeEvents.GazeTickEventArgs e)
        {
            Tracker.SendMessage(e.Position.ToString());
            /*if (screen.skip)
            {
                screen.skip = false;
                //SkipTrial();
            }*/
        }


        private void Tracker_FixationStart(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {

            Tracker.SendMessage("fixation start");
            foreach (var aoi in AOIs)
            {
                var pos = new PointF((float)(e.AveragePosition.X - offsetX), (float)(e.AveragePosition.Y - offsetY));
                //if (aoi.Contains(pos))
                //Tracker.SendMessage(aoi + " contains " + pos);

                if (aoi.Points[0].X < pos.X && aoi.Points[1].X > pos.X && aoi.Points[0].Y < pos.Y && aoi.Points[1].Y > pos.Y)
                    Tracker.SendMessage(aoi + " contains " + pos);
            }
            /*foreach (var aoi in AOIs) {
                Debug.WriteLine(aoi.Name + " -> " + e.AveragePosition + " :: " + aoi.Points[0] + ", " + aoi.Points[1]);
                var pos = new PointF(e.AveragePosition.X - 400, e.AveragePosition.Y - 200);
                if (aoi.Contains(pos))
                    Tracker.SendMessage(aoi  + " contains " + pos);
            }*/

        }

        /*
        private void Tracker_FixationEnd(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {
            Tracker.SendMessage("fixation end");
        }
         */

        private void endTrial(object source, System.Timers.ElapsedEventArgs e)
        {
            screen.takePicture("timeElapsed");
            screen.showExerciseEnd();
            timer2 = new System.Timers.Timer(300);
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(skip);
            timer2.AutoReset = false;
            timer2.Enabled = true;
        }

        private void skip(object source, System.Timers.ElapsedEventArgs e)
        {
            Tracker.SendMessage("TIME ELAPSED.");
            SkipTrial();
        }

        protected override void OnShown()
        {
            Log.Info("Screen " + counter + " and Trial " + trialID + " are shown.");

            var constraintsThread = new Thread(CheckConstraints);
            constraintsThreadIsRunning = true;
            constraintsThread.Start();
        }

        private void CheckConstraints()
        {
            //int counter = 50;
            while (constraintsThreadIsRunning)
            {
                if (screen.skip)
                {
                    screen.skip = false;
                    SkipTrial();
                }

                screen.ConstraintsFullfilled();

                /*if (screen.ConstraintsFullfilled())
                    counter--;

                if (counter < 0)
                {
                    screen.takePicture();
                    SkipTrial();
                }

                System.Threading.Thread.Sleep(10);
                 */
            }
        }

        protected override void OnHiding()
        {
            Log.Info("Hiding screen " + counter + " and Trial " + trialID + "...");
            constraintsThreadIsRunning = false;
            DeleteAois();
        }

        protected override void OnHidden()
        {
            Tracker.SendMessage("TRIAL_STOP " + trialID);
            Log.Info("Screen " + counter + " and Trial " + trialID + " hidden.");
            counter++;
        }

        public void UpdateAoi(Circle person)
        {
            var updatedAoi = GetUpdatedAoi(person);
            updatedAoi.Set((int)person.getPosition().X, (int)person.getPosition().Y, person.getRadius());
        }

        private MyAOI GetUpdatedAoi(Circle person)
        {
            foreach (var aoi in AOIs)
            {
                if (aoi.Name.Equals(person.getName()))
                    return (MyAOI)aoi;
            }
            //Debug.Assert(false);
            return null;
        }

    }
}
