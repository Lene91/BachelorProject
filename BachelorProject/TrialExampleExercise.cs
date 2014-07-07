using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using BachelorProject.Helper;
using ExperimentTemplate;
using Logging;
using System.Windows;
using System.Drawing;
using Eyetracker;


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
        private readonly int _hintModus;
        private DispatcherTimer _timer = new DispatcherTimer();
        private DispatcherTimer _timer2 = new DispatcherTimer();
        private DispatcherTimer _hintTimer = new DispatcherTimer();
        //private static double _averagePupilSize = 0;

        public TrialExampleExercise(int numberOfPersons, string constraints, List<string> names, ExampleExercise trial, bool tracking, bool timeLimit, bool constraintHelp, int hintModus, string hint)
        {
            Name = "TrialExampleExercise";
            TrackingRequired = tracking;
            Screen = trial;
            _screen = trial;
            _constraints = constraints;
            _names = names;
            _trialId = trial.GetId();
            _timeLimit = timeLimit;
            _hintModus = hintModus;


            Tracker.SendMessage("Trial " + _trialId + " - " + numberOfPersons + " Persons - HintModus: " + _hintModus);

            _offsetX = (SystemParameters.FullPrimaryScreenWidth - ScreenWidth) / 2;
            _offsetY = (SystemParameters.FullPrimaryScreenHeight - ScreenHeight) / 2;

            _screen.Initialize(this, numberOfPersons, constraints, names, Tracker, constraintHelp, hintModus, hint);
            CreateAois(_screen.GetPersons(), _screen.GetConstraints());
            _screen.SendAois(AOIs);

        }

        private void CreateAois(IEnumerable<Circle> persons, Dictionary<string,Rectangle> constraints)
        {
            foreach (var person in persons)
            {
                AOIs.Add(new MyAoi(person));
            }
            foreach (var constraint in constraints)
            {
                Rectangle rect = new Rectangle(constraint.Value.X,constraint.Value.Y,constraint.Value.Width,constraint.Value.Height);
                AOIs.Add(new AreaOfInterest(rect,constraint.Key));
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

            if (Tracker != null)
            {
                Tracker.GazeTick += Tracker_GazeTick;
                Tracker.FixationStart += Tracker_FixationStart;
                //Tracker.FixationEnd += Tracker_FixationEnd;
            }
        }


        private void Tracker_GazeTick(object sender, Eyetracker.EyeEvents.GazeTickEventArgs e)
        {
            //Tracker.SendMessage(e.Position.ToString());
            

            var pos = new PointF((float)(e.Position.X - _offsetX), (float)(e.Position.Y - _offsetY));

            //_screen.Show(pos, e.LeftPupilSize + ", " + e.RightPupilSize + ", " + sender.ToString());
            _screen.UpdatePos(pos);
            foreach (var aoi in AOIs)
            {
                if (aoi.Points[0].X < pos.X && aoi.Points[1].X > pos.X && aoi.Points[0].Y < pos.Y &&
                    aoi.Points[1].Y > pos.Y)
                {
                    Tracker.SendMessage(aoi + " contains " + pos);
                }
            }

            if (_trialId == 1002) // Tutorialtrial, bei dem durchschnittliche Pupillengröße ermittelt werden soll
            {
                _screen.UpdatePupilSize(e.LeftPupilSize, e.RightPupilSize);
            }
            else
                _screen.SendCurrentPupilSize(e.LeftPupilSize, e.RightPupilSize);
        }


        private void Tracker_FixationStart(object sender, Eyetracker.EyeEvents.FixationEventArgs e)
        {

            //Tracker.SendMessage("fixation start");
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

        private void EndTrial(object sender, EventArgs e)
        {
            _timer.Stop();
            _screen.TakePicture("timeElapsed");
            _screen.ShowExerciseEnd();
            _timer2 = new DispatcherTimer {Interval = TimeSpan.FromSeconds(3)};
            _timer2.Tick += Skip;
            _timer2.Start();
        }

        private void Skip(object sender, EventArgs e)
        {
            if (_timer.IsEnabled)
                _timer.Stop();
            if (_hintTimer.IsEnabled)
                _hintTimer.Stop();
            if (_timer2.IsEnabled)
                _timer2.Stop();
            Tracker.SendMessage("TIME ELAPSED.");
            SkipTrial();
        }

        protected override void OnShown()
        {

            Debug.WriteLine("info " + _screen.GetPupilSize());
            Log.Info("Screen " + _counter + " and Trial " + _trialId + " are shown.");

            if (_timeLimit)
            {
                _timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(5) };
                _timer.Tick += EndTrial;
                _timer.Start();
            }

            switch (_hintModus)
            {
                case 1:
                    _hintTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(45) };
                    _hintTimer.Tick += LookForClicks;
                    _hintTimer.Start();
                    break;
                case 2:
                    _hintTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(2) };
                    _hintTimer.Tick += ShowHint;
                    _hintTimer.Start();
                    break;
            }

            //if (_trialId == 1002)
                //_screen.SetPupilSize(0);


            var constraintsThread = new Thread(CheckConstraints);
            _constraintsThreadIsRunning = true;
            constraintsThread.Start();
        }

        private void LookForClicks(object sender, EventArgs e)
        {
            _hintTimer.Stop();
            _screen.StartNoClickTimer();
        }

        private void ShowHint(object sender, EventArgs e)
        {
            _screen.ShowHint(sender,e);
            _hintTimer.Stop();
        }

        private void CheckConstraints()
        {
            while (_constraintsThreadIsRunning)
            {
                if (_screen.Skip)
                {
                    if(_timer.IsEnabled)
                        _timer.Stop();
                    if (_hintTimer.IsEnabled)
                        _hintTimer.Stop();
                    if (_timer2.IsEnabled)
                        _timer2.Stop();
                    _screen.Skip = false;
                    SkipTrial();
                }

                _screen.ConstraintsFullfilled();
            }
        }

        protected override void OnHiding()
        {
            Log.Info("Hiding screen " + _counter + " and Trial " + _trialId + "...");
            _constraintsThreadIsRunning = false;
            DeleteAois();
            //if (_trialId == 1002)
                //_pupilSize = _screen.GetPupilSize();
        }

        protected override void OnHidden()
        {
            Tracker.SendMessage("TRIAL_STOP " + _trialId);
            Log.Info("Screen " + _counter + " and Trial " + _trialId + " hidden.");
            _counter++;

            //Debug.WriteLine("pupil " + _pupilSize);
        }

        public void UpdateAoi(Circle person)
        {
            var updatedAoi = GetUpdatedAoi(person);

            Debug.WriteLine(updatedAoi.Name);
            Debug.WriteLine("hallo " + (int)person.GetPosition().X);
            updatedAoi.Set((int)person.GetPosition().X, (int)person.GetPosition().Y, person.GetRadius());
        }

        private MyAoi GetUpdatedAoi(Circle person)
        {
            return AOIs.Where(aoi => aoi.Name.Equals(person.GetName())).Cast<MyAoi>().FirstOrDefault();
            //Debug.Assert(false);
        }
    }
}
