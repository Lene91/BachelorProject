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

        public ExampleExercise()
        {
            InitializeComponent();
            table = new Circle(Table);
            Circle c1 = new Circle(Person1);
            Circle c2 = new Circle(Person2);
            persons.Add(c1);
            persons.Add(c2);
            if (c1.touches(table))
                t1.Text = "lila am Tisch";
            else
                t1.Text = "lila nicht am Tisch";
            if (c2.touches(table))
                t2.Text = "hell am Tisch";
            else
                t2.Text = "hell nicht am Tisch";
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
            Debug.WriteLine(currentCircle.getName());
            currentCircle.updatePosition(new Point(current.X,current.Y));
            if (currentCircle.touches(table))
                t1.Text = "Person am Tisch";
            else
                t1.Text = "Person nicht am Tisch";
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
