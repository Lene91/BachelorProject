using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperimentTemplate;

namespace BachelorProject
{
    class TrialExampleExercise : Trial
    {
        private static readonly ExampleExercise screen = new ExampleExercise();

        public TrialExampleExercise()
        {
            Name = "TrialExampleExercise";
            TrackingRequired = true;
            Screen = screen;
        }

    }
}
