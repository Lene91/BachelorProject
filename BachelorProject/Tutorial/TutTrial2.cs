using System.Diagnostics;
using System;

namespace BachelorProject.Tutorial
{
    class TutTrial2 : ExampleExercise
    {
        /*private static double _counter = 1;
        public double _pupilSize;
        private object thisLock = new object();
        */
        public TutTrial2(double pupilSize)
            : base(pupilSize)
        {
            Id = 1002;
            _constraintsWithPersons.Add("c1", new Tuple<string, string>("Person4", "Person2"));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person5", "Person1"));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person2", "Person1"));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person5", null));
        }

        protected override void CheckActualConstraints()
        {
            UpdateConstraint("c1", NotSittingNextToEachOther(P4, P2));
            UpdateConstraint("c2", SittingNextToEachOther(P5, P1));
            UpdateConstraint("c3", SittingOn(P2,P1));
            UpdateConstraint("c4", OneNeighbourSharingFood(P5));
        }

        

        /*public override double GetPupilSize()
        {
            return _pupilSize;
        }*/
    }
}
