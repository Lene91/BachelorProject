﻿using System;
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

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, ExampleExercise trial, bool tracking)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = tracking;
            Screen = trial;
            screen = trial;
            this.constraints = constraints;
            this.names = names;
            this.trialID = trial.getID();

            offsetX = (SystemParameters.FullPrimaryScreenWidth - screenWidth)/2;
            offsetY = (SystemParameters.FullPrimaryScreenHeight - screenHeight)/2;
            screen.Initialize(this,numberOfPersons,constraints,names,Tracker);
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
                var pos = new PointF((float)(e.Position.X - offsetX), (float)(e.Position.Y - offsetY));
                //if (aoi.Contains(pos))
                    //Tracker.SendMessage(aoi + " contains " + pos);

                if (aoi.Points[0].X < pos.X && aoi.Points[1].X > pos.X && aoi.Points[0].Y < pos.Y && aoi.Points[1].Y > pos.Y)
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
            Debug.Assert(false);
            return null;
        }

    }
}
