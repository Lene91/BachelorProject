namespace BachelorProject.Trials
{
    class Trial1 : ExampleExercise
    {


        public Trial1()
        { Id = 1; }

        public override bool CheckActualConstraints()
        {
            
            if (SharingFood(P2, P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P2, P5))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P1, P2))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P2, P4))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;
            
            /*
            if (sittingNextToEachOther(p1, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sharingFood(p3, p4))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}
