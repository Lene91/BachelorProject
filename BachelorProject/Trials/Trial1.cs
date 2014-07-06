namespace BachelorProject.Trials
{
    class Trial1 : ExampleExercise
    {


        public Trial1()
        { Id = 1; }

        public override void CheckActualConstraints()
        {

            UpdateConstraint("c1", SharingFood(P2, P3));
            //else constraintsFullfilled = false;

            UpdateConstraint("c2", NotSittingNextToEachOther(P2, P5));
            //else constraintsFullfilled = false;

            UpdateConstraint("c3", SittingNextToEachOther(P1, P2));
            //else constraintsFullfilled = false;

            UpdateConstraint("c4", NotSittingNextToEachOther(P6, P3));
            //else constraintsFullfilled = false;

            UpdateConstraint("c5", NotSharingFood(P6, P4));
            //else constraintsFullfilled = false;

            UpdateConstraint("c6", SittingNextToEachOther(P2, P4));
            //else constraintsFullfilled = false;
            
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

            //return constraintsFullfilled;
        }
    }
}
