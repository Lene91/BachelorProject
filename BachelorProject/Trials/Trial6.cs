using System;

namespace BachelorProject.Trials
{
    class Trial6 : ExampleExercise
    {
        public Trial6(double pupilSize)
            : base(pupilSize)
        {
            Id = 6;
            _constraintsWithPersons.Add("c1", new Tuple<string, string>(null, null));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person5", "Person6"));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person1", "Person4"));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person2", "Person5"));
            _constraintsWithPersons.Add("c5", new Tuple<string, string>("Person4", null));
            _constraintsWithPersons.Add("c6", new Tuple<string, string>("Person2", null));
            _constraintsWithPersons.Add("c7", new Tuple<string, string>("Person5", null));
            _constraintsWithPersons.Add("c8", new Tuple<string, string>("Person3", "Person4"));
        }

        public override void CheckActualConstraints()
        {
            UpdateConstraint("c1", NumberSharingFood(2));
            UpdateConstraint("c2", SittingNextToEachOther(P5,P6));
            UpdateConstraint("c3", NotSittingNextToEachOther(P1,P4));
            UpdateConstraint("c4", NotSittingNextToEachOther(P2, P5));
            UpdateConstraint("c5", NoNeighbourIsSeat(P4));
            UpdateConstraint("c6", IsSeat(P2));
            UpdateConstraint("c7", NoNeighbourIsSeat(P5));
            UpdateConstraint("c8", SharingFood(P3, P4));
            

            /*if (NotSittingOnSomeone(P1))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (IsSeat(P5))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (NoNeighbourIsSeat(P2))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P1))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (SharingFood(P3))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;
            */
            /*
            if (sittingNextToEachOther(p2, p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (isSeat(p3))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p2, p3))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p2))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (notSittingOnSomeone(p5))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;
            */

            //return constraintsFullfilled;
        }
    }
}

