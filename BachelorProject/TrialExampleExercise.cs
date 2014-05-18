﻿using System;
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
        private static readonly ExampleExercise screen = new ExampleExercise();
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialExampleExercise));
        private bool constraintsThreadIsRunning = false;
        private string constraints;
        private List<string> names;
        //private delegate void function();

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, Del handler, MethodInfo mi,object o)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = true;
            Screen = screen;
            this.constraints = constraints;
            this.names = names;
            screen.Initialize(numberOfPersons,constraints,names,handler);
            string textInput = "Console.WriteLine(\"Hallo\");";
            if (mi != null)
                mi.Invoke(o, new object[] { textInput });
        }

        protected override void OnShowing()
        {
            Log.Info("Showing screen 1.");
        }

        protected override void OnShown()
        {
            Log.Info("Screen 1 is shown.");

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
                if(counter < 0)
                    SkipTrial();

                System.Threading.Thread.Sleep(10);
            }
        }

        protected override void OnHiding()
        {
            Log.Info("Hiding screen 1...");
            constraintsThreadIsRunning = false;
        }

        protected override void OnHidden()
        {
            Log.Info("Screen 1 hidden.");
        }

    }
}
