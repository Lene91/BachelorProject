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
        private int circleRadius = 50;
        private bool constraints = false;
        private double deltaX;
        private double deltaY;
        //private double screenWidth = SystemParameters.FullPrimaryScreenWidth;
        //private double screenHeight = SystemParameters.FullPrimaryScreenHeight;
        //private double xOffSet;
        //private double yOffSet;
        StackPanel sp;

        public ExampleExercise()
        {
            InitializeComponent();
            InitializeLeftInterface();
            InitializeLegend();

            //xOffSet = 0.5 * (screenWidth - this.Width);
            //yOffSet = 0.5 * (screenHeight - this.Height); 
        }

        private void InitializeLeftInterface()
        {
            table = new Circle(Table);
            p1 = new Circle(Person1);
            p2 = new Circle(Person2);
            persons.Add(p1);
            persons.Add(p2);
        }

        private void InitializeLegend()
        {
            // maybe later needed
        }

        public void InitializeConstraints(string constraints)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CheckConstraints());
                return;
            }
            string[] singleConstraints = constraints.Split(new Char[]{';'});
            sp = new StackPanel 
            { 
                Name = "ConstraintPanel", 
                Orientation = Orientation.Vertical, 
                Width = 400, 
                Height = 550, 
                Margin = new Thickness(800, 250, 0, 0) };
            this.MyCanvas.Children.Add(sp);

            TextBox tb = new TextBox
            {
                Name = "c0",
                Text = "Regeln",
                Margin = new Thickness(20, 20, 20, 20),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                FontSize = 30,
                FontWeight = FontWeights.Bold
            };
            sp.Children.Add(tb);

            int constraintCounter = 1;
            foreach (string c in singleConstraints)
            {
                string labelName = "c" + constraintCounter;
                TextBox t = new TextBox 
                { 
                    Name = labelName, 
                    Text = c,
                    Margin = new Thickness(30, 10, 30, 10),
                    BorderBrush = Brushes.Black, 
                    BorderThickness = new Thickness(2),
                    FontSize = 25,
                    MaxWidth = 360,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                constraintCounter++;
                sp.Children.Add(t);
            }
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
                getConstraint("c1").Background = Brushes.Green;
            else
            {
                getConstraint("c1").Background = Brushes.Red;
                constraints = false;
            }
            if (p2.touches(table))
                getConstraint("c2").Background = Brushes.Green;
            else
            {
                getConstraint("c2").Background = Brushes.Red;
                constraints = false;
            }   
        }

        private TextBox getConstraint(string name)
        {
            foreach (UIElement e in sp.Children)
            {
                if (e is TextBox)
                {
                    TextBox t = e as TextBox;
                    if (t.Name.Equals(name))
                        return t;
                }
            }
            return null;
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
            if (this.current.InputElement != null)
            {
                this.current.IsDragging = false;
                this.current.InputElement.ReleaseMouseCapture();
                this.current.InputElement = null;
            }
            var pos = e.GetPosition(MyCanvas);
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

                var mousePos = e.GetPosition(MyCanvas);

                updatePosition();
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

        private void updatePosition()
        {
            Ellipse el = this.current.InputElement as Ellipse;
            GeneralTransform generalTransform1 = MyCanvas.TransformToVisual(el);
            Point currentPoint = generalTransform1.Inverse.Transform(new Point(0, 0));
            var newCenterX = currentPoint.X + 50;
            var newCenterY = currentPoint.Y + 50;
            currentCircle.updatePosition(new Point(newCenterX, newCenterY));
        }
    }

}
