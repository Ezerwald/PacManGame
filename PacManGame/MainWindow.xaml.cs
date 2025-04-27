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

        int ghostSpeed = 10;
        int ghostMoveStep = 130;
        int currentGhostStep;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void GameSetup()
        {

        }

        private void GameOver()
        {

        }
    }
}
