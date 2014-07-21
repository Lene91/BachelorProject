using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Drawing.Point;

namespace TestProjectJakob
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private Ellipse gaze = null;

        public UserControl1()
        {
            InitializeComponent();
        }

        public void ShowGazePos(Point p)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ShowGazePos(p));
                return;
            }

            if (gaze == null)
            {
                gaze = new Ellipse() { Width = 10, Height = 10, Fill = new SolidColorBrush(Colors.CornflowerBlue)};
                MainGrid.Children.Add(gaze);
            }

            var left = p.X - (1920 - 1200) / 2 - 5;
            var top = p.Y - (1200 - 800) / 2 - 5;
            var right = 1200 - left - 10;
            var bottom = 800 - top - 10;
            gaze.Margin = new Thickness(left, top, right, bottom);
        }
    }
}
