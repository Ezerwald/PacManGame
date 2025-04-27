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

/// Timer
using System.Windows.Threading;

namespace PacManGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();

        bool goLeft, goRight, goUp, goDown;
        bool noLeft, noRight, noUp, noDown;

        int speed = 8;

        Rect pacmanHitBox;

        int ghostSpeed = 10;
        int ghostMoveStep = 130;
        int currentGhostStep;
        int score = 0;

        int coinCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            GameSetUp();
        }


        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Right && noRight == false)
            {
                goLeft = goUp = goDown = false;
                noLeft = noUp = noDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Up && noUp == false)
            {
                goLeft = goRight = goDown = false;
                noLeft = noRight = noDown = false;

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && noDown == false)
            {
                goLeft = goRight = goUp = false;
                noLeft = noRight = noUp = false;

                goDown = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
        }


        private void GameSetUp()
        {
            MyCanvas.Focus();

            timer.Tick += GameLoop;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Start();
            currentGhostStep = ghostMoveStep;

            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;

            foreach (UIElement element in MyCanvas.Children)
            {
                if (element is Shape shape && (string)shape.Tag == "coin")
                {
                    coinCount++;
                }
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {

            txtScore.Content = "Score: " + score;

            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }

            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }

            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }

            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }


            /// Window border collision
            if (goRight && Canvas.GetLeft(pacman) + 50 > Application.Current.MainWindow.Width) 
            {
                noRight = true;
                goRight = false;
            }

            if (goLeft && Canvas.GetLeft(pacman) < 9)
            {
                noLeft = true;
                goLeft = false;
            }

            if (goUp && Canvas.GetTop(pacman) < 7)
            {
                noUp = true;
                goUp = false;
            }

            if (goDown && Canvas.GetTop(pacman) + 75 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);


            /// Wall collision
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10); // Wall width = 10
                        noLeft = true;
                        goLeft = false;
                    }

                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }

                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }

                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                }

                /// Coin collection   
                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;

                    }
                }

                /// Ghost collision
                if ((string)x.Tag == "ghost")
                {
                    // check if pac man collides with the ghost 
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("Ghosts got you, click ok to play again!");
                    }

                    if (x.Name.ToString() == "orangeGuy")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    else if (x.Name.ToString() == "pinkGuy")
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) - 0.6 * ghostSpeed);
                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    currentGhostStep--;

                    if (currentGhostStep < 1)
                    {
                        currentGhostStep = ghostMoveStep;
                        ghostSpeed = -ghostSpeed;
                    }
                }
            }

            if (score == coinCount)
            {
                GameOver("You won! You collected all of the coins!");
            }

        }


        private void GameOver(string message)
        {
            timer.Stop();
            MessageBox.Show(message, "The Pac Man Game");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
