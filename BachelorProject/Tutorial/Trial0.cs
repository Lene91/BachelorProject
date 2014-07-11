using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System;

namespace BachelorProject.Tutorial
{
    class Trial0 : ExampleExercise
    {
        public Trial0(double pupilSize)
            : base(pupilSize)
        {
            Id = 0;
            InitializeTutorial();
            _constraintsWithPersons.Add("c1", new Tuple<string, string>("Person2", "Person4"));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person3", "Person2"));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person1", "Person4"));
        }

        public override void CheckActualConstraints()
        {
            UpdateConstraint("c1", SittingNextToEachOther(P2, P4));
            UpdateConstraint("c2", SittingOn(P3, P2));
            UpdateConstraint("c3", SharingFood(P1,P4));

            //return constraintsFullfilled;
        }

        public void InitializeTutorial()
        {
            var b = new Border
            {
                BorderThickness = new Thickness(4),
                BorderBrush = Brushes.Black,
                Margin = new Thickness(780, 475, 0, 0) //550
            };

            var sv = new ScrollViewer
            {
                Width = 400, //600
                Height = 290, //235
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            var tb = new TextBlock
            {
                //Text = text,
                Background = Brushes.White,
                Padding = new Thickness(10),
                FontSize = 22,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.WrapWithOverflow
            };

            tb.Inlines.Add(new Run("Weitere Hinweise") { FontWeight = FontWeights.Bold, FontSize = 30 });
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(getTutorialText());
            tb.Inlines.Add(new Run("Jetzt kannst du das Tool noch ein wenig" +
                           " ausprobieren. Klicke auf den Fertig-Button, wenn du" +
                           " die Aufgabe gelöst hast. Dein Platzierungsvorschlag" +
                           " wird dann geprüft und wenn du alle Sitzwünsche erfüllt hast," +
                           " gelangst du zu einer weiteren Übungsaufgabe." +
                           " Sind noch nicht alle Wünsche erfüllt, so erhälst du die" + 
                           " Möglichkeit, deine Platzierung noch einmal zu überarbeiten.") 
                           { FontStyle = FontStyles.Italic, FontSize = 22 });

            sv.Content = tb;
            b.Child = sv;
            MyCanvas.Children.Add(b);
        }

        private string getTutorialText()
        {
            const string tutorialText = "Hier siehst du nun alle Komponenten, die" +
                                        " du benötigst, um eine Aufgabe zu erfüllen. \n \n Die Aufgabe" +
                                        " kann nur gelöst werden, wenn" +
                                        " alle Personen am Tisch sitzen. \n \n Möchten zwei Personen nebeneinander" +
                                        " sitzen, so darf sich keine Person zwischen ihnen befinden." +
                                        " Sitzen beispielsweise nur zwei Personen am Tisch, so sitzen diese immer" +
                                        " nebeneinander. \n \n Wenn sich zwei Personen ein Essen teilen" +
                                        " möchten, müssen sie am Tisch sitzen und die Kreise der Personen" +
                                        " müssen sich überschneiden. Es können sich nicht mehr als zwei Personen ein Essen teilen," +
                                        " es ist jedoch möglich, dass es mehrere Paare gibt, die sich jeweils zu zweit ein" +
                                        " Essen teilen möchten. \n \n An der Feier nehmen auch ein paar Kleinkinder teil," +
                                        " die keinen eigenen Stuhl brauchen, sondern auf dem Schoß eines Elternteils platziert" +
                                        " werden müssen. Möchte ein Kind auf dem Schoß einer" +
                                        " Person sitzen, so musst du den Kreis des Kindes auf den der Person ziehen." +
                                        " Der Kreis des Kindes verkleinert sich dann" +
                                        " automatisch. Sitzt ein Kind auf dem Schoß eines Erwachsenen, so sitzen die beiden nicht" +
                                        " nebeneinander, haben jedoch die gleichen Nachbarn. \n \n" +
                                        " Eine Person kann sich entweder ein Essen teilen oder bei einer anderen auf dem Schoß sitzen" +
                                        " bzw. jemanden auf dem Schoß sitzen haben. Beides gleichzeitig ist nicht vorgesehen.\n \n" +
                                        " Du kannst alle deine Änderungen rückgängig machen," +
                                        " indem du auf den Reset-Button klickst. \n \n" +
                                        "Wenn du keine Lösung findest, kannst" +
                                        " du auf den Weiter-Button klicken und du gelangst zur nächsten Aufgabe. \n \n";
            return tutorialText;
        }
    }
}
