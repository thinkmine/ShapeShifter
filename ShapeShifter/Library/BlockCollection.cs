using ShapeShifter.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ShapeShifter.Library
{
    public class BlockCollection
    {
        Block[,] _blocks;
        public int MaxRows { get; }
        public int MaxColumns { get; }

        public BlockCollection(int columns, int rows)
        {
            MaxRows = rows;
            MaxColumns = columns;
            _blocks = new Block[MaxColumns, MaxRows];
        }

        internal void Add(Block block, int column, int row)
        {
            try
            {
                _blocks[column, row] = block;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Block index cannot exceed {MaxColumns} column and {MaxRows} rows.");
            }
        }

        public Block this[int column, int row]
        {
            get
            {

                if (column >= MaxColumns)
                    column = MaxColumns - 1;

                if (row >= MaxRows)
                    row = MaxRows - 1;

                return _blocks[column, row];
            }
        }


    }
}
