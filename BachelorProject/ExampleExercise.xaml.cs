using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private const int Computer = 2;     // 1 -> Laptop, 2 -> Lab, 3 -> Berlin

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

        protected ITracker _tracker;

        public bool Skip = false;

        private bool _constraintHelp;

        private bool _constraintDestroyed = false;

        private int _hintModus;

        private int _pictureId = 1;

        private bool _hintDelivered = false;

        private int _counter = 0;

        private bool _infoShown = false;

        private VirtualizingPanel _donePanel = new VirtualizingStackPanel();
        private VirtualizingPanel _notDonePanel = new VirtualizingStackPanel();
        private VirtualizingPanel _hintWindow = new VirtualizingStackPanel();
        private Run _hintRun = new Run();

        private bool _doneButtonKlicked;

        private DispatcherTimer _noClickTimer = new DispatcherTimer();
        private DispatcherTimer _resetHintTimer = new DispatcherTimer();
        private DispatcherTimer _constraintsVisitedTimer = new DispatcherTimer();
        private DispatcherTimer _hintTimer = new DispatcherTimer();

        private string _hint = "";
        private bool _hintThreadIsRunning = false;

        private string _toShow;
        private PointF _position;

        private IList<AreaOfInterest> _aois = new List<AreaOfInterest>();

        private Dictionary<int, bool> _constraintDict = new Dictionary<int, bool>();    // Zahl des Constraints und Angabe, ob erfüllt (true) oder nicht (false)
        private Dictionary<string, UIElement> _allConstraints = new Dictionary<string, UIElement>();
        private Dictionary<string, System.Drawing.Rectangle> _constraintAois = new Dictionary<string, System.Drawing.Rectangle>();

        private AreaOfInterest _constraintHighlighted = null;
        private AreaOfInterest _constraintTimerStarted = null;

        private DispatcherTimer _highlightTimer = new DispatcherTimer();

        protected Dictionary<string, Tuple<string,string>> _constraintsWithPersons = new Dictionary<string,Tuple<string,string>>();

        private static double _pupilSize;
        private object thisLock = new object();
        private double _pupilCounter = 1;
        private double _deltaPupilSize = 2.0;
        private double _borderPupilSize;
        private double _currentAvgPupilSize;

        private Dictionary<string,int> _visitedConstraints = new Dictionary<string,int>();
        private bool _constraintsVisited = false;

        private string _lastVisitedConstraint = null;

        private bool _lastConstraintFulfilled = false;

        private string _highlightedConstraint = null;

        private bool _firstRun = false;

        System.Windows.Media.Brush[] _allBrushes =
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

        private int _rnd;

        public ExampleExercise(double pupilSize)
        {
            InitializeComponent();
            _pupilSize = pupilSize;
        }

        /*
         * **********************************************************
         *                                                          *
         *              INITIALIZING INTERFACE                      *
         *                                                          *
         ************************************************************
         */

        public void Initialize(IAoiUpdate trial, int numberOfPersons, string constraints, List<string> names, ITracker tracker, bool constraintHelp, int hintModus, int rnd)
        {
            _trial = trial;
            _numberOfPersons = numberOfPersons;
            _constraints = constraints;
            _names = names;
            _tracker = tracker;
            _constraintHelp = constraintHelp;
            _hintModus = hintModus;
            _rnd = rnd;

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

        public void SendAois(IList<AreaOfInterest> aois)
        {
            _aois = aois;
        }

        public List<Circle> GetPersons()
        {
            return _persons;
        }

        public Dictionary<string, System.Drawing.Rectangle> GetConstraints()
        {
            return _constraintAois;
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
            /*System.Windows.Media.Brush[] _allBrushes =
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
            };*/

            foreach (System.Windows.Media.Brush b in _allBrushes)
                b.Freeze();

            
        }

        private void ShuffleColors()
        {
            var r = new Random(_rnd);
            while (_brushes.Count < _numberOfPersons)
            {
                var rNumber = r.Next(_allBrushes.Length);
                var brush = _allBrushes[rNumber];
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
                Width = 400,
                Height = 250,
                Margin = new Thickness(800, 50, 0, 0)
            };


            MyCanvas.Children.Add(_legendStackPanel);

            // Legendeneinträge bestehend aus Kreis und Name
            ShuffleColors();
            for (int i = 1; i <= _numberOfPersons; ++i)
            {
                var e = new Ellipse
                {
                    Name = "lc" + i,
                    Fill = _brushes[i - 1],
                    Opacity = 0.8,
                    Width = 40,
                    Height = 40,
                    StrokeThickness = 2,
                    Stroke = System.Windows.Media.Brushes.Black,
                    Margin = new Thickness(10, 10, 10, 10)
                };
                _legendStackPanel.Children.Add(e);

                var t = new TextBlock
                {
                    Name = "l" + i,
                    Text = _names[i - 1],
                    Background = System.Windows.Media.Brushes.Transparent,
                    FontSize = 20,
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
                _constraintDict.Add(index + 1, false);
                index++;
            }
            _singleConstraints = newConstraints;

            // Panel für Constraints
            _constraintStackPanel = new StackPanel
            {
                Name = "ConstraintPanel",
                Orientation = System.Windows.Controls.Orientation.Vertical,
                Width = 400,
                Height = 620,
                Margin = new Thickness(800, 180, 0, 0)
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
            var x = 830;
            var y = 238;
            var step = 68;
            var width = 360;
            var height = 60;
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
                    FontSize = 18,
                    MaxWidth = 360,
                    Height = 60,
                    Padding = new Thickness(10),
                    TextAlignment = System.Windows.TextAlignment.Center,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                constraintCounter++;
                b.Child = t;
                _constraintStackPanel.Children.Add(b);
                _allConstraints.Add(labelName, b);
                var r = new System.Drawing.Rectangle(x, y, width, height);
                _constraintAois.Add(labelName, r);
                y += step;
            }


            InitiateRedBrush();
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
            for (var i = 1; i <= _numberOfPersons; ++i)
            {
                newHint = newHint.Replace(i.ToString(), _names[i - 1]);
            }
            _hint = newHint;

            var constraintNumber = _singleConstraints.Count();
            for (var i = 1; i <= constraintNumber; ++i)
            {
                var c = "c" + i;
                _visitedConstraints.Add(c, 0);
            }
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

            /*foreach (UIElement uie in MyCanvas.Children)
            {
                if (uie is TextBlock && (uie as TextBlock).Name.Equals("tb"))
                {
                    (uie as TextBlock).Text = _position.ToString() + ", " + _toShow;
                    (uie as TextBlock).Margin = new Thickness(_position.X, _position.Y, 0, 0);
                }
            }*/

            //int counter = 20;
            if (_constraintsVisited)
            {
                if (_hintModus == 7)
                {
                    _constraintsVisitedTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                    _constraintsVisitedTimer.Tick += (s, args) => ShowHint();
                    //_constraintsVisitedTimer.Start();
                }
            }


            bool _aoiHit = false;
            foreach (var aoi in _aois)
            {
                if (aoi.Points[0].X < _position.X && aoi.Points[1].X > _position.X && aoi.Points[0].Y < _position.Y &&
                    aoi.Points[1].Y > _position.Y)
                {
                    if (_lastConstraintFulfilled && aoi.Name.Equals("rConstraints"))
                    {
                        _lastConstraintFulfilled = false;
                        _lastVisitedConstraint = null;
                        _hintTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
                        
                        if (_hintModus == 9)
                            _hintTimer.Tick += Highlight;
                        else if (_hintModus == 8)
                            _hintTimer.Tick += ShowHint;

                        _hintTimer.Start();
                    }
                    if (aoi.Name.StartsWith("c") && (_hintModus == 4 || _hintModus == 5 || _hintModus == 8 || _hintModus == 9))
                    {   // Constraint ausgewählt und soll gehighlighted werden
                        _aoiHit = true;
                        if (_constraintHighlighted == null && _constraintTimerStarted == null)
                        {   // noch nichts passiert
                            StartTimer(aoi, aoi);
                        }
                        else if (_constraintHighlighted == null)
                        {   // Timer wurde bereits gestartet
                            if (aoi != _constraintTimerStarted)
                            { // neuer constraint --> alten timer stoppen und neuen starten
                                if (_highlightTimer != null)
                                    _highlightTimer.Stop();
                                StartTimer(aoi, aoi);
                            }
                            // else gleicher constraint --> nichts zu tun
                        }
                        else
                        {   // Timer wurde bereits gestartet und constraint bereits highlighted   
                            if (aoi != _constraintTimerStarted)
                            {   // neuer constraint --> timer bei Bedarf stoppen (wenn noch nicht in UpdateHighlighting-Methode gewesen) und neuen starten; aber nur wenn aoi nicht bereits highlighted  
                                if (_highlightTimer != null)
                                    _highlightTimer.Stop();
                                _constraintTimerStarted = null;
                                if (aoi != _constraintHighlighted)
                                {
                                    StartTimer(_constraintHighlighted, aoi);
                                }
                            }
                            // else gleicher constraint --> nichts zu tun
                        }
                    }



                    /*
                    Border b = GetConstraint("c1");
                    TextBlock tb = b.Child as TextBlock;
                    tb.Text = aoi.Name;
                    */
                }
            }
            if (_highlightTimer != null && !_aoiHit)
            {
                _highlightTimer.Stop();
                _constraintTimerStarted = null;
            }

            if (!_constraintsVisited)
            {
                var visited = true;
                foreach (var entry in _visitedConstraints)
                {
                    if (entry.Value < 2)
                        visited = false;
                }
                _constraintsVisited = visited;
            }

            if (_hintModus == 6 && Id != 1002)
            {
                _borderPupilSize = _pupilSize + _deltaPupilSize;
                if (_currentAvgPupilSize > _borderPupilSize)
                    //Show(new PointF(100, 100), "große Pupille");
                    ;
                else
                    _deltaPupilSize -= 0.01;
            }

            if (_counter == 300)
            {
                // Mauskoordinaten speichern
                var pos = Mouse.GetPosition(null);
                _tracker.SendMessage("MousePos " + pos.ToString());
                _counter = 0;
            }
            _counter++;

            if (_constraintHelp || _doneButtonKlicked || _hintModus == 3 || _hintModus == 8 || _hintModus == 9)
            {
                // Sitzplatzbeziehungen prüfen
                CalculateSittingOrder();
                //InitiateRedBrush();

                constraintsFullfilled = true;

                foreach (var p in _persons.Where(p => !p.Touches(table) && p.GetSeat() == null))
                    constraintsFullfilled = false;
                CheckActualConstraints();
                foreach (KeyValuePair<int, bool> kvp in _constraintDict)
                {
                    if (!kvp.Value)
                        constraintsFullfilled = false;
                }
            }

            if (_currentCircle == null) return;
            foreach (var p in _persons.Where(p => !_currentCircle.Equals(p)))
            {
                _currentCircle.CheckSitting(p);
            }
        }

        private void ShowHint(object sender, EventArgs e)
        {
            
            ShowHint();
        }

        private void Highlight(object sender, EventArgs e)
        {
            Highlight();
        }

        private void StartTimer(AreaOfInterest oldC, AreaOfInterest newC)
        {
                _highlightTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };
                _highlightTimer.Tick += (s, args) => UpdateHighlighting(oldC, newC);
                _highlightTimer.Start();
                _constraintTimerStarted = newC;
        }

        protected virtual void CheckActualConstraints()
        {
            //return false;
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

        protected bool NotSittingOn(Circle c1, Circle c2) // c1 darf nicht auf dem Schoß von c2 sitzen
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

        public void ShowHint()
        {
            if (_hintTimer.IsEnabled)
            {
                _hintTimer.Stop();
                _hintTimer.Tick -= ShowHint;
            }
            //if (_hintDelivered) return;
            if (_infoShown)
            {
                var hintThread = new Thread(() => CheckInfoShown());
                _hintThreadIsRunning = true;
                hintThread.Start();
                return;
            }
            if (_constraintsVisitedTimer.IsEnabled)
            {
                _constraintsVisitedTimer.Stop();
            }
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ShowHint());
                return;
            }

            foreach (var b in MyCanvas.Children.OfType<VirtualizingPanel>().Select(uie => uie))
            {
                if (b.Name.Equals("HintWindow"))
                    _hintWindow = b;
            }

            string hint = "";
            foreach (var tb in _hintWindow.Children.OfType<Border>().Select(x => x).Select(bo => bo.Child as TextBlock))
            {
                if (tb == null) return;
                var nextConst = GetNextConstraintNumber();
                if (nextConst < 0) return;
                hint = _singleConstraints[GetNextConstraintNumber()-1];

                tb.Inlines.Clear();
                tb.Inlines.Add(new Run
                {
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    Text = "Tipp"
                });
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new Run { Text = hint });
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new LineBreak());
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
                    Content = "OK"
                };
                btn3.Click += Ok_Button_MouseDown;
                tb.Inlines.Add(btn3);
            }

            _hintWindow.Margin = new Thickness(200, 200, 0, 0);
            System.Windows.Controls.Panel.SetZIndex(_hintWindow, 100);
            _tracker.SendMessage("HINT (WINDOW) SHOWN - " + hint);
            _hintDelivered = true;
        }

        private int GetNextConstraintNumber()
        {
            // finde nächsten noch nicht erfüllten Constraint (random)
            int hint = -1;
            var listWithFalseConstraints = new List<int>();
            foreach (var entry in _constraintDict)
            {
                if (!entry.Value)
                    listWithFalseConstraints.Add(entry.Key);
            }
            var shuffledListWithFalseConstraints = Shuffle(listWithFalseConstraints);

            if (shuffledListWithFalseConstraints.Count > 0)
                hint = shuffledListWithFalseConstraints[0];

            //_lastVisitedConstraint = "c" + hint;
            return hint;
        }

        private static List<int> Shuffle(List<int> l)
        {
            var n = l.Count;
            var rnd = new Random();
            while (n > 1)
            {
                var k = (rnd.Next(0, n) % n);
                n--;
                var value = l[k];
                l[k] = l[n];
                l[n] = value;
            }
            return l;
        }

        private void CheckInfoShown()
        {
            while (_hintThreadIsRunning)
            {
                if (!_infoShown)
                {
                    ShowHint();
                    _hintThreadIsRunning = false;
                }
            }
        }

        public void StartNoClickTimer()
        {
            _noClickTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(20) };
            _noClickTimer.Tick += (s,args) => ShowHint();
            //_noClickTimer.Start();
        }

        private void Highlight()
        {
            if (_hintTimer.IsEnabled)
            {
                _hintTimer.Stop();
                _hintTimer.Tick -= Highlight;
            }

            var number = GetNextConstraintNumber();
            var name = "c" + number;
            if (!name.Equals(_highlightedConstraint))
                DeHighlight(_highlightedConstraint);
            if (number > 0)
            {
                (_allConstraints[name] as Border).Background = System.Windows.Media.Brushes.LightYellow;
                //(_allConstraints[name] as Border).Background = System.Windows.Media.Brushes.Black;
                //((_allConstraints[name] as Border).Child as TextBlock).Foreground = System.Windows.Media.Brushes.LightGray;
                _highlightedConstraint = name;
                _tracker.SendMessage("HINT (HIGHLIGHT) SHOWN - " + ((_allConstraints[name] as Border).Child as TextBlock).Text);
            }
            
 
        }

        private void DeHighlight(string name)
        {
            if (name != null)
            {
                (_allConstraints[name] as Border).Background = System.Windows.Media.Brushes.Transparent;
                //((_allConstraints[name] as Border).Child as TextBlock).Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void UpdateHighlighting(AreaOfInterest oldC, AreaOfInterest newC)
        {
            _highlightTimer.Stop();
            _constraintTimerStarted = null;

            if (oldC.Name != newC.Name)
                DeHighlightConstraint(oldC.Name);

            HighlightConstraint(newC.Name);
            _constraintHighlighted = newC;

            _visitedConstraints[newC.Name]++;
        }

        private void HighlightConstraint(string name)
        {
            _lastVisitedConstraint = name;
            if (_hintModus == 4 || _hintModus == 5)
                (_allConstraints[name] as Border).Background = System.Windows.Media.Brushes.LightGreen;
            if (_hintModus == 5)
            {
                Tuple<string, string> tuple = _constraintsWithPersons[name];
                foreach (var circle in _circles)
                {
                    if (tuple.Item1 != null && circle.Name.Equals(tuple.Item1))
                        circle.Stroke = System.Windows.Media.Brushes.LightGreen;
                    else if (tuple.Item2 != null && circle.Name.Equals(tuple.Item2))
                        circle.Stroke = System.Windows.Media.Brushes.LightGreen;
                }
            }

        }

        private void DeHighlightConstraint(string name)
        {
            if(_hintModus == 4 || _hintModus == 5)
                (_allConstraints[name] as Border).Background = System.Windows.Media.Brushes.Transparent;
            if (_hintModus == 5)
            {
                Tuple<string, string> tuple = _constraintsWithPersons[name];
                foreach (var circle in _circles)
                {
                    if (tuple.Item1 != null && circle.Name.Equals(tuple.Item1))
                        circle.Stroke = System.Windows.Media.Brushes.Black;
                    else if (tuple.Item2 != null && circle.Name.Equals(tuple.Item2))
                        circle.Stroke = System.Windows.Media.Brushes.Black;
                }
            }
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
            var constraintNumber = Int32.Parse(name[1].ToString());

            if (fulfilled && constraintNumber > 0)
                DeHighlight("c"+constraintNumber);
            if (_lastVisitedConstraint != null && _lastVisitedConstraint.Equals(name) && fulfilled &&
                !_constraintDict[constraintNumber])
                _lastConstraintFulfilled = true;

            if (!fulfilled && _constraintDict[constraintNumber])
            {
                _constraintDict[constraintNumber] = false;
                _constraintDestroyed = true;
            }

            if (fulfilled && !_constraintDict[constraintNumber])
                _constraintDict[constraintNumber] = true;


            if (!_constraintHelp) return;

            var b = GetConstraint(name);
            var tb = b.Child as TextBlock;
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
                var name = "c" + i;
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



            string fileName;
            long time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Graphics g;
            Bitmap screenshot;

            // snip wanted area
            if (Computer == 1)
            {
                screenshot = new Bitmap((int)Width, (int)Height - 30);
                g = Graphics.FromImage(screenshot);
                g.CopyFromScreen(80, 0, 0, 0, new System.Drawing.Size((int)this.Width, (int)this.Height), CopyPixelOperation.SourceCopy);
                fileName = "C:\\Users\\Lene\\Desktop\\BA\\Daten\\" + TestPerson + "\\" + time + "-Trial" + Id + "-" + _pictureId + "-" + info + ".jpg";
                mousePos.X = mousePos.X - 80;
            }
            else if (Computer == 2)
            {
                screenshot = new Bitmap((int)Width, (int)Height - 30);
                g = Graphics.FromImage(screenshot);
                g.CopyFromScreen(360, 210, 0, 0, new System.Drawing.Size((int)this.Width + 50, (int)this.Height + 50), CopyPixelOperation.SourceCopy);
                fileName = "C:\\Users\\lganschow\\Documents\\Daten\\" + TestPerson + "\\" + time + "-Trial" + Id + "-" + _pictureId + "-" + info + ".jpg";
                mousePos.X = mousePos.X - 360;
                mousePos.Y = mousePos.Y - 210;
            }
            else if (Computer == 3)
            {
                screenshot = new Bitmap((int)Width, (int)Height - 10);
                g = Graphics.FromImage(screenshot);
                g.CopyFromScreen(360, 150, 0, 0, new System.Drawing.Size((int)this.Width + 50, (int)this.Height + 50), CopyPixelOperation.SourceCopy);
                fileName = "C:\\Users\\Ganschow\\Documents\\Lene\\Daten\\" + TestPerson + "\\" + time + "-Trial" + Id + "-" + _pictureId + "-" + info + ".jpg";
                mousePos.X = mousePos.X - 360;
                mousePos.Y = mousePos.Y - 150;
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
            _infoShown = true;
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
        *              PUPIL SIZE                                  *
        *                                                          *
        ************************************************************
        */

        public void SendCurrentPupilSize(double left, double right)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SendCurrentPupilSize(left, right));
                return;
            }

            double avg = (left + right) / 2;
            _currentAvgPupilSize = avg;
        }

        public void UpdatePupilSize(double left, double right)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdatePupilSize(left, right));
                return;
            }

            lock (thisLock)
            {
                //if (_counter == 0)
                //pupilSize = 0;
                if (left != 0 && right != 0)
                {
                    double avg = (left + right) / 2.0;
                    _pupilSize += avg;

                    if (_pupilCounter > 2)
                        _pupilSize = _pupilSize / 2;
                    else
                    {
                        _pupilSize = _pupilSize / _pupilCounter;
                        _pupilCounter++;
                    }
                }
            }
        }

        /*public void SetPupilSize(double size)
        {
            _pupilSize = size;
        }*/

        public double GetPupilSize()
        {
            if (!Dispatcher.CheckAccess())
            {
                double result = Dispatcher.Invoke(() => GetPupilSize());
                return result;
            }

            return _pupilSize;
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
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Reset_Button_MouseDown(sender, e));
                return;
            }

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
            /*if (_hintModus == 2)
            {
                _resetHintTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
                _resetHintTimer.Tick += ShowHint;
                _resetHintTimer.Start();
            }*/
        }

        private void Continue_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Continue_Button_MouseDown(sender, e));
                return;
            }

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
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Done_Button_MouseDown(sender, e));
                return;
            }

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
                    //else uie.Opacity = 0.2;
                }
                //else uie.Opacity = 0.2;
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
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Done_Back_Button_MouseDown(sender, e));
                return;
            }

            /*foreach (UIElement uie in MyCanvas.Children)
            {
                uie.Opacity = 1;
                if (uie is Ellipse && !(uie as Ellipse).Name.Equals("table"))
                    uie.Opacity = 0.8;
            }*/
            System.Windows.Controls.Panel.SetZIndex(_notDonePanel, 100);
            _notDonePanel.Margin = new Thickness(2000, 1000, 0, 0);
            _infoShown = false;

            if (_noClickTimer.IsEnabled)
            {
                _noClickTimer.Stop();
                StartNoClickTimer();
            }
        }

        /*private void Help_Wanted_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tracker.SendMessage("HELP WANTED");

            _firstHintWindow.Margin = new Thickness(2000, 1000, 0, 0);

            _secondHintWindow.Margin = new Thickness(20, 490, 0, 0);
            System.Windows.Controls.Panel.SetZIndex(_secondHintWindow, 100);
        }

        private void Help_Not_Wanted_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tracker.SendMessage("HELP NOT WANTED");

            if (_hintModus == 1)
            {
                _noClickTimer.Stop();
                StartNoClickTimer();
            }
            _hintDelivered = false;

            _firstHintWindow.Margin = new Thickness(2000, 1000, 0, 0);
        }
        `*/

        private void Ok_Button_MouseDown(object sender, RoutedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Ok_Button_MouseDown(sender, e));
                return;
            }

            _tracker.SendMessage("HINT CLOSED");

            _hintWindow.Margin = new Thickness(2000, 4900, 0, 0);
        }

        /*
        private void Not_Helpful_Button_MouseDown(object sender, RoutedEventArgs e)
        {
            _tracker.SendMessage("NOT HELPFUL");

            _hintWindow.Margin = new Thickness(2000, 4900, 0, 0);
        }*/



        /*
         * **********************************************************
         *                                                          *
         *              REALIZING DRAGGING                          *
         *                                                          *
         ************************************************************
         */


        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Canvas_MouseDown(sender, e));
                return;
            }

            if (_noClickTimer.IsEnabled && _hintModus == 1)
            {
                _noClickTimer.Stop();
                _noClickTimer.Tick -= ShowHint;
                StartNoClickTimer();
            }
            if (_current.InputElement == null) return;
            _current.X = Mouse.GetPosition((IInputElement)sender).X;
            _current.Y = Mouse.GetPosition((IInputElement)sender).Y;
            _current.InputElement.CaptureMouse();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Canvas_MouseUp(sender, e));
                return;
            }

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
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Canvas_MouseMove(sender, e));
                return;
            }

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
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ellipse_MouseLeftButtonDown(sender, e));
                return;
            }

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
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(UpdatePosition);
                return;
            }

            var el = _current.InputElement as Ellipse;
            if (el == null) return;
            var generalTransform1 = MyCanvas.TransformToVisual(el);
            if (generalTransform1.Inverse == null) return;
            var currentPoint = generalTransform1.Inverse.Transform(new System.Windows.Point(0, 0));
            var newCenterX = currentPoint.X + CircleRadius;
            var newCenterY = currentPoint.Y + CircleRadius;
            _currentCircle.UpdatePosition(new System.Windows.Point(newCenterX, newCenterY));
        }

        public void Show(PointF p, String s)
        {
            
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Show(p,s));
                return;
            }

            if (!_firstRun)
            {
                //MyCanvas.Children.Add(new Ellipse() { Name = "ellipse", Fill = System.Windows.Media.Brushes.Black, Margin = new Thickness(p.X, p.Y, 0, 0), Width = 20, Height = 20 });
                MyCanvas.Children.Add(new TextBlock() { Name = "tb", Margin = new Thickness(p.X, p.Y, 0, 0), Text = p.ToString() });
                _firstRun = true;
            }
            foreach (var child in MyCanvas.Children)
            {
                //if (child is Ellipse && (child as Ellipse).Name.Equals("ellipse"))
                //    (child as Ellipse).Margin = new Thickness(p.X, p.Y, 0, 0);
                if (child is TextBlock && (child as TextBlock).Name.Equals("tb"))
                {
                    (child as TextBlock).Margin = new Thickness(p.X, p.Y, 0, 0);
                    (child as TextBlock).Text = p.ToString();
                }
            }

            _position = p;
            _toShow = s;
        }

        public void UpdatePos(PointF p)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdatePos(p));
                return;
            }

            _position = new PointF(p.X,p.Y);
        }
    }

}
