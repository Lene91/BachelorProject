namespace BachelorProject.Trials
{
    class Trial4 : ExampleExercise
    {
        public Trial4()
        { Id = 4; }

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

            if (NotSharingFood(P1,P2))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;

            /*
            if (SittingNextToEachOther(P2, P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P1, P4))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P1))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (OneNeighbourIsSeat(P5))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P3, P5))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P5))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P2, P5))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (SittingOnSomeone(P4))
                UpdateConstraint("c8", true);
            else constraintsFullfilled = false;

            if (NumberSharingFood(1))
                UpdateConstraint("c9", true);
            else constraintsFullfilled = false;
            */
            /*
            if (sittingNextToEachOther(p3, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p3))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (numberSittingOn(1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}
