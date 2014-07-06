using System.Diagnostics;

namespace BachelorProject.Tutorial
{
    class TutTrial2 : ExampleExercise
    {
        private static double _counter = 0.0;
        //public static double _pupilSize;
        private object thisLock = new object();

        public TutTrial2()
        { Id = 1002; }

        public override void CheckActualConstraints()
        {
            if (NotSittingNextToEachOther(P4, P2))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P5, P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingOn(P2,P1))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P5))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            
            //return constraintsFullfilled;
        }

        public override double UpdatePupilSize(double pupilSize, double left, double right)
        {
            lock (thisLock)
            {
                if (_counter == 0)
                    pupilSize = 0;
                if (left != 0 && right != 0)
                {
                    double avg = (left + right) / 2.0;
                    pupilSize += avg;

                    if (_counter > 2)
                        pupilSize = pupilSize / 2;
                    else
                    {
                        pupilSize = pupilSize / _counter;
                        _counter++;
                    }
                }
                return pupilSize;
            }
        }
    }
}
