using Windows.UI;

namespace SnakeUWP.Model
{
    /// <summary>
    /// Contains snake coordinates and color which are used on the map.
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// x2 has to be greater than x1 and y2 has to be greater than y1.
        /// </summary>
        /// <param name="x1">The X axis of the bounding rectangle (left side).</param>
        /// <param name="y1">The Y axis of the bounding rectangle (top side).</param>
        /// <param name="x2">The X axis of the bounding rectangle (right side).</param>
        /// <param name="y2">The Y axis of the bounding rectangle (bottom side).</param>
        /// <param name="color">The color of the rectangle.</param>
        public Rectangle(int x1, int y1, int x2, int y2, Color color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Color = color;
        }

        /// <summary>The X1 property represents location of the X axis (left side).</summary>
        public int X1 { get; set; }

        /// <summary>The Y1 property represents location of the Y axis (top side).</summary>
        public int Y1 { get; set; }

        /// <summary>The X2 property represents location of the X axis (right side).</summary>
        public int X2 { get; set; }

        /// <summary>The Y2 property represents location of the Y axis (bottom side).</summary>
        public int Y2 { get; set; }

        /// <summary>The Color property represents color of the object.</summary>
        public Color Color { get; set; }

        /// <summary>
        /// Perform a Copy of the object.
        /// </summary>
        /// <returns>The copied object.</returns>
        public Rectangle Clone()
        {
            return new Rectangle(X1, Y1, X2, Y2, Color);
        }
    }
}
