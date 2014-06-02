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

    public interface IAoiUpdate
    {
        System.Drawing.Point[] UpdateAoi(Circle person);
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

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, ExampleExercise trial, bool tracking)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = tracking;
            Screen = trial;
            screen = trial;
            this.constraints = constraints;
            this.names = names;
            this.trialID = trial.getID();
            screen.Initialize(this,numberOfPersons,constraints,names,Tracker);
            //Debug.WriteLine(screen.GetPersons().Count);
            CreateAois(screen.GetPersons());
            //Debug.WriteLine("aoi " + AOIs.Count);
        }

        private void CreateAois(List<Circle> persons)
        {
            foreach (var person in persons)
            {
                AOIs.Add(new MyAOI(person));
            }
        }

        protected override void OnShowing()
        {
            Tracker.SendMessage("TRIAL_START " + trialID);
            Log.Info("Showing screen " + counter + " and Trial " + trialID + ".");

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
            foreach (var aoi in AOIs)
            {
                var pos = new PointF(e.Position.X - 400, e.Position.Y - 200);
                Debug.WriteLine(aoi.Name + " -> " + pos.ToString());
                //if (aoi.Contains(pos))
                    //Tracker.SendMessage(aoi + " contains " + pos);

                Debug.WriteLine(trialID);
                var aoiTrialId = Convert.ToInt32(aoi.Name.Substring(5, aoi.Name.IndexOf("P")-5));
                if (aoiTrialId == trialID && aoi.Points[0].X < pos.X && aoi.Points[1].X > pos.X && aoi.Points[0].Y < pos.Y && aoi.Points[1].Y > pos.Y)
                    Tracker.SendMessage(aoi + " contains " + pos);
            }
        }
        

        private void Tracker_FixationStart(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {
            /*foreach (var aoi in AOIs) {
                Debug.WriteLine(aoi.Name + " -> " + e.AveragePosition + " :: " + aoi.Points[0] + ", " + aoi.Points[1]);
                var pos = new PointF(e.AveragePosition.X - 400, e.AveragePosition.Y - 200);
                if (aoi.Contains(pos))
                    Tracker.SendMessage(aoi  + " contains " + pos);
            }*/
            Tracker.SendMessage("fixation start");
        }

        /*
        private void Tracker_FixationEnd(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {
            Tracker.SendMessage("fixation end");
        }
         */

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
            Tracker.SendMessage("TRIAL_STOP " + trialID);
            Log.Info("Screen " + counter + " and Trial " + trialID + " hidden.");
            counter++;
        }

        public System.Drawing.Point[] UpdateAoi(Circle person)
        {
            var updatedAoi = GetUpdatedAoi(person);
            updatedAoi.Set((int)person.getPosition().X, (int)person.getPosition().Y, person.getRadius());
            return new System.Drawing.Point[]{updatedAoi.Points[0],updatedAoi.Points[1]};
        }

        private MyAOI GetUpdatedAoi(Circle person)
        {
            foreach (var aoi in AOIs)
            {
                Debug.WriteLine("AOI: " + aoi);
                if (aoi.Name.Equals(person.getAoiName()))
                    return (MyAOI)aoi;
            }
            Debug.Assert(false);
            return null;
        }

    }
}
