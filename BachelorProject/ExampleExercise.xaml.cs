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
//using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.Reflection;


namespace BachelorProject
{
    /// <summary>
    /// Interaktionslogik für ExampleExercise.xaml
    /// Quelle für Dragging-Interaktion: http://denismorozov.blogspot.de/2008/01/drag-controls-in-wpf-using.html
    /// </summary>
    public partial class ExampleExercise : UserControl
    {
        private double screenWidth = SystemParameters.FullPrimaryScreenWidth;
        private double screenHeight = SystemParameters.FullPrimaryScreenHeight;

        private Element current = new Element();
        private Circle currentCircle;
        protected Circle table;
        private Circle p1;
        private Circle p2;
        protected Circle p3;
        private Circle p4;
        private Circle p5;
        private Circle p6;
        private List<string> circleNames;
        private int circleRadius = 50;
        private List<Circle> persons;
        
        protected bool constraintsFullfilled = false;
        private StackPanel constraintStackPanel;
        private WrapPanel legendStackPanel;

        private List<Brush> brushes;
        private List<Ellipse> circles;

        private int numberOfPersons;
        private string constraints;
        private string[] singleConstraints;
        private List<string> names;

        private List<KeyValuePair<Circle, double>> sittingOrder;


        public ExampleExercise()
        {
            InitializeComponent();
        }

        /*
         * **********************************************************
         *                                                          *
         *              INITIALIZING INTERFACE                      *
         *                                                          *
         ************************************************************
         */

        public void Initialize(int numberOfPersons, string constraints, List<string> names)
        {
            this.numberOfPersons = numberOfPersons;
            this.constraints = constraints;
            this.names = names;

            circleNames = new List<string>() {"Person1","Person2","Person3","Person4","Person5","Person6"};
            persons = new List<Circle>();
            brushes = new List<Brush>();
            circles = new List<Ellipse>();
            names = new List<string>();
            sittingOrder = new List<KeyValuePair<Circle, double>>();

            InitializeColors();
            InitializeLegend();
            InitializeLeftInterface();
            InitializeConstraints();
            InitializeBigCircles();
        }

        private void InitializeLeftInterface()
        {
            table = new Circle(Table);
            p1 = new Circle(Person1);
            p2 = new Circle(Person2);
            p3 = new Circle(Person3);
            p4 = new Circle(Person4);
            p5 = new Circle(Person5);
            p6 = new Circle(Person6);
            persons.Add(p1);
            persons.Add(p2);
            persons.Add(p3);
            persons.Add(p4);
            persons.Add(p5);
            persons.Add(p6);
            for (int i = 5; i > numberOfPersons-1; --i)
            {
                persons.Remove(persons[i]);
                circleNames.Remove(circleNames[i]);
            }
        }

        private void InitializeColors()
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
            while (brushes.Count < numberOfPersons)
            {
                int rNumber = r.Next(allBrushes.Length);
                var brush = allBrushes[rNumber];
                if(!brushes.Contains(brush))
                    brushes.Add(brush);
            }
        }

        private void InitializeLegend()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => InitializeLegend());
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
                Width = 330,
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


        public void InitializeConstraints()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => InitializeConstraints());
                return;
            }
            singleConstraints = constraints.Split(new Char[]{';'});
            string[] newConstraints = new string[singleConstraints.Count()];
            
            // Ersetzen der Platzhalter (Zahlen) in Constraints durch konkrete Namen
            int index = 0;
            foreach (string s in singleConstraints)
            {
                var newString = s;
                for (int i = 1; i <= numberOfPersons; ++i)
                {
                    newString = newString.Replace(i.ToString(), names[i-1]);
                }
                newConstraints[index] = newString;
                index++;
            }
            singleConstraints = newConstraints;

            // Panel für Constraints
            constraintStackPanel = new StackPanel 
            { 
                Name = "ConstraintPanel", 
                Orientation = Orientation.Vertical, 
                Width = 400, 
                Height = 550, 
                Margin = new Thickness(800, 270, 0, 0) };
            this.MyCanvas.Children.Add(constraintStackPanel);

            // Überschrift "Regeln"
            TextBox tb = new TextBox
            {
                Name = "c0",
                Text = "Sitzwünsche",
                Margin = new Thickness(10, 10, 10, 10),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                FontSize = 26,
                FontWeight = FontWeights.Bold
            };
            constraintStackPanel.Children.Add(tb);

            // einzelne Constraints darstellen
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
            // Liste mit allen Personenkreisen erstellen
            foreach (UIElement e in MyCanvas.Children)
            {
                if (e is Ellipse)
                {
                    Ellipse el = e as Ellipse;
                    if (!el.Name.Equals("Table") && circleNames.Contains(el.Name))
                    {
                        circles.Add(el);
                    }
                }
            }

            // Positionieren und Einfärben der einzelnen Personenkreise
            int counter = 0;
            int x = 30;
            foreach (Ellipse e in circles)
            {
                e.Fill = brushes[counter];
                e.Margin = new Thickness(x, 30, 0, 0);
                x += 110;
                counter++;
            }

            x = 30;
            foreach (Circle c in persons)
            {
                var newCenterX = x + circleRadius;
                var newCenterY = 30 + circleRadius;
                c.updatePosition(new Point(newCenterX, newCenterY));
                x += 110;
            }
            
        }


        /*
         * **********************************************************
         *                                                          *
         *                   LOGIC UNIT                             *
         *                                                          *
         ************************************************************
         */


        public bool ConstraintsFullfilled()
        {
            CheckConstraints();
            return constraintsFullfilled;
        }

        /*public static Assembly CompileCode(string CodeInput)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("ExperimentTemplate.dll");
            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using ExperimentTemplate;");
            sb.AppendLine(@"namespace BachelorProject{");
            sb.AppendLine(@"public class Test{");
            sb.AppendLine(@"public string Ergebnis(string input){");
            sb.AppendLine(CodeInput);
            sb.AppendLine(@"return input;");
            sb.AppendLine(@"}}}");

            CompilerResults cr =
                provider.CompileAssemblyFromSource(cp, sb.ToString());

            if (cr.Errors.Count > 0)
            {
                Console.WriteLine(cr.Errors[0].ErrorText);
                return null;
            }

            return cr.CompiledAssembly;
        }*/

        private void CheckConstraints()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CheckConstraints());
                return;
            }

            calculateSittingOrder();
            initiateRedBrush();
            constraintsFullfilled = true;

            

            constraintsFullfilled = checkActualConstraints();

            var persons2 = persons;
            foreach (Circle p in persons)
            {
                foreach (Circle q in persons2)
                {
                    if (!p.Equals(q) && currentCircle != null && currentCircle.Equals(p))
                    {
                        if (p.enters(q))
                        {
                            p.isSittingOn(q);
                            break;
                        }
                        else if (p.leaves(q))
                        {
                            p.stopsSittingOn(q);
                            break;
                        }
                    }
                }
            }


            // Prüfen, ob alle am Tisch sitzen
            foreach (Circle c in persons)
            {
                if (!c.touches(table))
                    constraintsFullfilled = false;
            }


            /*
            if (!isSitting)
                getConstraint("c4").Background = Brushes.Red;

            //if (p1.touches(table))
                //getConstraint("c1").Background = Brushes.Green;
            //else constraintsFullfilled = false;

            if (p2.touches(table))
                getConstraint("c2").Background = Brushes.Green;
            else constraintsFullfilled = false;

            if (p3.touches(table) && p4.touches(table) && p3.overlaps(p4) && p3.touches(table))
                getConstraint("c3").Background = Brushes.Green;
            else constraintsFullfilled = false;

            if (p5.enters(p2) && currentCircle != null && currentCircle.Equals(p5) || isSitting)
            {
                p5.isSittingOn(p2);
                getConstraint("c4").Background = Brushes.Green;
                isSitting = true;
            }
            if (p5.leaves(p2) && currentCircle != null && currentCircle.Equals(p5) || !isSitting)
            {
                p5.stopsSittingOn(p2);
                getConstraint("c4").Background = Brushes.Red;
                constraintsFullfilled = false;
                isSitting = false;
            }*/

            //if (p1.sitsNextTo(p2, persons)) //&& p1.touches(table) && p2.touches(table))
            //getConstraint("c5").Text = p1.sitsNextTo(p2,persons);
            //else
            //getConstraint("c5").Background = Brushes.Red;

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

        public virtual bool checkActualConstraints()
        {
            return false;
        }

        private void calculateSittingOrder()
        {
            sittingOrder.Clear();
            List<Circle> onTable = new List<Circle>();
            foreach (Circle c in persons)
            {
                if (c.touches(table))
                    onTable.Add(c);
            }
            var centerPoint = table.getPosition();
            var tableEdgePoint = new Point(centerPoint.X, centerPoint.Y - table.getRadius());
            var firstVector = centerPoint - tableEdgePoint;
            
            foreach (Circle c in onTable)
            {
                var secondVector = centerPoint - c.getPosition();
                var angle = Math.Acos(firstVector * secondVector / (firstVector.Length * secondVector.Length));
                angle = angle * 180 / Math.PI;
                if (getHalf(c) == 2)
                    angle = 360 - angle;
                var newEntry = new KeyValuePair<Circle,double>(c,angle);
                if (!sittingOrder.Contains(newEntry))
                    sittingOrder.Add(newEntry);
            }

            sittingOrder.Sort((firstPair,nextPair) =>
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                }
            );  
        }

        private bool sittingNextToEachOther(Circle c1, Circle c2)
        {
            int lastIndex = sittingOrder.Count - 1;
            int index = 0;

            if (sittingOrder.Count > 1)
            {
                foreach (KeyValuePair<Circle, double> kvp in sittingOrder)
                {
                    if (c1.Equals(kvp.Key))
                    {
                        if (index == 0)
                        {
                            if (c2.Equals(sittingOrder[lastIndex].Key) || c2.Equals(sittingOrder[index + 1].Key))
                                return true;
                            else break;
                        }
                        else if (index == lastIndex)
                        {
                            if (c2.Equals(sittingOrder[index - 1].Key) || c2.Equals(sittingOrder[0].Key))
                                return true;
                            else break;
                        }
                        else
                        {
                            if (c2.Equals(sittingOrder[index - 1].Key) || c2.Equals(sittingOrder[index + 1].Key))
                                return true;
                            else break;
                        }
                    }
                    index++;
                }
            }
            return false;
        }

        private int getHalf(Circle c)
        {
            var x = c.getPosition().X;
            var tableX = table.getPosition().X;
            if (x >= tableX)
                return 1;
            else
                return 2;
        }

        private void initiateRedBrush()
        {
            for (int i = 1; i < singleConstraints.Count(); ++i)
            {
                string name = "c" + i.ToString();
                getConstraint(name).Background = Brushes.Red;
            }
        }

        protected TextBox getConstraint(string name)
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



        /*
         * **********************************************************
         *                                                          *
         *              REALIZING DRAGGING                          *
         *                                                          *
         ************************************************************
         */


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
