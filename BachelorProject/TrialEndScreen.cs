﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperimentTemplate;
using Logging;

namespace BachelorProject
{
    class TrialEndScreen : Trial
    {
        private static readonly EndScreen screen = new EndScreen();
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialEndScreen));

        public TrialEndScreen()
        {
            Name = "TrialEndScreen";
            TrackingRequired = false;
            Screen = screen;
        }
    }
}