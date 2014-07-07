using System;
namespace BachelorProject.Trials
{
    class Trial1 : ExampleExercise
    {


        public Trial1()
        { 
            Id = 1;
            _constraintsWithPersons.Add("c1", new Tuple<string,string>("Person1", "Person3"));
            _constraintsWithPersons.Add("c2", new Tuple<string,string>("Person2", "Person5"));
            _constraintsWithPersons.Add("c3", new Tuple<string,string>("Person1", "Person2"));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person6", "Person3"));
            _constraintsWithPersons.Add("c5", new Tuple<string, string>("Person6", "Person4"));
            _constraintsWithPersons.Add("c6", new Tuple<string, string>("Person2", "Person4"));
        }

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
