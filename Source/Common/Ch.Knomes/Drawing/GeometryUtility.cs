using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ch.Knomes.Drawing
{
    public static class GeometryUtility
    {
        /// <summary>
        /// Shrinks a the size proportionally until it fits into the given rectangle size
        /// </summary>
        public static Size ShrinkToFit(Size size, Size maxSize)
        {
            if (size.Width < 0 || size.Height < 0) throw new ArgumentException("area may not be empty", nameof(size));
            if (maxSize.Width < 0 || maxSize.Height < 0) throw new ArgumentException("maximum area may not be empty", nameof(maxSize));

            var newSize = Size.Empty;
            if (size.Width > maxSize.Width || size.Height > maxSize.Height)
            {
                if (maxSize.Height * size.Width > maxSize.Width * size.Height)
                {
                    newSize.Width = maxSize.Width;
                    newSize.Height = Convert.ToInt32((decimal)newSize.Width / size.Width * size.Height);
                }
                else
                {
                    newSize.Height = maxSize.Height;
                    newSize.Width = Convert.ToInt32((decimal)newSize.Height / size.Height * size.Width);
                }
            }
            else
            {
                newSize = new Size(size.Width, size.Height);
            }
            return newSize;
        }
    }
}
