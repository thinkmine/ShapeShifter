using ShapeShifter.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;

namespace ShapeShifter.Library
{
    public class BlockInfo
    {
        public int Column { get; set; }
        public Brush BlockColor { get; set; }
    }

    public class FieldLineManager
    {
        GameField _field;
        int _width; int _line_number;
        List<BlockInfo> _line_blocks;
        public FieldLineManager(int width, int line_number, GameField field)
        {
            _field = field;
            _width = width;
            _line_number = line_number;
            _line_blocks = new List<BlockInfo>();
        }

        public void RegisterShapeBlock(BlockInfo block_info)
        {
            var target_block = _line_blocks.Where(i => i.Column == block_info.Column).FirstOrDefault();
            if (target_block == null)
                _line_blocks.Add(block_info);
        }

        public bool IsFull
        {
            get
            {
                return _line_blocks.Count() == _width;
            }
        }

        public void ClearLine()
        {
            foreach (var block in _line_blocks)
            {
                _field.Blocks[block.Column, _line_number].Clear();
            }
        }

        public void DrawLine()
        {
            foreach (var block_info in _line_blocks)
            {
                _field.Blocks[block_info.Column, _line_number].Occupy(block_info);
            }
        }

        public void ShiftDown()
        {
            if (_line_blocks.Count > 0)
            {
                if (_field.LineManagers[_line_number - 1]._line_blocks.Count > 0)
                {
                    //remove the blocks from the current line
                    ClearLine();

                    //get blocks from line above me and copy them below me
                    _line_blocks = new List<BlockInfo>(_field.LineManagers[_line_number - 1]._line_blocks);
                    DrawLine();
                }
                else
                {


                    ClearLine();

                    _line_blocks = new List<BlockInfo>(_field.LineManagers[_line_number - 1]._line_blocks);
                }


                //remove them from the abovel line

                //repeat process
                _field.LineManagers[_line_number - 1].ShiftDown();
            }
        }
    }
}
