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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BachelorProject
{
    
    class TrialExampleExercise : Trial
    {
        private readonly ExampleExercise screen;
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialExampleExercise));
        private static int counter = 0;
        private bool constraintsThreadIsRunning = false;
        private string constraints;
        private List<string> names;
        private int trialID;

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, ExampleExercise trial, bool tracking)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = tracking;
            Screen = trial;
            screen = trial;
            this.constraints = constraints;
            this.names = names;
            this.trialID = trial.getID();
            screen.Initialize(numberOfPersons,constraints,names,Tracker);    
        }

        protected override void OnShowing()
        {
            Log.Info("Showing screen " + counter + " and Trial " + trialID + ".");

            if (Tracker != null)
            {
                Tracker.GazeTick += Tracker_GazeTick;
                Tracker.FixationStart += Tracker_FixationStart;
                Tracker.FixationEnd += Tracker_FixationEnd;
            }
        }

        private void Tracker_GazeTick(object sender, Eyetracker.EyeEvents.GazeTickEventArgs e)
        {
            Tracker.SendMessage(e.Position.ToString());
            //var gazePos = new Point(e.Position.X, e.Position.Y);
            //var now = DateTime.Now.Ticks / 10000;
            //gazePositions.Add(gazePos);
        }

        private void Tracker_FixationStart(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {
            Tracker.SendMessage("fixation start");
        }

        private void Tracker_FixationEnd(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {
            Tracker.SendMessage("fixation end");
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
            int counter = 50;
            while (constraintsThreadIsRunning)
            {
                if (screen.ConstraintsFullfilled())
                    counter--;
                if (counter < 0)
                {
                    screen.takePicture();
                    SkipTrial();
                }

                System.Threading.Thread.Sleep(10);
            }
        }

        protected override void OnHiding()
        {
            Log.Info("Hiding screen " + counter + " and Trial " + trialID + "...");
            constraintsThreadIsRunning = false;
        }

        protected override void OnHidden()
        {
            Log.Info("Screen " + counter + " and Trial " + trialID + " hidden.");
            counter++;
        }

    }
}
