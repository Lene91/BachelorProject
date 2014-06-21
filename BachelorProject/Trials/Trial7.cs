namespace BachelorProject.Trials
{
    class Trial7 : ExampleExercise
    {
        public Trial7()
        { Id = 7; }

        public override bool CheckActualConstraints()
        {
            if (SharingFood(P2))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingOnSomeone(P4))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P1))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (OneNeighbourIsSeat(P3))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (OneNeighbourIsSeat(P1))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            /*
            if (sittingNextToEachOther(p2, p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (atLeastOneNeighbourSharingFood(p3))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (noNeighbourSharingFood(p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p6, p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p3, p5))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p6))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}

