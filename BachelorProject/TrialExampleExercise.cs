﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExperimentTemplate;
using System.Diagnostics;
using Logging;

namespace BachelorProject
{
    class TrialExampleExercise : Trial
    {
        private static readonly ExampleExercise screen = new ExampleExercise();
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialExampleExercise));
        private bool constraintsThreadIsRunning = false;
        private string constraints;

        public TrialExampleExercise(string constraints)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = true;
            Screen = screen;
            this.constraints = constraints;
            screen.InitializeConstraints(constraints);
        }

        protected override void OnShowing()
        {
            Log.Info("Showing screen 1.");
        }

        protected override void OnShown()
        {
            Log.Info("Screen 1 is shown.");

            //bee.StartFlyAround();
            var constraintsThread = new Thread(CheckConstraints);
            constraintsThreadIsRunning = true;
            constraintsThread.Start();
        }

        private void CheckConstraints()
        {
            while (constraintsThreadIsRunning)
            {
                if (screen.ConstraintsFullfilled())
                    //SkipTrial();
                System.Threading.Thread.Sleep(10);
            }
        }

        protected override void OnHiding()
        {
            Log.Info("Hiding screen 1...");
            //bee.StopFlyingAround();
            constraintsThreadIsRunning = false;
        }

        protected override void OnHidden()
        {
            Log.Info("Screen 1 hidden.");
        }

    }
}
