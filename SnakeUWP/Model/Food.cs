using Windows.UI;

namespace SnakeUWP.Model
{
    /// <summary>
    /// Contains food coordinates, width, height and color which are used on the map.
    /// </summary>
    public class Food
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Food"/> class.
        /// </summary>
        /// <param name="x">Location of the X axis.</param>
        /// <param name="y">Location of the Y axis.</param>
        public Food(int x, int y)
        {
            X = x;
            Y = y;
            Width = Settings.CellSize/3;
            Height = Settings.CellSize/3;
            Color = Settings.FoodColor;
        }

        /// <summary>The X property represents location of the X axis.</summary>
        public int X { get; set; }

        /// <summary>The Y property represents location of the Y axis.</summary>
        public int Y { get; set; }

        /// <summary>The Width property represents the width of the object.</summary>
        public int Width { get; set; }

        /// <summary>The Height property represents the height of the object.</summary>
        public int Height { get; set; }

        /// <summary>The Color property represents color of the object.</summary>
        public Color Color { get; set; }
}
}
