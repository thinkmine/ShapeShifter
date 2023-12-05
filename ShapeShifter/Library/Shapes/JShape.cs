using ShapeShifter.UI.Controls;
using System;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace ShapeShifter.Library.Shapes
{
    public class JShape : Shape
    {
        public JShape()
        {
            ShapeAxis = 90;
            ShapeBackground = new SolidColorBrush(Colors.Red);
        }

        public override bool CanBeAdded(GameField field)
        {
            if (!field.Blocks[Column, Row].IsOccupied &&
            !field.Blocks[Column, Row - 1].IsOccupied &&
            !field.Blocks[Column, Row + 1].IsOccupied &&
            !field.Blocks[Column - 1, Row + 1].IsOccupied)
            {
                return true;
            }
            return false;
        }

        public override bool CanDescend(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    if (Row + 1 < field.GameHeight)
                    {
                        if (!field.Blocks[Column, Row + 1].IsOccupied &&
                           !field.Blocks[Column - 1, Row + 1].IsOccupied &&
                           !field.Blocks[Column + 1, Row + 2].IsOccupied)
                            return true;
                    }
                    break;
                case 90:
                    if (Row + 2 < field.GameHeight)
                    {
                        if (!field.Blocks[Column, Row + 2].IsOccupied &&
                            !field.Blocks[Column - 1, Row + 2].IsOccupied)
                            return true;
                    }
                    break;

                case 180:
                    if (Row + 1 < field.GameHeight)
                    {
                        if (!field.Blocks[Column, Row + 1].IsOccupied &&
                           !field.Blocks[Column - 1, Row + 1].IsOccupied &&
                           !field.Blocks[Column + 1, Row + 1].IsOccupied)
                            return true;
                    }
                    break;
                case 270:
                    if (Row + 2 < field.GameHeight)
                    {
                        if (!field.Blocks[Column, Row + 2].IsOccupied &&
                            !field.Blocks[Column + 1, Row].IsOccupied)
                            return true;
                    }
                    break;

            }
            return false;
        }

        public override bool CanRotate(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    if (this.Row + 1 < field.GameHeight)
                    {
                        if (Row - 1 >= 0)
                        {
                            if (Column - 1 >= 0)
                            {
                                if (!field.Blocks[Column, Row - 1].IsOccupied &&
                                    !field.Blocks[Column, Row + 1].IsOccupied &&
                                    !field.Blocks[Column - 1, Row + 1].IsOccupied)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    break;
                case 90:

                    if (this.Row - 1 >= 0)
                    {

                        if (Column + 1 < field.GameWidth && Column - 1 > 0)
                        {
                            if (!field.Blocks[Column - 1, Row].IsOccupied &&
                                !field.Blocks[Column - 1, Row - 1].IsOccupied &&
                                !field.Blocks[Column + 1, Row].IsOccupied)
                            {
                                return true;
                            }
                        }

                    }


                    break;
                case 180:
                    if (this.Row + 1 < field.GameHeight && Column + 1 < field.GameWidth)
                    {
                        if (Row - 1 >= 0)
                        {
                            if (!field.Blocks[Column, Row - 1].IsOccupied &&
                                !field.Blocks[Column + 1, Row - 1].IsOccupied &&
                                !field.Blocks[Column, Row + 1].IsOccupied)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case 270:
                    //check boundaries
                    if ((Column - 1) > 0 && Column + 1 < field.GameWidth && Row + 1 < field.GameHeight)
                    {
                        if (!field.Blocks[Column - 1, Row].IsOccupied &&
                               !field.Blocks[Column + 1, Row].IsOccupied &&
                               !field.Blocks[Column + 1, Row + 1].IsOccupied)
                        {
                            return true;
                        }
                    }


                    break;

            }
            return false;
        }

        public override bool CanShiftLeft(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    if (Column - 2 >= 0)
                        if (!field.Blocks[Column - 2, Row].IsOccupied &&
                           !field.Blocks[Column, Row + 1].IsOccupied)
                            return true;
                    break;
                case 90:
                    if (Column - 2 >= 0)
                        if (!field.Blocks[Column - 1, Row].IsOccupied &&
                           !field.Blocks[Column - 1, Row - 1].IsOccupied &&
                           !field.Blocks[Column - 2, Row + 1].IsOccupied)
                            return true;
                    break;
                case 180:
                    if (Column - 2 >= 0)
                        if (!field.Blocks[Column - 2, Row].IsOccupied &&
                        !field.Blocks[Column - 2, Row - 1].IsOccupied)
                            return true;

                    break;
                case 270:
                    if (Column - 1 >= 0)
                        if (!field.Blocks[Column - 1, Row].IsOccupied &&
                      !field.Blocks[Column - 1, Row - 1].IsOccupied &&
                      !field.Blocks[Column - 1, Row + 1].IsOccupied)
                            return true;
                    break;

            }

            return false;
        }

        public override bool CanShiftRight(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    if (Column + 2 < field.GameWidth)
                        if (!field.Blocks[Column + 2, Row].IsOccupied &&
                           !field.Blocks[Column + 2, Row + 1].IsOccupied)
                            return true;
                    break;
                case 90:
                    if (Column + 1 < field.GameWidth)
                        if (!field.Blocks[Column + 1, Row].IsOccupied &&
                           !field.Blocks[Column + 1, Row - 1].IsOccupied &&
                           !field.Blocks[Column + 1, Row + 1].IsOccupied)
                            return true;
                    break;
                case 180:
                    if (Column + 2 < field.GameWidth)
                        if (!field.Blocks[Column + 2, Row].IsOccupied &&
                        !field.Blocks[Column, Row - 1].IsOccupied)
                            return true;

                    break;
                case 270:
                    if (Column + 2 < field.GameWidth)
                        if (!field.Blocks[Column + 1, Row].IsOccupied &&
                      !field.Blocks[Column + 2, Row - 1].IsOccupied &&
                      !field.Blocks[Column + 1, Row + 1].IsOccupied)
                            return true;
                    break;

            }

            return false;
        }

        public override void Clear(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    field.Blocks[Column, Row].Clear();
                    field.Blocks[Column - 1, Row].Clear();
                    field.Blocks[Column + 1, Row].Clear();
                    field.Blocks[Column + 1, Row + 1].Clear();
                    break;
                case 90:
                    field.Blocks[Column, Row].Clear();
                    field.Blocks[Column, Row - 1].Clear();
                    field.Blocks[Column, Row + 1].Clear();
                    field.Blocks[Column - 1, Row + 1].Clear();
                    break;
                case 180:
                    field.Blocks[Column, Row].Clear();
                    field.Blocks[Column - 1, Row].Clear();
                    field.Blocks[Column - 1, Row - 1].Clear();
                    field.Blocks[Column + 1, Row].Clear();
                    break;
                case 270:
                    field.Blocks[Column, Row].Clear();
                    field.Blocks[Column, Row - 1].Clear();
                    field.Blocks[Column + 1, Row - 1].Clear();
                    field.Blocks[Column, Row + 1].Clear();
                    break;
            }
        }

        public override void Draw(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    field.Blocks[Column, Row].Occupy(this);
                    field.Blocks[Column - 1, Row].Occupy(this);
                    field.Blocks[Column + 1, Row].Occupy(this);
                    field.Blocks[Column + 1, Row + 1].Occupy(this);


                    break;
                case 90:
                    field.Blocks[Column, Row].Occupy(this);
                    field.Blocks[Column, Row - 1].Occupy(this);
                    field.Blocks[Column, Row + 1].Occupy(this);
                    field.Blocks[Column - 1, Row + 1].Occupy(this);
                    break;
                case 180:
                    field.Blocks[Column, Row].Occupy(this);
                    field.Blocks[Column - 1, Row].Occupy(this);
                    field.Blocks[Column - 1, Row - 1].Occupy(this);
                    field.Blocks[Column + 1, Row].Occupy(this);
                    break;
                case 270:
                    field.Blocks[Column, Row].Occupy(this);
                    field.Blocks[Column, Row - 1].Occupy(this);
                    field.Blocks[Column + 1, Row - 1].Occupy(this);
                    field.Blocks[Column, Row + 1].Occupy(this);
                    break;
            }
        }

        public override void Wedge(GameField field)
        {
            switch (this.ShapeAxis)
            {
                case 0:
                    #region 0
                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });
                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column - 1,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column + 1,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column + 1,
                        BlockColor = this.ShapeBackground,

                    });
                    #endregion
                    break;
                case 90:
                    #region 90
                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });
                    field.LineManagers[Row - 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column - 1,
                        BlockColor = this.ShapeBackground,

                    });
                    #endregion
                    break;
                case 180:
                    #region 180
                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });
                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column - 1,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column + 1,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row - 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column - 1,
                        BlockColor = this.ShapeBackground,

                    });
                    #endregion
                    break;
                case 270:
                    #region 270
                    field.LineManagers[Row].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });
                    field.LineManagers[Row - 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row + 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column,
                        BlockColor = this.ShapeBackground,

                    });

                    field.LineManagers[Row - 1].RegisterShapeBlock(new BlockInfo
                    {
                        Column = this.Column + 1,
                        BlockColor = this.ShapeBackground,

                    });
                    #endregion
                    break;
            }
        }
    }
}
