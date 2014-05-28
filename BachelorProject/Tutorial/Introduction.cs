using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperimentTemplate;
using System.Windows.Controls;

namespace BachelorProject
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
