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
using System.Threading;
using Eyetracker;
using Eyetracker.EyeEvents;
using Eyetracker.EyeEvents.FixationDetectionStrategies;
using Eyetracker.Eyelink;
using Eyetracker.MouseTracker;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.IO;

namespace BachelorProject
{
    /// <summary>
    /// Interaktionslogik für ExampleExercise.xaml
    /// Quelle für Dragging-Interaktion: http://denismorozov.blogspot.de/2008/01/drag-controls-in-wpf-using.html
    /// </summary>
    public partial class ExampleExercise : UserControl
    {
        // IMPORTANT!
        private int testPerson = 1;

        private IAoiUpdate trial;

        private double screenWidth = SystemParameters.FullPrimaryScreenWidth;
        private double screenHeight = SystemParameters.FullPrimaryScreenHeight;

        private Element current = new Element();
        private Circle currentCircle;
        protected Circle table;
        protected Circle p1;
        protected Circle p2;
        protected Circle p3;
        protected Circle p4;
        protected Circle p5;
        protected Circle p6;
        private List<string> circleNames;
        private int circleRadius = 50;
        private List<Circle> persons;

        protected bool constraintsFullfilled = false;
        private StackPanel constraintStackPanel;
        private WrapPanel legendStackPanel;

        private List<System.Windows.Media.Brush> brushes;
        private List<Ellipse> circles;

        private int numberOfPersons;
        private string constraints;
        private string[] singleConstraints;
        private List<string> names;

        private List<KeyValuePair<Circle, double>> sittingOrder; // Kreis und Winkel zur Senkrechten auf dem Tisch

        protected int id;

        private ITracker tracker;

        public bool skip = false;

        private bool constraintHelp;


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

        public void Initialize(IAoiUpdate trial, int numberOfPersons, string constraints, List<string> names, ITracker tracker, bool constraintHelp)
        {
            this.trial = trial;
            this.numberOfPersons = numberOfPersons;
            this.constraints = constraints;
            this.names = names;
            this.tracker = tracker;
            this.constraintHelp = constraintHelp;

            circleNames = new List<string>() { "Person1", "Person2", "Person3", "Person4", "Person5", "Person6" };
            persons = new List<Circle>();
            brushes = new List<System.Windows.Media.Brush>();
            circles = new List<Ellipse>();
            names = new List<string>();
            sittingOrder = new List<KeyValuePair<Circle, double>>();

            InitializeColors();
            InitializeLegend();
            InitializeLeftInterface();
            InitializeConstraints();
            InitializeBigCircles();
        }

        public List<Circle> GetPersons()
        {
            return persons;
        }

        private void InitializeLeftInterface()
        {
            table = new Circle(Table, id);
            p1 = new Circle(Person1, id);
            p2 = new Circle(Person2, id);
            p3 = new Circle(Person3, id);
            p4 = new Circle(Person4, id);
            p5 = new Circle(Person5, id);
            p6 = new Circle(Person6, id);
            persons.Add(p1);
            persons.Add(p2);
            persons.Add(p3);
            persons.Add(p4);
            persons.Add(p5);
            persons.Add(p6);
            for (int i = 5; i > numberOfPersons - 1; --i)
            {
                persons.Remove(persons[i]);
                circleNames.Remove(circleNames[i]);
            }
        }

        private void InitializeColors()
        {
            System.Windows.Media.Brush[] allBrushes = new System.Windows.Media.Brush[] {
                System.Windows.Media.Brushes.White,
                System.Windows.Media.Brushes.Gold,
                System.Windows.Media.Brushes.OrangeRed,
                System.Windows.Media.Brushes.Red,
                System.Windows.Media.Brushes.YellowGreen,
                System.Windows.Media.Brushes.DarkGreen,
                System.Windows.Media.Brushes.LightSkyBlue,
                System.Windows.Media.Brushes.DarkBlue,
                System.Windows.Media.Brushes.Peru,
                System.Windows.Media.Brushes.MediumOrchid,
                System.Windows.Media.Brushes.MediumSeaGreen,
                System.Windows.Media.Brushes.RoyalBlue,
                System.Windows.Media.Brushes.DarkRed,
                System.Windows.Media.Brushes.HotPink
            };

            Random r = new Random();
            while (brushes.Count < numberOfPersons)
            {
                int rNumber = r.Next(allBrushes.Length);
                var brush = allBrushes[rNumber];
                if (!brushes.Contains(brush))
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
            TextBlock tb = new TextBlock
            {
                Name = "l0",
                Text = "Legende",
                Margin = new Thickness(810, 10, 0, 0),
                Background = System.Windows.Media.Brushes.Transparent,
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
                    Fill = brushes[i - 1],
                    Opacity = 0.8,
                    Width = 50,
                    Height = 50,
                    StrokeThickness = 3,
                    Stroke = System.Windows.Media.Brushes.Black,
                    Margin = new Thickness(10, 10, 10, 10)
                };
                legendStackPanel.Children.Add(e);

                TextBlock t = new TextBlock
                {
                    Name = "l" + i,
                    Text = names[i - 1],
                    Background = System.Windows.Media.Brushes.Transparent,
                    FontSize = 25,
                    Margin = new Thickness(10, 10, 10, 10)
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
            singleConstraints = constraints.Split(new Char[] { ';' });
            string[] newConstraints = new string[singleConstraints.Count()];

            // Ersetzen der Platzhalter (Zahlen) in Constraints durch konkrete Namen
            int index = 0;
            foreach (string s in singleConstraints)
            {
                var newString = s;
                for (int i = 1; i <= numberOfPersons; ++i)
                {
                    newString = newString.Replace(i.ToString(), names[i - 1]);
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
                Margin = new Thickness(800, 270, 0, 0)
            };
            this.MyCanvas.Children.Add(constraintStackPanel);

            // Überschrift "Sitzwünsche"
            TextBlock tb = new TextBlock
            {
                Name = "c0",
                Text = "Sitzwünsche",
                Margin = new Thickness(10, 10, 10, 10),
                Background = System.Windows.Media.Brushes.Transparent,
                FontSize = 26,
                FontWeight = FontWeights.Bold
            };
            constraintStackPanel.Children.Add(tb);

            // einzelne Constraints darstellen
            int constraintCounter = 1;
            foreach (string c in singleConstraints)
            {
                string labelName = "c" + constraintCounter;
                Border b = new Border
                {
                    Name = labelName,
                    BorderThickness = new Thickness(1),
                    BorderBrush = System.Windows.Media.Brushes.Black,
                    Margin = new Thickness(30, 3, 30, 3)
                };
                TextBlock t = new TextBlock
                {
                    Text = c,
                    FontSize = 20,
                    MaxWidth = 360,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                constraintCounter++;
                b.Child = t;
                constraintStackPanel.Children.Add(b);
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
                c.updatePosition(new System.Windows.Point(newCenterX, newCenterY));
                x += 110;
            }

        }

        public int getID()
        {
            return id;
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


        private void CheckConstraints()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CheckConstraints());
                return;
            }

            var pos = Mouse.GetPosition(null);
            //Debug.WriteLine(pos);
            tracker.SendMessage(pos.ToString());

            calculateSittingOrder();
            initiateRedBrush();
            constraintsFullfilled = true;

            constraintsFullfilled = checkActualConstraints();

            // Größe ändern, wenn ein Kreis über einem anderen liegt
            foreach (Circle p in persons)
            {
                if (currentCircle != null && !currentCircle.Equals(p))
                    currentCircle.checkSitting(p);
            }

            // Prüfen, ob alle am Tisch sitzen
            foreach (Circle c in persons)
            {
                if (!c.touches(table))
                    constraintsFullfilled = false;
            }
        }

        public virtual bool checkActualConstraints()
        {
            return false;
        }

        /*
         * **********************************************************
         *              (not) sitting next to                       *
         ************************************************************
         */

        private void calculateSittingOrder()
        {
            sittingOrder.Clear();
            List<Circle> onTable = new List<Circle>();
            foreach (Circle c in persons)
            {
                if (c.getSeat() != null)
                {

                }
                else if (c.touches(table))
                {
                    onTable.Add(c);
                }
            }

            var centerPoint = table.getPosition();
            var tableEdgePoint = new System.Windows.Point(centerPoint.X, centerPoint.Y - table.getRadius());
            var firstVector = centerPoint - tableEdgePoint;

            foreach (Circle c in onTable)
            {
                var secondVector = centerPoint - c.getPosition();
                var angle = Math.Acos(firstVector * secondVector / (firstVector.Length * secondVector.Length));
                angle = angle * 180 / Math.PI;
                if (getHalf(c) == 2)
                    angle = 360 - angle;
                var newEntry = new KeyValuePair<Circle, double>(c, angle);
                if (!sittingOrder.Contains(newEntry))
                    sittingOrder.Add(newEntry);
            }

            sittingOrder.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );
        }

        protected bool sittingNextToEachOther(Circle c1, Circle c2)
        {
            int lastIndex = sittingOrder.Count - 1;
            int index = 0;


            if (sittingOrder.Count > 1)
            {
                foreach (KeyValuePair<Circle, double> kvp in sittingOrder)
                {
                    if (c1.getSeat() != null)
                        c1 = c1.getSeat();
                    if (c2.getSeat() != null)
                        c2 = c2.getSeat();

                    if (c1.Equals(kvp.Key) && c1.touches(table) && c2.touches(table))
                    {
                        if (index == 0)
                        {
                            if (c2.Equals(sittingOrder[lastIndex].Key) || c2.Equals(sittingOrder[index + 1].Key))
                                return true;
                            else return false;
                        }
                        else if (index == lastIndex)
                        {
                            if (c2.Equals(sittingOrder[index - 1].Key) || c2.Equals(sittingOrder[0].Key))
                                return true;
                            else return false;
                        }
                        else
                        {
                            if (c2.Equals(sittingOrder[index - 1].Key) || c2.Equals(sittingOrder[index + 1].Key))
                                return true;
                            else return false;
                        }
                    }
                    index++;
                }
            }
            return false;
        }

        protected bool notSittingNextToEachOther(Circle c1, Circle c2)
        {
            if (!c1.touches(table) || !c2.touches(table))
                return false;
            else
                return !sittingNextToEachOther(c1, c2);
        }

        private Tuple<Circle, Circle> getNeighbours(Circle c)
        {
            int counter = 0;
            int lastIndex = sittingOrder.Count - 1;
            Circle last = null;
            Circle next = null;
            foreach (KeyValuePair<Circle, double> kvp in sittingOrder)
            {
                if (c.Equals(kvp.Key))
                {
                    if (counter - 1 < 0)
                        last = sittingOrder[lastIndex].Key;
                    if (counter + 1 > lastIndex)
                        next = sittingOrder[0].Key;
                    if (last == null)
                        last = sittingOrder[counter - 1].Key;
                    if (next == null)
                        next = sittingOrder[counter + 1].Key;
                    return new Tuple<Circle, Circle>(last, next);
                }
                counter++;
            }
            return new Tuple<Circle, Circle>(null, null);
        }

        /*
         * **********************************************************
         *              (not) sharing food                          *
         ************************************************************
         */

        protected bool sharingFood(Circle c1, Circle c2)
        {
            if (c1.touches(table) && c2.touches(table) && c1.overlaps(c2) && c1.getRadius() == c2.getRadius())
                return true;
            else
                return false;
        }

        protected bool notSharingFood(Circle c1, Circle c2)
        {
            if (c1.touches(table) && c2.touches(table) && !c1.overlaps(c2))
                return true;
            else
                return false;
        }

        protected bool sharingFood(Circle c)
        {
            foreach (Circle p in persons)
            {
                if (!c.Equals(p) && sharingFood(c, p))
                    return true;
            }
            return false;
        }

        protected bool notSharingFood(Circle c)
        {
            foreach (Circle p in persons)
            {
                if ((!c.Equals(p) && sharingFood(c, p)))
                    return false;
            }
            if (c.touches(table))
                return true;
            return false;
        }

        protected bool numberSharingFood(int number)
        {
            int amount = 0;
            foreach (Circle p in persons)
            {
                if (!p.touches(table))
                    return false;
                foreach (Circle q in persons)
                {
                    if (!p.Equals(q) && sharingFood(p, q))
                        amount++;
                }
            }
            return amount / 2 == number;
        }

        protected bool oneNeighbourSharingFood(Circle c)
        {
            Tuple<Circle, Circle> neighbours = getNeighbours(c);
            if ((neighbours.Item1 != null && sharingFood(neighbours.Item1)) ^ (neighbours.Item2 != null && sharingFood(neighbours.Item2)))
                return true;
            return false;
        }

        protected bool atLeastOneNeighbourSharingFood(Circle c)
        {
            Tuple<Circle, Circle> neighbours = getNeighbours(c);
            if ((neighbours.Item1 != null && sharingFood(neighbours.Item1)) || (neighbours.Item2 != null && sharingFood(neighbours.Item2)))
                return true;
            return false;
        }

        protected bool noNeighbourSharingFood(Circle c)
        {
            Tuple<Circle, Circle> neighbours = getNeighbours(c);
            if ((neighbours.Item1 != null && notSharingFood(neighbours.Item1)) && (neighbours.Item2 != null && notSharingFood(neighbours.Item2)))
                return true;
            return false;
        }


        /*
         * **********************************************************
         *              (not) sitting somewhere                     *
         ************************************************************
         */

        protected bool sittingOn(Circle c1, Circle c2)
        {
            if (c2.touches(table) && c1.sitsOn(c2))
                return true;
            else
                return false;
        }

        protected bool notSittingOn(Circle c1, Circle c2)
        {
            if (c1.touches(table) && c2.touches(table) && !c1.sitsOn(c2))
                return true;
            else
                return false;
        }

        protected bool sittingOnSomeone(Circle c)
        {
            if (c.getSeat() != null && c.touches(table))
                return true;
            return false;
        }

        protected bool notSittingOnSomeone(Circle c)
        {
            if (c.getSeat() == null && c.touches(table))
                return true;
            return false;
        }

        protected bool isSeat(Circle c)
        {
            if (c.isSitter && c.touches(table))
                return true;
            return false;
        }

        protected bool isNotSeat(Circle c)
        {
            if (!c.isSitter && c.touches(table))
                return true;
            return false;
        }


        protected bool numberSittingOn(int number)
        {
            int amount = 0;
            foreach (Circle p in persons)
            {
                if (!p.touches(table))
                    return false;
                if (p.getSeat() != null)
                    amount++;
            }
            return amount == number;
        }

        protected bool oneNeighbourIsSeat(Circle c)
        {
            Tuple<Circle, Circle> neighbours = getNeighbours(c);
            if ((neighbours.Item1 != null && isSeat(neighbours.Item1)) ^ (neighbours.Item2 != null && isSeat(neighbours.Item2)))
                return true;
            return false;
        }

        protected bool atLeastOneNeighbourIsSeat(Circle c)
        {
            Tuple<Circle, Circle> neighbours = getNeighbours(c);
            if ((neighbours.Item1 != null && isSeat(neighbours.Item1)) || (neighbours.Item2 != null && isSeat(neighbours.Item2)))
                return true;
            return false;
        }

        protected bool noNeighbourIsSeat(Circle c)
        {
            Tuple<Circle, Circle> neighbours = getNeighbours(c);
            if (neighbours.Item1 != null && isNotSeat(neighbours.Item1) && neighbours.Item2 != null && isNotSeat(neighbours.Item2))
                return true;
            return false;
        }

        /*
         * **********************************************************
         *                                                          *
         *              HELPER FUNCTIONS                            *
         *                                                          *
         ************************************************************
         */


        protected void updateConstraint(string name, bool fulfilled)
        {
            Border b = getConstraint(name);
            TextBlock tb = b.Child as TextBlock;
            if (constraintHelp)
            {
                if (fulfilled)
                {
                    tb.Background = System.Windows.Media.Brushes.LightGreen;
                    b.BorderThickness = new Thickness(1);
                }
                else
                {
                    tb.Background = System.Windows.Media.Brushes.LightCoral;
                    b.BorderThickness = new Thickness(1);
                }
            }
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
            for (int i = 1; i <= singleConstraints.Count(); ++i)
            {
                string name = "c" + i.ToString();
                updateConstraint(name, false);
            }
        }

        private Border getConstraint(string name)
        {
            foreach (UIElement e in constraintStackPanel.Children)
            {
                if (e is Border)
                {
                    Border b = e as Border;
                    if (b.Name.Equals(name))
                        return b;
                }
            }
            return null;
        }

        public void takePicture()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => takePicture());
                return;
            }

            Bitmap Screenshot = new Bitmap((int)this.Width + 100, (int)this.Height + 100);
            Graphics G = Graphics.FromImage(Screenshot);
            // snip wanted area
            G.CopyFromScreen(350, 150, 0, 0, new System.Drawing.Size((int)this.Width + 50, (int)this.Height + 50), CopyPixelOperation.SourceCopy);

            // save uncompressed bitmap to disk
            //string fileName = "C:\\Users\\lganschow\\Documents\\Daten\\" + testPerson + "\\Trial" + id + ".bmp";
            string fileName = "C:\\Users\\Lene\\Desktop\\BA\\Daten\\" + testPerson + "\\Trial" + id + ".bmp";
            System.IO.FileStream fs = System.IO.File.Open(fileName, System.IO.FileMode.OpenOrCreate);
            Screenshot.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            fs.Close();
        }

        public void showExerciseEnd()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => showExerciseEnd());
                return;
            }

            /*foreach (UIElement e in MyCanvas.Children)
            {
                if (e is Border)
                {
                    Border b = e as Border;
                    if (b.Name.Equals("end"))
                    {
                        b.Margin = new Thickness(100, 250, 0, 0);
                    }
                }
            }*/

            Border b = new Border()
            {
                BorderThickness = new Thickness(3),
                BorderBrush = System.Windows.Media.Brushes.Black,
                Background = System.Windows.Media.Brushes.White,
                Height = 250,
                Margin = new Thickness(80, 250, 0, 0)
            };

            TextBlock tb = new TextBlock()
            {
                Padding = new Thickness(80),
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                Text = "Deine Zeit ist abgelaufen. In wenigen Sekunden geht es weiter."
            };

            b.Child = tb;
            MyCanvas.Children.Add(b);


        }


        /*
         * **********************************************************
         *                                                          *
         *              BUTTONS                                     *
         *                                                          *
         ************************************************************
         */

        private void Reset_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Circle el in persons)
            {
                // Reset the location of the object (add to sender's renderTransform
                // newPosition minus currentElement's position
                var rt = ((UIElement)el.getEllipse()).RenderTransform;
                var offsetX = rt.Value.OffsetX;
                var offsetY = rt.Value.OffsetY;
                rt.SetValue(TranslateTransform.XProperty, 0.0);
                rt.SetValue(TranslateTransform.YProperty, 0.0);

                GeneralTransform generalTransform1 = MyCanvas.TransformToVisual(el.getEllipse());
                System.Windows.Point currentPoint = generalTransform1.Inverse.Transform(new System.Windows.Point(0, 0));
                var newCenterX = currentPoint.X + circleRadius;
                var newCenterY = currentPoint.Y + circleRadius;
                el.updatePosition(new System.Windows.Point(newCenterX, newCenterY));

                // resize object
                el.updateRadius(50);
            }
            tracker.SendMessage("RESET");
        }

        private void Continue_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            takePicture();
            tracker.SendMessage("CONTINUE");
            skip = true;
        }

        private void Help_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tracker.SendMessage("HELP");
            constraintHelp = true;
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
                trial.UpdateAoi(currentCircle);
                updatePosition();
            }
        }

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.current.InputElement = (IInputElement)sender;
            foreach (Circle c in persons)
            {
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
            System.Windows.Point currentPoint = generalTransform1.Inverse.Transform(new System.Windows.Point(0, 0));
            var newCenterX = currentPoint.X + circleRadius;
            var newCenterY = currentPoint.Y + circleRadius;
            currentCircle.updatePosition(new System.Windows.Point(newCenterX, newCenterY));
        }
    }

}
