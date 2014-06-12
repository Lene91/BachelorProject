using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperimentTemplate;
using Logging;

namespace BachelorProject
{
    class TrialInterScreen : Trial
    {
        private static readonly InterScreen screen = new InterScreen();
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialInterScreen));

        public TrialInterScreen()
        {
            Name = "TrialInterScreen";
            TrackingRequired = false;
            Screen = screen;
        }
    }
}
