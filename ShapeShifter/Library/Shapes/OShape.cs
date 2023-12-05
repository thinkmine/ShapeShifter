using ShapeShifter.UI.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ShapeShifter.Library
{
    public class OShape : Shape
    {
        public OShape()
        {
            ShapeAxis = 0;
            ShapeBackground = new SolidColorBrush(Colors.Purple);
        }

        public override bool CanBeAdded(GameField field)
        {
            if (!field.Blocks[Column, Row].IsOccupied &&
            !field.Blocks[Column + 1, Row].IsOccupied &&
            !field.Blocks[Column, Row + 1].IsOccupied &&
            !field.Blocks[Column + 1, Row + 1].IsOccupied)
            {
                return true;
            }
            return false;
        }

        public override bool CanDescend(GameField field)
        {
            if (Row + 2 < field.GameHeight)
            {
                if (!field.Blocks[Column, Row + 2].IsOccupied &&
                !field.Blocks[Column + 1, Row + 2].IsOccupied)
                    return true;
            }

            return false;
        }

        public override bool CanRotate(GameField field)
        {
            return true;
        }

        public override bool CanShiftLeft(GameField field)
        {
            if (Column - 1 >= 0)
            {
                if (!field.Blocks[Column - 1, Row].IsOccupied &&
                !field.Blocks[Column - 1, Row + 1].IsOccupied)
                    return true;
            }

            return false;
        }

        public override bool CanShiftRight(GameField field)
        {
            if (Column + 2 < field.GameWidth)
            {
                if (!field.Blocks[Column + 2, Row].IsOccupied &&
                !field.Blocks[Column + 2, Row + 1].IsOccupied)
                    return true;
            }
            return false;
        }

        public override void Clear(GameField field)
        {
            field.Blocks[Column, Row].Clear();
            field.Blocks[Column + 1, Row].Clear();
            field.Blocks[Column, Row + 1].Clear();
            field.Blocks[Column + 1, Row + 1].Clear();
        }

        public override void Draw(GameField field)
        {
            field.Blocks[Column, Row].Occupy(this);
            field.Blocks[Column + 1, Row].Occupy(this);
            field.Blocks[Column, Row + 1].Occupy(this);
            field.Blocks[Column + 1, Row + 1].Occupy(this);
        }

        public override void Wedge(GameField field)
        {
            field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
            {
                Column = this.Column,
                BlockColor = this.ShapeBackground,

            });
            field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
            {
                Column = this.Column + 1,
                BlockColor = this.ShapeBackground,

            });

            field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
            {
                Column = this.Column,
                BlockColor = this.ShapeBackground,

            });

            field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
            {
                Column = this.Column + 1,
                BlockColor = this.ShapeBackground,

            });
        }
    }

}
