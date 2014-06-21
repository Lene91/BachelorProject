using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BachelorProject.Helper;

namespace BachelorProject.Tutorial
{
    /// <summary>
    /// Interaktionslogik für IntroScreen2.xaml
    /// </summary>
    public partial class IntroScreen2 : UserControl
    {
        private Element current = new Element();

        public IntroScreen2()
        {
            InitializeComponent();
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
        }
    }
}
