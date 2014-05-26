using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExperimentTemplate;
using System.Diagnostics;
using Logging;

using System.CodeDom.Compiler;
using System.Reflection;

namespace BachelorProject
{
    class TrialExampleExercise : Trial
    {
        private readonly ExampleExercise screen;
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialExampleExercise));
        private static int counter = 1;
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
            screen.Initialize(numberOfPersons,constraints,names);    
        }

        protected override void OnShowing()
        {
            Log.Info("Showing screen " + counter + " and Trial " + trialID + ".");
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
                    ;//SkipTrial();

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


        public void setTutorialText(string text) {
            screen.setTutorialText(text);
        }

    }
}
