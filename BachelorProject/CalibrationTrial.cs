using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ExperimentTemplate;

namespace BachelorProject
{
    class CalibrationTrial : Trial
    {
        protected override bool DriftCorrectRequired
        {
            get { return false; }
        }

        protected override void OnShown()
        {
            Tracker.CalibrationFinished += Tracker_CalibrationFinished;
            Tracker.DoCalibration(); 
        }

        private void Tracker_CalibrationFinished(object sender, EventArgs args)
        {
            Tracker.CalibrationFinished -= Tracker_CalibrationFinished;
        }
    }
}
