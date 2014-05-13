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
using System.Text.RegularExpressions;

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
        private Circle p3;
        private int circleRadius = 50;
        private bool constraints = false;
        private double screenWidth = SystemParameters.FullPrimaryScreenWidth;
        private double screenHeight = SystemParameters.FullPrimaryScreenHeight;
        //private double xOffSet;
        //private double yOffSet;
        StackPanel constraintStackPanel;
        WrapPanel legendStackPanel;
        Brush[] allBrushes;
        List<Brush> brushes = new List<Brush>();
        List<Ellipse> circles = new List<Ellipse>();
        private List<string> names = new List<string>();

        public ExampleExercise()
        {
            InitializeComponent();
            InitializeLeftInterface();

            //xOffSet = 0.5 * (screenWidth - this.Width);
            //yOffSet = 0.5 * (screenHeight - this.Height); 
        }

        public void Initialize(int numberOfPersons, string constraints, List<string> names)
        {
            InitializeColors(numberOfPersons);
            InitializeLegend(numberOfPersons, names);
            InitializeConstraints(constraints, names);
            InitializeBigCircles();

        }

        private void InitializeLeftInterface()
        {
            table = new Circle(Table);
            p1 = new Circle(Person1);
            p2 = new Circle(Person2);
            p3 = new Circle(Person3);
            persons.Add(p1);
            persons.Add(p2);
            persons.Add(p3);
        }

        private void InitializeColors(int number)
        {
            Brush[] allBrushes = new Brush[] {
                Brushes.AliceBlue,
                Brushes.AntiqueWhite,
                Brushes.Aqua,
                Brushes.Aquamarine,
                Brushes.Blue,
                Brushes.BlueViolet,
                Brushes.Brown,
                Brushes.BurlyWood,
                Brushes.CadetBlue,
                Brushes.Coral,
                Brushes.CornflowerBlue,
                Brushes.Crimson,
                Brushes.DarkGoldenrod,
                Brushes.DarkGreen,
                Brushes.DarkOrange,
                Brushes.Gold,
                Brushes.Fuchsia,
                Brushes.Indigo,
                Brushes.LightGreen,
                Brushes.Olive,
                Brushes.YellowGreen
            };

            Random r = new Random();
            while (brushes.Count < number)
            {
                int rNumber = r.Next(allBrushes.Length);
                var brush = allBrushes[rNumber];
                if(!brushes.Contains(brush))
                    brushes.Add(brush);
            }
        }

        private void InitializeLegend(int numberOfPersons, List<string> names)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CheckConstraints());
                return;
            }

            // Überschrift "Legende"
            TextBox tb = new TextBox
            {
                Name = "l0",
                Text = "Legende",
                Margin = new Thickness(810, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                FontSize = 26,
                FontWeight = FontWeights.Bold
            };
            MyCanvas.Children.Add(tb);

            // Panel für Legendeneinträge
            legendStackPanel = new WrapPanel()
            {
                Name = "LegendPanel",
                Width = 320,
                Height = 250,
                Margin = new Thickness(800, 50, 0, 0)
            };

            this.MyCanvas.Children.Add(legendStackPanel);

            // Legendeneinträge bestehend aus Kreis und Name
            for (int i = 1; i <= numberOfPersons; ++i)
            {
                Ellipse e = new Ellipse
                {
                    Name = "lc" + i,
                    Fill = brushes[i-1],
                    Opacity = 0.8,
                    Width = 50,
                    Height = 50,
                    StrokeThickness = 3,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(10,10,10,10)
                };
                legendStackPanel.Children.Add(e);

                TextBox t = new TextBox
                {
                    Name = "l" + i,
                    Text = names[i-1],
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    FontSize = 25,
                    Margin = new Thickness(10,10,10,10)
                };
                legendStackPanel.Children.Add(t);
            }
        }


        public void InitializeConstraints(string constraints, List<string> names)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CheckConstraints());
                return;
            }
            string[] singleConstraints = constraints.Split(new Char[]{';'});
            string[] newConstraints = new string[singleConstraints.Count()];
            int counter = 0;
            foreach (string s in singleConstraints)
            {
                var resultString = Regex.Match(s, @"\d+").Value;
                var resultInt = Int32.Parse(resultString);
                Debug.WriteLine(resultInt + " " + names[resultInt]);
                var newS = s.Replace(resultString, names[resultInt-1]);
                newConstraints[counter] = newS;
                Debug.WriteLine(newS);
                counter++;
            }
            singleConstraints = newConstraints;

            constraintStackPanel = new StackPanel 
            { 
                Name = "ConstraintPanel", 
                Orientation = Orientation.Vertical, 
                Width = 400, 
                Height = 550, 
                Margin = new Thickness(800, 270, 0, 0) };
            this.MyCanvas.Children.Add(constraintStackPanel);

            TextBox tb = new TextBox
            {
                Name = "c0",
                Text = "Regeln",
                Margin = new Thickness(10, 10, 10, 10),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                FontSize = 26,
                FontWeight = FontWeights.Bold
            };
            constraintStackPanel.Children.Add(tb);

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
                constraintStackPanel.Children.Add(t);
            }
        }

        private void InitializeBigCircles()
        {
            
            foreach (UIElement e in MyCanvas.Children)
            {
                if (e is Ellipse)
                    if (!(e as Ellipse).Name.Equals("Table"))
                        circles.Add(e as Ellipse);
            }

            int counter = 0;
            int x = 30;
            foreach (Ellipse e in circles)
            {
                e.Fill = brushes[counter];
                e.Margin = new Thickness(x, 30, 0, 0);
                x += 110;
                counter++;
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
            if (p1.overlaps(p2))
                getConstraint("c3").Background = Brushes.Green;
            else
            {
                getConstraint("c3").Background = Brushes.Red;
                constraints = false;
            }
            if (p3.enters(p2) && currentCircle != null && currentCircle.Equals(p3))
            {
                p3.isSittingOn(p2);
                getConstraint("c4").Background = Brushes.Green;
            }
            if (p3.leaves(p2) && currentCircle != null && currentCircle.Equals(p3))
            {
                p3.stopsSittingOn(p2);
                getConstraint("c4").Background = Brushes.Red;
                constraints = false;
            }
            //if (p1.sitsNextTo(p2, persons)) //&& p1.touches(table) && p2.touches(table))
            getConstraint("c5").Text = p1.sitsNextTo(p2,persons);
            //else
                //getConstraint("c5").Background = Brushes.Red;

            foreach (Circle p in persons)
            {
                bool bla = true;
                if (!p.touches(table))
                    bla = false;
                getConstraint("c4").Text = bla.ToString();
            }
            /*
            foreach(Circle p in persons)
            {
                foreach(Circle q in persons)
                {
                    if (!p.Equals(q))
                    {
                        if (p.enters(q) && currentCircle.Equals(p))
                            p.isSittingOn(q);
                        if (p.leaves(q) && currentCircle.Equals(p))
                            p.stopsSittingOn(q);
                    }
                }
            }*/
        }

        private TextBox getConstraint(string name)
        {
            foreach (UIElement e in constraintStackPanel.Children)
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
            if (this.current.InputElement != null)
            {
                this.current.X = Mouse.GetPosition((IInputElement)sender).X;
                this.current.Y = Mouse.GetPosition((IInputElement)sender).Y;
                this.current.InputElement.CaptureMouse();
            }
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
            var newCenterX = currentPoint.X + circleRadius;
            var newCenterY = currentPoint.Y + circleRadius;
            currentCircle.updatePosition(new Point(newCenterX, newCenterY));
        }
    }

}
