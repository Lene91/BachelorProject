using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperimentTemplate;

namespace BachelorProject
{
    class CalibrationTrial : Trial
    {

        protected override void OnShown()
        {
            Tracker.CalibrationFinished += Tracker_CalibrationFinished;
            Tracker.DoCalibration(); 
        }

        private void Tracker_CalibrationFinished(object sender, EventArgs args)
        {
            Tracker.CalibrationFinished -= Tracker_CalibrationFinished;
            //EndTrial();
        }
    }
}
