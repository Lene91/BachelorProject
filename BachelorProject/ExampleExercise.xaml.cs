using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace BachelorProject
{
    /// <summary>
    /// Interaktionslogik für ExampleExercise.xaml
    /// Quelle für Dragging-Interaktion: http://denismorozov.blogspot.de/2008/01/drag-controls-in-wpf-using.html
    /// </summary>
    public partial class ExampleExercise : UserControl
    {
        private Element current = new Element();
        private Circle table;
        private List <Circle> persons = new List<Circle>();
        private Circle currentCircle;
        private Circle p1;
        private Circle p2;
        private bool constraints = false;

        public ExampleExercise()
        {
            InitializeComponent();
            table = new Circle(Table);
            p1 = new Circle(Person1);
            p2 = new Circle(Person2);
            persons.Add(p1);
            persons.Add(p2);
        }

        public bool ConstraintsFullfilled()
        {
            CheckConstraints();
            return constraints;
        }

        private void CheckConstraints()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CheckConstraints());
                return;
            }

            constraints = true;
            if (p1.touches(table))
                c1.Background = Brushes.Green;
            else
            {
                c1.Background = Brushes.Red;
                constraints = false;
            }
            if (p2.touches(table))
                c2.Background = Brushes.Green;
            else
            {
                c2.Background = Brushes.Red;
                constraints = false;
            }
        }





        // REALIZING DRAGGING

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.current.X = Mouse.GetPosition((IInputElement)sender).X;
            this.current.Y = Mouse.GetPosition((IInputElement)sender).Y;
            this.current.InputElement.CaptureMouse();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentCircle.updatePosition(new Point(current.X,current.Y));

            if (this.current.InputElement != null)
            {
                this.current.IsDragging = false;
                this.current.InputElement.ReleaseMouseCapture();
                this.current.InputElement = null;
            }
            var pos = e.GetPosition(null);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // if mouse is down when its moving, then it's dragging current
            if (e.LeftButton == MouseButtonState.Pressed)
                this.current.IsDragging = true;

            if (this.current.IsDragging && current.InputElement != null)
            {
                // Retrieve the current position of the mouse.
                var newX = Mouse.GetPosition((IInputElement)sender).X;
                var newY = Mouse.GetPosition((IInputElement)sender).Y;
  
                // Reset the location of the object (add to sender's renderTransform
                // newPosition minus currentElement's position
                var rt = ((UIElement)this.current.InputElement).RenderTransform;
                var offsetX = rt.Value.OffsetX;
                var offsetY = rt.Value.OffsetY;
                rt.SetValue(TranslateTransform.XProperty, offsetX + newX - current.X);
                rt.SetValue(TranslateTransform.YProperty, offsetY + newY - current.Y);

                // Update position of the mouse
                current.X = newX;
                current.Y = newY;
            }
        }

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.current.InputElement = (IInputElement)sender;
            foreach (Circle c in persons) {
                if (c.getName().Equals(((Ellipse)sender).Name))
                {
                    currentCircle = c;
                    break;
                }
            }
        }
    }

}
