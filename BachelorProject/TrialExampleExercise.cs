﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BachelorProject.Helper;
using ExperimentTemplate;
using Logging;
using System.Windows;
using System.Drawing;


namespace BachelorProject
{

    public interface IAoiUpdate
    {
        void UpdateAoi(Circle person);
    }

    class TrialExampleExercise : Trial, IAoiUpdate
    {
        private readonly ExampleExercise _screen;
        private static readonly Logger Log = Logger.GetLogger(typeof(TrialExampleExercise));
        private static int _counter;
        private bool _constraintsThreadIsRunning;
        private string _constraints;
        private List<string> _names;
        private readonly int _trialId;
        private const double ScreenWidth = 1200;
        private const double ScreenHeight = 800;
        private readonly double _offsetX;
        private readonly double _offsetY;
        private readonly bool _timeLimit;
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timer2;

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, ExampleExercise trial, bool tracking, bool timeLimit, bool constraintHelp)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = tracking;
            Screen = trial;
            _screen = trial;
            _constraints = constraints;
            _names = names;
            _trialId = trial.GetId();
            _timeLimit = timeLimit;

            _offsetX = (SystemParameters.FullPrimaryScreenWidth - ScreenWidth) / 2;
            _offsetY = (SystemParameters.FullPrimaryScreenHeight - ScreenHeight) / 2;
            _screen.Initialize(this, numberOfPersons, constraints, names, Tracker, constraintHelp);
            CreateAois(_screen.GetPersons());
        }

        private void CreateAois(IEnumerable<Circle> persons)
        {
            foreach (var person in persons)
            {
                AOIs.Add(new MyAoi(person));
            }
        }

        private void DeleteAois()
        {
            AOIs.Clear();
        }

        protected override void OnShowing()
        {
            Tracker.SendMessage("TRIAL_START " + _trialId);
            Log.Info("Showing screen " + _counter + " and Trial " + _trialId + ".");
            if (_timeLimit)
            {
                _timer = new System.Timers.Timer(300000); // 5 Minuten = 300000
                //_timer.Elapsed += new System.Timers.ElapsedEventHandler(endTrial);
                _timer.AutoReset = false;
                _timer.Enabled = true;
            }

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
                var pos = new PointF((float)(e.Position.X - _offsetX), (float)(e.Position.Y - _offsetY));
                //if (aoi.Contains(pos))
                //Tracker.SendMessage(aoi + " contains " + pos);

                if (aoi.Points[0].X < pos.X && aoi.Points[1].X > pos.X && aoi.Points[0].Y < pos.Y && aoi.Points[1].Y > pos.Y)
                    Tracker.SendMessage(aoi + " contains " + pos);
            }
            /*if (screen.skip)
            {
                screen.skip = false;
                //SkipTrial();
            }*/
        }


        private void Tracker_FixationStart(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {

            Tracker.SendMessage("fixation start");
            /*foreach (var aoi in AOIs)
            {
                var pos = new PointF((float)(e.AveragePosition.X - _offsetX), (float)(e.AveragePosition.Y - _offsetY));
                //if (aoi.Contains(pos))
                //Tracker.SendMessage(aoi + " contains " + pos);

                if (aoi.Points[0].X < pos.X && aoi.Points[1].X > pos.X && aoi.Points[0].Y < pos.Y && aoi.Points[1].Y > pos.Y)
                    Tracker.SendMessage(aoi + " contains " + pos);
            }*/
            /*foreach (var aoi in AOIs) {
                Debug.WriteLine(aoi.Name + " -> " + e.AveragePosition + " :: " + aoi.Points[0] + ", " + aoi.Points[1]);
                var pos = new PointF(e.AveragePosition.X - 400, e.AveragePosition.Y - 200);
                if (aoi.Contains(pos))
                    Tracker.SendMessage(aoi  + " contains " + pos);
            }*/

        }

        /*
        private void Tracker_FixationEnd(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {
            Tracker.SendMessage("fixation end");
        }
         */

        private void endTrial(object source, System.Timers.ElapsedEventArgs e)
        {
            _screen.TakePicture("timeElapsed");
            _screen.ShowExerciseEnd();
            _timer2 = new System.Timers.Timer(300);
            //_timer2.Elapsed += new System.Timers.ElapsedEventHandler(Skip);
            _timer2.AutoReset = false;
            _timer2.Enabled = true;
        }

        private void Skip(object source, System.Timers.ElapsedEventArgs e)
        {
            Tracker.SendMessage("TIME ELAPSED.");
            SkipTrial();
        }

        protected override void OnShown()
        {
            Log.Info("Screen " + _counter + " and Trial " + _trialId + " are shown.");

            var constraintsThread = new Thread(CheckConstraints);
            _constraintsThreadIsRunning = true;
            constraintsThread.Start();
        }

        private void CheckConstraints()
        {
            //int counter = 50;
            while (_constraintsThreadIsRunning)
            {
                if (_screen.Skip)
                {
                    _screen.Skip = false;
                    SkipTrial();
                }

                _screen.ConstraintsFullfilled();

                /*if (screen.ConstraintsFullfilled())
                    counter--;

                if (counter < 0)
                {
                    screen.takePicture();
                    SkipTrial();
                }

                System.Threading.Thread.Sleep(10);
                 */
            }
        }

        protected override void OnHiding()
        {
            Log.Info("Hiding screen " + _counter + " and Trial " + _trialId + "...");
            _constraintsThreadIsRunning = false;
            DeleteAois();
        }

        protected override void OnHidden()
        {
            Tracker.SendMessage("TRIAL_STOP " + _trialId);
            Log.Info("Screen " + _counter + " and Trial " + _trialId + " hidden.");
            _counter++;
        }

        public void UpdateAoi(Circle person)
        {
            var updatedAoi = GetUpdatedAoi(person);
            updatedAoi.Set((int)person.GetPosition().X, (int)person.GetPosition().Y, person.GetRadius());
        }

        private MyAoi GetUpdatedAoi(Circle person)
        {
            return AOIs.Where(aoi => aoi.Name.Equals(person.GetName())).Cast<MyAoi>().FirstOrDefault();
            //Debug.Assert(false);
        }
    }
}
