using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;

namespace BachelorProject
{
    /// Quelle: http://denismorozov.blogspot.de/2008/01/drag-controls-in-wpf-using.html
    public class Element
    {

        #region Fields

        bool isDragging = false;

        IInputElement inputElement = null;

        double x, y = 0;

        #endregion



        #region Constructor

        public Element() { }

        #endregion



        #region Properties

        public IInputElement InputElement
        {

            get { return this.inputElement; }

            set
            {

                this.inputElement = value;

                this.isDragging = false;

            }

        }

        public double X
        {

            get { return this.x; }

            set { this.x = value; }

        }

        public double Y
        {

            get { return this.y; }

            set { this.y = value; }

        }

        public bool IsDragging
        {

            get { return this.isDragging; }

            set { this.isDragging = value; }

        }

        #endregion

    }
}
