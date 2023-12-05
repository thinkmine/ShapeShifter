using ShapeShifter.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Graphics.Printing;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;

namespace ShapeShifter.Library
{
    public abstract class Shape
    {
        public event Action Rotated;
        public event Action Shifted;
        public event Action Descended;
        public event Action<Shape> Wedged;

        Brush _temp_background;

        public Axis ShapeAxis { get; set; }
        public Brush ShapeBackground { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        #region Abstract Methods

        public abstract bool CanBeAdded(GameField field);

        public abstract void Draw(GameField field);

        public abstract void Clear(GameField field);

        public abstract bool CanDescend(GameField field);

        public abstract bool CanShiftLeft(GameField field);

        public abstract bool CanShiftRight(GameField field);

        public abstract bool CanRotate(GameField field);

        public abstract void Wedge(GameField field);
        #endregion

        void DrawShape(GameField field)
        {
            ShapeBackground = _temp_background;
            Draw(field);
        }

        public void ClearShape(GameField field)
        {
            _temp_background = this.ShapeBackground;
            ShapeBackground = field.FieldBackground;
            Clear(field);
        }

        public virtual void ShiftLeft(GameField field)
        {
            if (CanShiftLeft(field))
            {
                ClearShape(field);
                Column -= 1;
                DrawShape(field);
                Shifted?.Invoke();
            }
        }

        public virtual void ShiftRight(GameField field)
        {
            if (CanShiftRight(field))
            {
                ClearShape(field);
                Column += 1;
                DrawShape(field);
                Shifted?.Invoke();
            }
        }

        public virtual void Rotate(GameField field)
        {
            if (CanRotate(field))
            {
                ClearShape(field);
                ShapeAxis += 90;
                DrawShape(field);
                Rotated?.Invoke();
            }
        }

        public virtual void Descend(GameField field)
        {
            if (CanDescend(field))
            {
                ClearShape(field);
                Row += 1;
                DrawShape(field);
                Descended?.Invoke();
            }
            else
            {
                Wedge(field);
                Wedged?.Invoke(this);
            }
        }



    }

}
