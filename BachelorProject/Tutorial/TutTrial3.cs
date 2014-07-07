using System.Diagnostics;

namespace BachelorProject.Tutorial
{
    class TutTrial3 : ExampleExercise
    {

        public TutTrial3(double pupilSize)
            : base(pupilSize)
        { 
            Id = 1003;
        }

        public override void CheckActualConstraints()
        {
            if (OneNeighbourSharingFood(P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P1, P4))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SharingFood(P2))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            //return constraintsFullfilled;
        }
    }
}
