using ShapeShifter.Library;
using ShapeShifter.Library.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ShapeShifter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        int _index = 0;

        public MainPage()
        {
            this.InitializeComponent();
            this.Focus(FocusState.Keyboard);
            this.Loaded += MainPage_Loaded;
            btn_replay.Click += Btn_replay_Click;
            gamefield.NoAvailableSpace += Gamefield_NoAvailableSpace;
            gamefield.ScoreChanged += Gamefield_ScoreChanged;
            gamefield.LevelChanged += Gamefield_LevelChanged;
            gamefield.NextShapeCalculated += Gamefield_NextShapeCalculated;
            this.KeyDown += MainPage_KeyDown;
            this.KeyUp += MainPage_KeyUp;

        }

        

        private void Btn_replay_Click(object sender, RoutedEventArgs e)
        {
            btn_replay.Visibility = Visibility.Collapsed;
            gamefield.Reset();
            gamefield.StartGame();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            gamefield.StartGame();
        }

        async private void Gamefield_NoAvailableSpace()
        {
            MessageDialog md = new MessageDialog("Game Over!");
            await md.ShowAsync();
            btn_replay.Visibility = Visibility.Visible;
        }

        private void Gamefield_LevelChanged()
        {
            txt_level.Text = $"Level: {gamefield.CurrentLevel}";
        }

        private void Gamefield_ScoreChanged()
        {
            txt_score.Text = $"Score: {gamefield.Score}";
        }



        private void MainPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                    gamefield.ShiftActiveShapeLeft();
                    break;
                case Windows.System.VirtualKey.Right:
                    gamefield.ShiftActiveShapeRight();
                    break;
                case Windows.System.VirtualKey.Up:
                    gamefield.RotateActiveShape();
                    break;
                case Windows.System.VirtualKey.Down:
                    gamefield.TurboActiveShape();
                    break;
            }
            
        }

        private void MainPage_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Down:
                    gamefield.EndActiveShapeTurbo();
                    break;
            }
        }

        private void Gamefield_NextShapeCalculated()
        {
            control_preview.PreviewShape(gamefield.CurrentPreviewShape);
        }

        


        

    }
}
