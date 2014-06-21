namespace BachelorProject.Trials
{
    class Trial6 : ExampleExercise
    {
        public Trial6()
        { Id = 6; }

        public override bool CheckActualConstraints()
        {
            if (NotSittingOnSomeone(P1))
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

            return constraintsFullfilled;
        }
    }
}

