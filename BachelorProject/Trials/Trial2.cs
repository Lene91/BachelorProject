using System;

namespace BachelorProject.Trials
{
    class Trial2 : ExampleExercise
    {

        public Trial2(double pupilSize)
            : base(pupilSize)
        {
            Id = 2;
            _constraintsWithPersons.Add("c1", new Tuple<string, string>("Person6", "Person1"));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person5", "Person1"));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person2", null));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person1", "Person2"));
            _constraintsWithPersons.Add("c5", new Tuple<string, string>("Person3", "Person6"));
            _constraintsWithPersons.Add("c6", new Tuple<string, string>("Person4", "Person5"));
            _constraintsWithPersons.Add("c7", new Tuple<string, string>("Person6", null));
            _constraintsWithPersons.Add("c8", new Tuple<string, string>("Person1", "Person6"));
        }

        protected override void CheckActualConstraints()
        {
            UpdateConstraint("c1", NotSittingOn(P6, P1));
            UpdateConstraint("c2", SittingNextToEachOther(P5, P1));
            UpdateConstraint("c3", IsNotSeat(P2));
            UpdateConstraint("c4", NotSittingNextToEachOther(P1, P2));
            UpdateConstraint("c5", SittingNextToEachOther(P3, P6));
            UpdateConstraint("c6", SittingNextToEachOther(P4, P5));
            UpdateConstraint("c7", SittingOnSomeone(P6));
            UpdateConstraint("c8", NotSittingNextToEachOther(P1,P6));

            /*if (SharingFood(P2, P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P1, P2))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P3, P4))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P1, P4))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;
            */

            /*
            if (notSittingNextToEachOther(p2,p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p5))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5,p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if(isNotSeat(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;
             */

            //return constraintsFullfilled;
        }
    }
}
