using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeShifter.Library
{
    /// <summary>
    /// todo: make a number type that can only be 0, 90,180, 270 (360 = 0)
    /// </summary>
    public class Axis
    {
        int _value;

        private Axis(int value = 90) { _value = value; }

        public static implicit operator int(Axis axis)
        {
            return axis._value;
        }

        public static implicit operator Axis(int axis)
        {
            if (axis == 0 || axis == 90 || axis == 180 || axis == 270 || axis == 360)
            {
                var new_axis = new Axis(axis);
                if (new_axis == 360)
                    new_axis._value = 0;

                return new_axis;
            }
            else
                throw new InvalidOperationException("Axis can only be 0, 90, 180,270, or 360");
        }
    }
}
