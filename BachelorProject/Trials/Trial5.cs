namespace BachelorProject.Trials
{
    class Trial5 : ExampleExercise
    {
        public Trial5()
        { Id = 5; }

        public override bool CheckActualConstraints()
        {
            if (NotSittingNextToEachOther(P2, P4))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P5, P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (NotSittingOn(P2, P4))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P1, P2))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P3, P2))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P4, P5))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;


            /*
            if (notSittingNextToEachOther(p4, p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (numberSharingFood(2))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}
