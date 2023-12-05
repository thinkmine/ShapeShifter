using ShapeShifter.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShapeShifter.UI.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Block : Page
    {
        GameField Field { get; set; }
        Shape OccupyingShape { get; set; }
        public bool IsOccupied { get; set; }
        public Block(GameField field)
        {
            this.InitializeComponent();
            this.Field = field;
        }

        public void Clear(Shape shape = null)
        {
            IsOccupied = false;
            LayoutRoot.Background = Field.Background;
            OccupyingShape = null;
        }

        public void Occupy(Shape shape)
        {
            IsOccupied = true;
            LayoutRoot.Background = shape.ShapeBackground;
            OccupyingShape = shape;
        }

        public void Occupy(BlockInfo block_info)
        {
            LayoutRoot.Background = block_info.BlockColor;
            IsOccupied = true;
        }
    }
}
