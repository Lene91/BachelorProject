using System.Windows;

namespace BachelorProject.Helper
{
    /// Quelle: http://denismorozov.blogspot.de/2008/01/drag-controls-in-wpf-using.html
    public class Element
    {

        #region Fields

        bool _isDragging;

        IInputElement _inputElement;

        double _x, _y;

        #endregion



        #region Constructor

        #endregion



        #region Properties

        public IInputElement InputElement
        {

            get { return _inputElement; }

            set
            {

                _inputElement = value;

                _isDragging = false;

            }

        }

        public double X
        {

            get { return _x; }

            set { _x = value; }

        }

        public double Y
        {

            get { return _y; }

            set { _y = value; }

        }

        public bool IsDragging
        {

            get { return _isDragging; }

            set { _isDragging = value; }

        }

        #endregion

    }
}
