using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Documents;

namespace BachelorProject
{
    class Trial0 : ExampleExercise
    {
        public Trial0()
            : base()
        { 
            id = 0;
            initializeTutorial();
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

        public void initializeTutorial()
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
                //Text = text,
                Background = Brushes.White,
                Padding = new Thickness(10),
                FontSize = 25,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.WrapWithOverflow
            };

            tb.Inlines.Add(new Run("Weitere Hinweise") { FontWeight = FontWeights.Bold, FontSize = 40 });
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(getTutorialText());
            tb.Inlines.Add(new Run("Jetzt kannst du das Tool noch ein wenig" +
                           " ausprobieren. Wenn du alle Wünsche in dieser Aufgabe" +
                           " erfüllt hast, gelangst du automatisch zu ein paar" +
                           " weiteren Übungsaufgaben.") 
                           { FontStyle = FontStyles.Italic, FontSize = 25 });

            sv.Content = tb;
            b.Child = sv;
            MyCanvas.Children.Add(b);
        }

        private string getTutorialText()
        {
            string tutorialText = "Hier siehst du nun alle Komponenten, die" +
                " du benötigst, um eine Aufgabe zu erfüllen. \n \n Die Aufgabe" +
                " kann nur gelöst werden, wenn" +
                " alle Personen am Tisch sitzen. Platziere dafür den Kreis" +
                " einer Person nah am Tisch, sodass die Ränder sich" +
                " überschneiden. \n \n Möchten zwei Personen nebeneinander" +
                " sitzen, so darf sich keine Person zwischen ihnen befinden." +
                " Sitzen nur zwei Personen am Tisch, so sitzen diese immer" +
                " nebeneinander. \n \n Wenn sich zwei Personen ein Essen teilen" +
                " möchten, müssen sie am Tisch sitzen und die Kreise der Personen" +
                " müssen sich überschneiden. \n \n Möchte eine Person auf dem Schoß einer" +
                " anderen sitzen, so musst du die eine Person auf die andere ziehen." +
                " Der Kreis der Person, die auf dem Schoß sitzt, verkleinert sich" +
                " automatisch. \n \n Du kannst alle deine Änderungen rückgängig machen," +
                " indem du auf den Reset-Button klickst. \n \n";
            return tutorialText;
        }
    }
}
