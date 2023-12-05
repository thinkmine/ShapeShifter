using ShapeShifter.UI.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Gaming.Input;
using System.IO;
using Windows.Graphics.DirectX;

namespace ShapeShifter.Library;

public class IShape : Shape
{
    public IShape()
    {
        ShapeAxis = 0;
        ShapeBackground = new SolidColorBrush(Colors.DarkGray);
    }

    public override bool CanBeAdded(GameField field)
    {
        if (!field.Blocks[Column, Row - 1].IsOccupied &&
        !field.Blocks[Column, Row].IsOccupied &&
        !field.Blocks[Column, Row + 1].IsOccupied &&
        !field.Blocks[Column, Row + 2].IsOccupied)
        {
            return true;
        }
        return false;
    }

    public override bool CanDescend(GameField field)
    {
        switch (ShapeAxis)
        {
            case 0:
            case 180:
                if (field.Blocks[Column, Row + 3].IsOccupied == false)
                {
                    return true;
                }
                break;

            case 90:
            case 270:
                if (field.Blocks[Column - 1, Row + 1].IsOccupied == false &&
                    field.Blocks[Column, Row + 1].IsOccupied == false &&
                        field.Blocks[Column + 1, Row + 1].IsOccupied == false &&
                        field.Blocks[Column + 2, Row + 1].IsOccupied == false)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    public override bool CanRotate(GameField field)
    {
        //if horizontal then I need to know that when vertical no spaces are occupied
        //if vertical then ensure none of the potential horizontal spaces are occupied
        switch (ShapeAxis)
        {
            case 0:
            case 180:
                if (Column - 1 >= 0 && Column + 2 < field.GameWidth)
                    if (field.Blocks[Column - 1, Row].IsOccupied == false &&
                        field.Blocks[Column + 1, Row].IsOccupied == false &&
                        field.Blocks[Column + 2, Row].IsOccupied == false)
                    {
                        return true;
                    }
                break;

            case 90:
            case 270:
                if (field.Blocks[Column, Row - 1].IsOccupied == false &&
                    field.Blocks[Column, Row + 1].IsOccupied == false &&
                    field.Blocks[Column, Row + 2].IsOccupied == false)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    public override bool CanShiftLeft(GameField field)
    {
        if (Column - 1 >= 0)
        {
            switch (ShapeAxis)
            {
                case 0:
                case 180:
                    if (field.Blocks[Column - 1, Row - 1].IsOccupied == false
                        && field.Blocks[Column - 1, Row].IsOccupied == false
                        && field.Blocks[Column - 1, Row + 1].IsOccupied == false
                        && field.Blocks[Column - 1, Row + 2].IsOccupied == false)
                    {
                        return true;
                    }
                    break;

                case 90:
                case 270:
                    if (Column - 2 >= 0)
                        if (field.Blocks[Column - 2, Row].IsOccupied == false)
                            return true;
                    break;
            }
        }

        return false;
    }

    public override bool CanShiftRight(GameField field)
    {
        switch (ShapeAxis)
        {
            case 0:
            case 180:
                if ((Column + 1 < field.GameWidth)
                    && field.Blocks[Column + 1, Row - 1].IsOccupied == false
                    && field.Blocks[Column + 1, Row].IsOccupied == false
                    && field.Blocks[Column + 1, Row + 1].IsOccupied == false
                    && field.Blocks[Column + 1, Row + 2].IsOccupied == false)
                {
                    return true;
                }
                break;

            case 90:
            case 270:
                if (Column + 3 < field.GameWidth &&
                    field.Blocks[Column + 3, Row].IsOccupied == false)
                    return true;
                break;
        }


        return false;
    }

    public override void Clear(GameField field)
    {
        //based on the axis
        //find the blocks that make up the shape and clear each one
        switch (ShapeAxis)
        {
            case 0:
            case 180:
                field.Blocks[Column, Row - 1].Clear(this);
                field.Blocks[Column, Row].Clear(this);
                field.Blocks[Column, Row + 1].Clear(this);
                field.Blocks[Column, Row + 2].Clear(this);
                break;

            case 90:
            case 270:
                field.Blocks[Column - 1, Row].Clear(this);
                field.Blocks[Column, Row].Clear(this);
                field.Blocks[Column + 1, Row].Clear(this);
                field.Blocks[Column + 2, Row].Clear(this);
                break;
        }
    }

    public override void Draw(GameField field)
    {
        //based on the axis
        //find the blocks that make up the shape and call occupy on each one
        switch (ShapeAxis)
        {
            case 0:
            case 180:
                field.Blocks[Column, Row - 1].Occupy(this);
                field.Blocks[Column, Row].Occupy(this);
                field.Blocks[Column, Row + 1].Occupy(this);
                field.Blocks[Column, Row + 2].Occupy(this);
                break;

            case 90:
            case 270:
                field.Blocks[Column - 1, Row].Occupy(this);
                field.Blocks[Column, Row].Occupy(this);
                field.Blocks[Column + 1, Row].Occupy(this);
                field.Blocks[Column + 2, Row].Occupy(this);
                break;
        }
    }

    public override void Wedge(GameField field)
    {
        //todo: add wedging code
        switch (ShapeAxis)
        {
            case 0:
            case 180:
                field.LineManagers[Row - 1].RegisterShapeBlock(new BlockInfo
                {
                    Column = this.Column,
                    BlockColor = this.ShapeBackground,
                });
                field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                {
                    Column = this.Column,
                    BlockColor = this.ShapeBackground,
                });
                field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
                {
                    Column = this.Column,
                    BlockColor = this.ShapeBackground,
                });
                field.LineManagers[Row + 2].RegisterShapeBlock(new BlockInfo
                {
                    Column = this.Column,
                    BlockColor = this.ShapeBackground,
                });
                break;

            case 90:
            case 270:
                field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                {
                    Column = this.Column - 1,
                    BlockColor = this.ShapeBackground,
                });
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
                field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                {
                    Column = this.Column + 2,
                    BlockColor = this.ShapeBackground,
                });
                break;
        }
    }
}
