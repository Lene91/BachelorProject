﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BachelorProject.Helper;
using Eyetracker;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace BachelorProject
{
    /// <summary>
    /// Interaktionslogik für ExampleExercise.xaml
    /// Quelle für Dragging-Interaktion: http://denismorozov.blogspot.de/2008/01/drag-controls-in-wpf-using.html
    /// </summary>
    public partial class ExampleExercise
    {
        // IMPORTANT!
        private const int TestPerson = 1;
        private const bool Laptop = true;

        private IAoiUpdate _trial;

        //private double _screenWidth = SystemParameters.FullPrimaryScreenWidth;
        //private double _screenHeight = SystemParameters.FullPrimaryScreenHeight;

        private readonly Element _current = new Element();
        private Circle _currentCircle;
        protected Circle table;
        protected Circle P1;
        protected Circle P2;
        protected Circle P3;
        protected Circle P4;
        protected Circle P5;
        protected Circle P6;
        private List<string> _circleNames;
        private const int CircleRadius = 50;
        private List<Circle> _persons;

        protected bool constraintsFullfilled = false;
        private StackPanel _constraintStackPanel;
        private WrapPanel _legendStackPanel;

        private List<System.Windows.Media.Brush> _brushes;
        private List<Ellipse> _circles;

        private int _numberOfPersons;
        private string _constraints;
        private string[] _singleConstraints;
        private List<string> _names;

        private List<KeyValuePair<Circle, double>> _sittingOrder; // Kreis und Winkel zur Senkrechten auf dem Tisch

        protected int Id;

        private ITracker _tracker;

        public bool Skip = false;

        private bool _constraintHelp;

        private int _hintModus;

        private int _pictureId = 1;

        private bool _hintDelivered = false;

        private int _counter = 0;

        private bool _infoShown = false;

        private VirtualizingPanel _donePanel = new VirtualizingStackPanel();
        private VirtualizingPanel _notDonePanel = new VirtualizingStackPanel();
        private VirtualizingPanel _firstHintWindow = new VirtualizingStackPanel();
        private VirtualizingPanel _secondHintWindow = new VirtualizingStackPanel();
        private Run _hintRun = new Run();

        private bool _doneButtonKlicked;

        private readonly DispatcherTimer _noClickTimer = new DispatcherTimer();
        private readonly DispatcherTimer _resetHintTimer = new DispatcherTimer();
        private readonly DispatcherTimer _waitTimer = new DispatcherTimer();

        private string _hint;

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

        public void Initialize(IAoiUpdate trial, int numberOfPersons, string constraints, List<string> names, ITracker tracker, bool constraintHelp, int hintModus, string hint)
        {
            _trial = trial;
            _numberOfPersons = numberOfPersons;
            _constraints = constraints;
            _names = names;
            _tracker = tracker;
            _constraintHelp = constraintHelp;
            _hintModus = hintModus;
            _hint = hint;

            _circleNames = new List<string> { "Person1", "Person2", "Person3", "Person4", "Person5", "Person6" };
            _persons = new List<Circle>();
            _brushes = new List<System.Windows.Media.Brush>();
            _circles = new List<Ellipse>();
            names = new List<string>();
            _sittingOrder = new List<KeyValuePair<Circle, double>>();

            InitializeColors();
            InitializeLegend();
            InitializeLeftInterface();
            InitializeConstraints();
            InitializeBigCircles();
            InitializeHints();
        }

        public List<Circle> GetPersons()
        {
            return _persons;
        }

        private void InitializeLeftInterface()
        {
            table = new Circle(Table);
            P1 = new Circle(Person1);
            P2 = new Circle(Person2);
            P3 = new Circle(Person3);
            P4 = new Circle(Person4);
            P5 = new Circle(Person5);
            P6 = new Circle(Person6);
            _persons.Add(P1);
            _persons.Add(P2);
            _persons.Add(P3);
            _persons.Add(P4);
            _persons.Add(P5);
            _persons.Add(P6);
            for (int i = 5; i > _numberOfPersons - 1; --i)
            {
                _persons.Remove(_persons[i]);
                _circleNames.Remove(_circleNames[i]);
            }

            // Trialnumber unten links anzeigen
            TextBlock tb = new TextBlock
            {
                Margin = new Thickness(5, 780, 0, 0),
                FontSize = 15,
                Foreground = System.Windows.Media.Brushes.Gray,
                Text = Id + " - " + _hintModus
            };
            MyCanvas.Children.Add(tb);
        }

        private void InitializeColors()
        {
            System.Windows.Media.Brush[] allBrushes =
            {
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

            foreach (System.Windows.Media.Brush b in allBrushes)
                b.Freeze();

            var r = new Random();
            while (_brushes.Count < _numberOfPersons)
            {
                var rNumber = r.Next(allBrushes.Length);
                var brush = allBrushes[rNumber];
                if (!_brushes.Contains(brush))
                    _brushes.Add(brush);
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
            var tb = new TextBlock
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
            _legendStackPanel = new WrapPanel
            {
                Name = "LegendPanel",
                Width = 320,
                Height = 250,
                Margin = new Thickness(800, 50, 0, 0)
            };
            

            MyCanvas.Children.Add(_legendStackPanel);

            // Legendeneinträge bestehend aus Kreis und Name
            for (int i = 1; i <= _numberOfPersons; ++i)
            {
                var e = new Ellipse
                {
                    Name = "lc" + i,
                    Fill = _brushes[i - 1],
                    Opacity = 0.8,
                    Width = 50,
                    Height = 50,
                    StrokeThickness = 3,
                    Stroke = System.Windows.Media.Brushes.Black,
                    Margin = new Thickness(10, 10, 10, 10)
                };
                _legendStackPanel.Children.Add(e);

                var t = new TextBlock
                {
                    Name = "l" + i,
                    Text = _names[i - 1],
                    Background = System.Windows.Media.Brushes.Transparent,
                    FontSize = 25,
                    Margin = new Thickness(10, 10, 10, 10)
                };
                _legendStackPanel.Children.Add(t);
            }
        }


        public void InitializeConstraints()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => InitializeConstraints());
                return;
            }
            _singleConstraints = _constraints.Split(new[] { ';' });
            var newConstraints = new string[_singleConstraints.Count()];

            // Ersetzen der Platzhalter (Zahlen) in Constraints durch konkrete Namen
            var index = 0;
            foreach (string s in _singleConstraints)
            {
                var newString = s;
                for (var i = 1; i <= _numberOfPersons; ++i)
                {
                    newString = newString.Replace(i.ToString(), _names[i - 1]);
                }
                newConstraints[index] = newString;
                index++;
            }
            _singleConstraints = newConstraints;

            // Panel für Constraints
            _constraintStackPanel = new StackPanel
            {
                Name = "ConstraintPanel",
                Orientation = System.Windows.Controls.Orientation.Vertical,
                Width = 400,
                Height = 550,
                Margin = new Thickness(800, 260, 0, 0)
            };
            MyCanvas.Children.Add(_constraintStackPanel);

            // Überschrift "Sitzwünsche"
            var tb = new TextBlock
            {
                Name = "c0",
                Text = "Sitzwünsche",
                Margin = new Thickness(10, 10, 10, 10),
                Background = System.Windows.Media.Brushes.Transparent,
                FontSize = 26,
                FontWeight = FontWeights.Bold
            };
            _constraintStackPanel.Children.Add(tb);

            // einzelne Constraints darstellen
            var constraintCounter = 1;
            foreach (var c in _singleConstraints)
            {
                var labelName = "c" + constraintCounter;
                var b = new Border
                {
                    Name = labelName,
                    BorderThickness = new Thickness(1),
                    BorderBrush = System.Windows.Media.Brushes.Black,
                    Margin = new Thickness(30, 3, 30, 3)
                };
                var t = new TextBlock
                {
                    Text = c,
                    FontSize = 20,
                    MaxWidth = 360,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                constraintCounter++;
                b.Child = t;
                _constraintStackPanel.Children.Add(b);
            }
        }

        private void InitializeBigCircles()
        {
            // Liste mit allen Personenkreisen erstellen
            foreach (UIElement e in MyCanvas.Children)
            {
                if (!(e is Ellipse)) continue;
                var el = e as Ellipse;
                if (!el.Name.Equals("Table") && _circleNames.Contains(el.Name))
                {
                    _circles.Add(el);
                }
            }

            // Positionieren und Einfärben der einzelnen Personenkreise
            var counter = 0;
            var x = 30;
            foreach (var e in _circles)
            {
                e.Fill = _brushes[counter];
                e.Margin = new Thickness(x, 30, 0, 0);
                x += 110;
                counter++;
            }

            x = 30;
            foreach (var c in _persons)
            {
                var newCenterX = x + CircleRadius;
                const int newCenterY = 30 + CircleRadius;
                c.UpdatePosition(new System.Windows.Point(newCenterX, newCenterY));
                x += 110;
            }

        }

        private void InitializeHints()
        {
            string newHint = _hint;
            var index = 0;
            for (var i = 1; i <= _numberOfPersons; ++i)
            {
                newHint = newHint.Replace(i.ToString(), _names[i - 1]);
            }
            _hint = newHint;
        }

        public int GetId()
        {
            return Id;
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



            if (_counter == 200)
            {                
                // Mauskoordinaten speichern
                var pos = Mouse.GetPosition(null);
                _tracker.SendMessage("MousePos " + pos.ToString());
                _counter = 0;
            }
            _counter++;

            if (_constraintHelp || _doneButtonKlicked)
            {
                // Sitzplatzbeziehungen prüfen
                CalculateSittingOrder();
                InitiateRedBrush();

                constraintsFullfilled = true;

                foreach (var p in _persons.Where(p => !p.Touches(table) && p.GetSeat() == null))
                    constraintsFullfilled = false;
                constraintsFullfilled = CheckActualConstraints();
            }

            if (_currentCircle == null) return;
            foreach (var p in _persons.Where(p => !_currentCircle.Equals(p)))
            {
                _currentCircle.CheckSitting(p);
            }
        }

        public virtual bool CheckActualConstraints()
        {
            return false;
        }

        /*
         * **********************************************************
         *              (not) sitting next to                       *
         ************************************************************
         */

        private void CalculateSittingOrder()
        {
            _sittingOrder.Clear();
            var onTable = _persons.Where(c => c.GetSeat() == null && c.Touches(table)).ToList();

            var centerPoint = table.GetPosition();
            var tableEdgePoint = new System.Windows.Point(centerPoint.X, centerPoint.Y - table.GetRadius());
            var firstVector = centerPoint - tableEdgePoint;

            foreach (var c in onTable)
            {
                var secondVector = centerPoint - c.GetPosition();
                var angle = Math.Acos(firstVector * secondVector / (firstVector.Length * secondVector.Length));
                angle = angle * 180 / Math.PI;
                if (GetHalf(c) == 2)
                    angle = 360 - angle;
                var newEntry = new KeyValuePair<Circle, double>(c, angle);
                if (!_sittingOrder.Contains(newEntry))
                    _sittingOrder.Add(newEntry);
            }

            _sittingOrder.Sort((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value));
        }

        protected bool SittingNextToEachOther(Circle c1, Circle c2)
        {
            var lastIndex = _sittingOrder.Count - 1;
            var index = 0;


            if (_sittingOrder.Count <= 1) return false;
            if (c1.GetSeat() != null)
                c1 = c1.GetSeat();
            if (c2.GetSeat() != null)
                c2 = c2.GetSeat();

            foreach (var kvp in _sittingOrder)
            {
                if (c1.Equals(kvp.Key) && c1.Touches(table) && c2.Touches(table))
                {
                    if (index == 0)
                    {
                        return c2.Equals(_sittingOrder[lastIndex].Key) || c2.Equals(_sittingOrder[index + 1].Key);
                    }
                    else if (index == lastIndex)
                    {
                        return c2.Equals(_sittingOrder[index - 1].Key) || c2.Equals(_sittingOrder[0].Key);
                    }
                    else
                    {
                        return c2.Equals(_sittingOrder[index - 1].Key) || c2.Equals(_sittingOrder[index + 1].Key);
                    }
                }
                index++;
            }
            return false;
        }

        protected bool NotSittingNextToEachOther(Circle c1, Circle c2)
        {
            if ((c1.GetSeat() != null && c1.GetSeat().Equals(c2)) || (c2.GetSeat() != null && c2.GetSeat().Equals(c1))) // Person sitzt auf dem Schoß der anderen
                return true;
            if (!c1.Touches(table) && c1.GetSeat() == null || !c2.Touches(table) && c2.GetSeat() == null)
                return false;
            else
                return !SittingNextToEachOther(c1, c2);
        }

        private Tuple<Circle, Circle> GetNeighbours(Circle c)
        {
            var counter = 0;
            var lastIndex = _sittingOrder.Count - 1;
            Circle last = null;
            Circle next = null;
            if (c.GetSeat() != null)
                c = c.GetSeat();
            foreach (var kvp in _sittingOrder)
            {
                if (c.Equals(kvp.Key))
                {
                    if (counter - 1 < 0)
                        last = _sittingOrder[lastIndex].Key;
                    if (counter + 1 > lastIndex)
                        next = _sittingOrder[0].Key;
                    if (last == null)
                        last = _sittingOrder[counter - 1].Key;
                    if (next == null)
                        next = _sittingOrder[counter + 1].Key;
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

        protected bool SharingFood(Circle c1, Circle c2)
        {
            return c1.Touches(table) && c2.Touches(table) && c1.Overlaps(c2) && c1.GetRadius() == c2.GetRadius();
        }

        protected bool NotSharingFood(Circle c1, Circle c2)
        {
            return c1.Touches(table) && c2.Touches(table) && (!c1.Overlaps(c2) || c1.GetRadius() != c2.GetRadius());
        }

        protected bool SharingFood(Circle c)
        {
            return _persons.Any(p => !c.Equals(p) && SharingFood(c, p));
        }

        protected bool NotSharingFood(Circle c)
        {
            if (_persons.Any(p => (!c.Equals(p) && SharingFood(c, p))))
            {
                return false;
            }
            return c.Touches(table);
        }

        protected bool NumberSharingFood(int number)
        {
            var amount = 0;
            foreach (var p in _persons)
            {
                if (!p.Touches(table) && p.GetSeat() == null)
                    return false;
                amount += _persons.Count(q => !p.Equals(q) && SharingFood(p, q));
            }
            return amount / 2 == number;
        }

        protected bool OneNeighbourSharingFood(Circle c)
        {
            var neighbours = GetNeighbours(c);
            return (neighbours.Item1 != null && SharingFood(neighbours.Item1)) ^ (neighbours.Item2 != null && SharingFood(neighbours.Item2));
        }

        protected bool AtLeastOneNeighbourSharingFood(Circle c)
        {
            var neighbours = GetNeighbours(c);
            return (neighbours.Item1 != null && SharingFood(neighbours.Item1)) || (neighbours.Item2 != null && SharingFood(neighbours.Item2));
        }

        protected bool NoNeighbourSharingFood(Circle c)
        {
            var neighbours = GetNeighbours(c);
            return (neighbours.Item1 != null && NotSharingFood(neighbours.Item1)) && (neighbours.Item2 != null && NotSharingFood(neighbours.Item2));
        }


        /*
         * **********************************************************
         *              (not) sitting somewhere                     *
         ************************************************************
         */

        protected bool SittingOn(Circle c1, Circle c2)
        {
            return c2.Touches(table) && c1.SitsOn(c2);
        }

        protected bool NotSittingOn(Circle c1, Circle c2)
        {
            return c1.Touches(table) && c2.Touches(table) && !c1.SitsOn(c2);
        }

        protected bool SittingOnSomeone(Circle c)
        {
            return c.GetSeat() != null;
        }

        protected bool NotSittingOnSomeone(Circle c)
        {
            return c.GetSeat() == null && c.Touches(table);
        }

        protected bool IsSeat(Circle c)
        {
            return c.IsSitter && c.Touches(table);
        }

        protected bool IsNotSeat(Circle c)
        {
            return !c.IsSitter && c.Touches(table);
        }


        protected bool NumberSittingOn(int number)
        {
            var amount = 0;
            foreach (var p in _persons)
            {
                if (!p.Touches(table))
                    return false;
                if (p.GetSeat() != null)
                    amount++;
            }
            return amount == number;
        }

        protected bool OneNeighbourIsSeat(Circle c)
        {
            var neighbours = GetNeighbours(c);
            return (neighbours.Item1 != null && IsSeat(neighbours.Item1)) ^ (neighbours.Item2 != null && IsSeat(neighbours.Item2));
        }

        protected bool AtLeastOneNeighbourIsSeat(Circle c)
        {
            var neighbours = GetNeighbours(c);
            return (neighbours.Item1 != null && IsSeat(neighbours.Item1)) || (neighbours.Item2 != null && IsSeat(neighbours.Item2));
        }

        protected bool NoNeighbourIsSeat(Circle c)
        {
            var neighbours = GetNeighbours(c);
            return neighbours.Item1 != null && IsNotSeat(neighbours.Item1) && neighbours.Item2 != null && IsNotSeat(neighbours.Item2);
        }

        /*
         * **********************************************************
         *                                                          *
         *                       HINTS                              *
         *                                                          *
         ************************************************************
         */

        public void ShowHint(object sender, EventArgs e)
        {
            if (_hintDelivered) return;
            if (_infoShown)
            {
                _waitTimer.Interval = TimeSpan.FromSeconds(4);
                _waitTimer.Tick += ShowHint;
                _waitTimer.Start();
                return;
            }
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ShowHint(sender,e));
                return;
            }
            foreach (var b in MyCanvas.Children.OfType<VirtualizingPanel>().Select(uie => uie))
            {
                if (b.Name.Equals("FirstHintWindow"))
                    _firstHintWindow = b;
                else if (b.Name.Equals("SecondHintWindow"))
                    _secondHintWindow = b;
            }
            
            foreach (var tb in _secondHintWindow.Children.OfType<Border>().Select(x => x).Select(bo => bo.Child as TextBlock))
            {
                if (tb == null) return;
                tb.Inlines.Add(new Run
                {
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    Text = "Hinweis"
                });
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new Run { Text = _hint });
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new Run{ Text="Ist dieser Hinweis hilfreich?"});
                tb.Inlines.Add(new LineBreak());
                var btn3 = new System.Windows.Controls.Button
                {
                    Name = "Btn3", 
                    Margin = new Thickness(20), 
                    FontSize = 25, 
                    FontWeight = FontWeights.Bold,
                    Width = 75,
                    Height = 50,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Content = "Ja"
                };
                btn3.Click += Helpful_Button_MouseDown;
                tb.Inlines.Add(btn3);
                var btn4 = new System.Windows.Controls.Button
                {
                    Name = "Btn4",
                    Margin = new Thickness(20),
                    FontSize = 25,
                    FontWeight = FontWeights.Bold,
                    Width = 75,
                    Height = 50,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Content = "Nein"
                };
                btn4.Click += Not_Helpful_Button_MouseDown;
                tb.Inlines.Add(btn4);
            }
            _firstHintWindow.Margin = new Thickness(200, 100, 0, 0);
            System.Windows.Controls.Panel.SetZIndex(_firstHintWindow, 100);
            _tracker.SendMessage("HINT SHOWN");
            _hintDelivered = true;
        }

        public void StartNoClickTimer()
        {
            _noClickTimer.Interval = TimeSpan.FromMinutes(0.25);
            _noClickTimer.Tick += ShowHint;
            _noClickTimer.Start();
        }

        /*
         * **********************************************************
         *                                                          *
         *              HELPER FUNCTIONS                            *
         *                                                          *
         ************************************************************
         */


        protected void UpdateConstraint(string name, bool fulfilled)
        {
            var b = GetConstraint(name);
            var tb = b.Child as TextBlock;
            if (!_constraintHelp) return;
            if (tb != null)
                tb.Background = fulfilled ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.LightCoral;
        }

        private int GetHalf(Circle c)
        {
            var x = c.GetPosition().X;
            var tableX = table.GetPosition().X;
            return x >= tableX ? 1 : 2;
        }

        private void InitiateRedBrush()
        {
            for (var i = 1; i <= _singleConstraints.Count(); ++i)
            {
                var name = "c" + i.ToString();
                UpdateConstraint(name, false);
            }
        }

        private Border GetConstraint(string name)
        {
            return _constraintStackPanel.Children.OfType<Border>().Select(e => e as Border).FirstOrDefault(b => b.Name.Equals(name));
        }

        public void TakePicture(string info)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => TakePicture(info));
                return;
            }

            var mousePos = System.Windows.Forms.Cursor.Position;
            var screenshot = new Bitmap((int)Width, (int)Height - 30);
            var g = Graphics.FromImage(screenshot);

            
            string fileName;
            // snip wanted area
            if (Laptop)
            {
                g.CopyFromScreen(80, 0, 0, 0, new System.Drawing.Size((int)this.Width, (int)this.Height), CopyPixelOperation.SourceCopy);
                fileName = "C:\\Users\\Lene\\Desktop\\BA\\Daten\\" + TestPerson + "\\Trial" + Id + "-" + _pictureId + "-" + info + ".jpg";
                mousePos.X = mousePos.X - 80;
            }
            else
            {
                g.CopyFromScreen(360, 210, 0, 0, new System.Drawing.Size((int)this.Width + 50, (int)this.Height + 50), CopyPixelOperation.SourceCopy);
                fileName = "C:\\Users\\lganschow\\Documents\\Daten\\" + TestPerson + "\\Trial" + Id + "-" + _pictureId + "-" + info + ".jpg";
                mousePos.X = mousePos.X - 360;
                mousePos.Y = mousePos.Y - 210;
            }

            // Maus Cursor auf Graphics-Objekt zeichnen
            var icon = new Icon(SystemIcons.Hand, 12, 12);
            var image = System.Drawing.Image.FromFile("cursor.png");
            g.DrawImage(image, mousePos);


            // save compressed jpg to disk
            System.IO.FileStream fs = System.IO.File.Open(fileName, System.IO.FileMode.OpenOrCreate);
            screenshot.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();

            _pictureId++;
        }

        public void ShowExerciseEnd()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ShowExerciseEnd());
                return;
            }

            var b = new Border
            {
                BorderThickness = new Thickness(3),
                BorderBrush = System.Windows.Media.Brushes.Black,
                Background = System.Windows.Media.Brushes.White,
                Height = 250,
                Margin = new Thickness(80, 250, 0, 0)
            };

            var tb = new TextBlock
            {
                Padding = new Thickness(80),
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                Text = "Deine Zeit ist abgelaufen. In wenigen Sekunden geht es weiter."
            };

            System.Windows.Controls.Panel.SetZIndex(b, 100);
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
            TakePicture("resetButton");

            foreach (var el in _persons)
            {
                // Reset the location of the object (add to sender's renderTransform
                // newPosition minus currentElement's position
                var rt = el.GetEllipse().RenderTransform;
                //var offsetX = rt.Value.OffsetX;
                //var offsetY = rt.Value.OffsetY;
                rt.SetValue(TranslateTransform.XProperty, 0.0);
                rt.SetValue(TranslateTransform.YProperty, 0.0);

                var generalTransform1 = MyCanvas.TransformToVisual(el.GetEllipse());
                if (generalTransform1.Inverse != null)
                {
                    var currentPoint = generalTransform1.Inverse.Transform(new System.Windows.Point(0, 0));

                    if (el.GetRadius() < CircleRadius)
                    {
                        var seat = el.GetSeat();
                        el.StopsSittingOn(seat);
                    }
                    var newCenterX = currentPoint.X + CircleRadius;
                    var newCenterY = currentPoint.Y + CircleRadius;

                    el.UpdatePosition(new System.Windows.Point(newCenterX, newCenterY));
                }

                // resize object
                el.UpdateRadius(CircleRadius);
            }
            _tracker.SendMessage("RESET BUTTON PRESSED");
            if (_hintModus == 2)
            {
                _resetHintTimer.Interval = TimeSpan.FromSeconds(4);
                _resetHintTimer.Tick += ShowHint;
                _resetHintTimer.Start();
            }
        }

        private void Continue_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TakePicture("continueButton");
            _tracker.SendMessage("CONTINUE BUTTON PRESSED");
            Skip = true;
        }

        /*private void Help_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tracker.SendMessage("HELP BUTTON PRESSED");
            _constraintHelp = true;
            TakePicture("helpButton");
        }*/

        private void Done_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _infoShown = true;
            _doneButtonKlicked = true;
            var done = ConstraintsFullfilled();
            _doneButtonKlicked = false;

            foreach (UIElement uie in MyCanvas.Children)
            {
                if (uie is VirtualizingPanel)
                {
                    var vp = uie as VirtualizingPanel;
                    if (vp.Name.Equals("Done"))
                        _donePanel = vp;
                    else if (vp.Name.Equals("NotDone"))
                        _notDonePanel = vp;
                    else uie.Opacity = 0.2;
                }
                else uie.Opacity = 0.2;
            }

            if (done)
            {
                _tracker.SendMessage("DONE BUTTON PRESSED (DONE)");
                TakePicture("doneButton(done)");
                // change margin so that it is visible
                System.Windows.Controls.Panel.SetZIndex(_donePanel, 100);
                _donePanel.Margin = new Thickness(200, 100, 0, 0);
            }
            else
            {
                _tracker.SendMessage("DONE BUTTON PRESSED (NOT DONE)");
                TakePicture("doneButton(notDone)");
                // change margin so that it is visible
                System.Windows.Controls.Panel.SetZIndex(_notDonePanel, 100);
                _notDonePanel.Margin = new Thickness(200, 100, 0, 0);
            }
        }

        // OK-Button nachdem Fertig-Button gedrückt wurde und alle Wünsche erfüllt sind
        // -> zum nächsten Screen weiterleiten
        private void Done_Continue_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Skip = true;
        }

        // OK-Button nachdem Fertig-Button gedrückt wurde und noch nicht alle Wünsche erfüllt sind
        // -> zurück zur aktuellen Aufgabe
        private void Done_Back_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (UIElement uie in MyCanvas.Children)
            {
                uie.Opacity = 1;
                if (uie is Ellipse && !(uie as Ellipse).Name.Equals("table"))
                    uie.Opacity = 0.8;
            }
            System.Windows.Controls.Panel.SetZIndex(_notDonePanel, 100);
            _notDonePanel.Margin = new Thickness(2000, 1000, 0, 0);
            _infoShown = false;
        }

        private void Help_Wanted_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tracker.SendMessage("HELP WANTED");

            _firstHintWindow.Margin = new Thickness(2000, 1000, 0, 0);

            _secondHintWindow.Margin = new Thickness(20, 490, 0, 0);
            System.Windows.Controls.Panel.SetZIndex(_secondHintWindow, 100);
        }

        private void Help_Not_Wanted_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tracker.SendMessage("HELP NOT WANTED");

            _firstHintWindow.Margin = new Thickness(2000, 1000, 0, 0);
        }

        private void Helpful_Button_MouseDown(object sender, RoutedEventArgs e)
        {
            _tracker.SendMessage("HELPFUL");

            _secondHintWindow.Margin = new Thickness(2000, 4900, 0, 0);
        }

        private void Not_Helpful_Button_MouseDown(object sender, RoutedEventArgs e)
        {
            _tracker.SendMessage("NOT HELPFUL");

            _secondHintWindow.Margin = new Thickness(2000, 4900, 0, 0);
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
            if (_noClickTimer != null && _hintModus == 1)
            {
                _noClickTimer.Stop();
                _noClickTimer.Interval = TimeSpan.FromSeconds(15);
                _noClickTimer.Tick += ShowHint;
                _noClickTimer.Start();
            }
            if (_current.InputElement == null) return;
            _current.X = Mouse.GetPosition((IInputElement)sender).X;
            _current.Y = Mouse.GetPosition((IInputElement)sender).Y;
            _current.InputElement.CaptureMouse();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_current.InputElement != null)
            {
                _current.IsDragging = false;
                _current.InputElement.ReleaseMouseCapture();
                _current.InputElement = null;
                TakePicture("stopDragging");
                _tracker.SendMessage("STOP DRAGGING " + _currentCircle.GetName());
            }
            //var pos = e.GetPosition(MyCanvas);
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // if mouse is down when its moving, then it's dragging current
            if (e.LeftButton == MouseButtonState.Pressed)
                _current.IsDragging = true;

            if (!_current.IsDragging || _current.InputElement == null) return;
            // Retrieve the current position of the mouse.
            var newX = Mouse.GetPosition((IInputElement)sender).X;
            var newY = Mouse.GetPosition((IInputElement)sender).Y;

            // Reset the location of the object (add to sender's renderTransform
            // newPosition minus currentElement's position
            var rt = ((UIElement)_current.InputElement).RenderTransform;
            var offsetX = rt.Value.OffsetX;
            var offsetY = rt.Value.OffsetY;
            rt.SetValue(TranslateTransform.XProperty, offsetX + newX - _current.X);
            rt.SetValue(TranslateTransform.YProperty, offsetY + newY - _current.Y);

            // Update position of the mouse
            _current.X = newX;
            _current.Y = newY;

            //var mousePos = e.GetPosition(MyCanvas);
            _trial.UpdateAoi(_currentCircle);
            UpdatePosition();
        }

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _current.InputElement = (IInputElement)sender;
            foreach (Circle c in _persons.Where(c => c.GetName().Equals(((Ellipse)sender).Name)))
            {
                _currentCircle = c;
                break;
            }
            TakePicture("startDragging");
            _tracker.SendMessage("START DRAGGING " + _currentCircle.GetName());
        }

        private void UpdatePosition()
        {
            var el = _current.InputElement as Ellipse;
            if (el == null) return;
            var generalTransform1 = MyCanvas.TransformToVisual(el);
            if (generalTransform1.Inverse == null) return;
            var currentPoint = generalTransform1.Inverse.Transform(new System.Windows.Point(0, 0));
            var newCenterX = currentPoint.X + CircleRadius;
            var newCenterY = currentPoint.Y + CircleRadius;
            _currentCircle.UpdatePosition(new System.Windows.Point(newCenterX, newCenterY));
        }
    }

}
