using System.Windows.Controls;
using ExperimentTemplate;

namespace BachelorProject.Tutorial
{
    class Introduction : Trial
    {
        public Introduction(UserControl uc)
        {
            Name = "Introduction";
            TrackingRequired = false;
            Screen = uc;
        }
    }
}
