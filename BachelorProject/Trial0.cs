using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace BachelorProject
{
    class Trial0 : ExampleExercise
    {
        public Trial0(string text)
            : base()
        { 
            id = 0;
            initializeTutorial(text);
        }

        public override bool checkActualConstraints()
        {
            if (sittingNextToEachOther(p2, p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingOn(p3, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sharingFood(p1,p4))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            return constraintsFullfilled;
        }

        public void initializeTutorial(string text)
        {
            Border b = new Border
            {
                BorderThickness = new Thickness(4),
                BorderBrush = Brushes.Black,
                Margin = new Thickness(290, 460, 0, 0)
            };

            ScrollViewer sv = new ScrollViewer
            {

                Width = 520,
                Height = 300,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            TextBlock tb = new TextBlock
            {
                Text = text,
                Background = Brushes.White,
                Padding = new Thickness(10),
                FontSize = 30,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.WrapWithOverflow
            };
            sv.Content = tb;
            b.Child = sv;
            MyCanvas.Children.Add(b);
        }

        public override void setTutorialText(string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => setTutorialText(text));
                return;
            }

            foreach (UIElement e in MyCanvas.Children)
            {
                if (e is Border)
                {
                    Border b = e as Border;
                    ScrollViewer sv = b.Child as ScrollViewer;
                    TextBlock tb = sv.Content as TextBlock;
                    tb.Text = text;
                }
            }
        }
    }
}
