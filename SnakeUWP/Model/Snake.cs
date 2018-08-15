using System.Collections.Generic;
using Windows.UI;

namespace SnakeUWP.Model
{
    /// <summary>
    /// Contains list of snake parts and direction which are used on the map.
    /// </summary>
    class Snake
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Snake"/> class.
        /// </summary>
        public Snake()
        {
            SnakeParts = new LinkedList<Rectangle>();
            SnakeParts.AddFirst(
                 new Rectangle(Settings.NumberOfCellsPerSide / 2, Settings.NumberOfCellsPerSide / 2, (Settings.NumberOfCellsPerSide / 2) + 1,
                    (Settings.NumberOfCellsPerSide / 2) + 1, Settings.SnakeColor));
            SnakeDirection = Direction.Right;
        }

        /// <summary>The SnakeParts property represents a list of snake parts.</summary>
        public LinkedList<Rectangle> SnakeParts { get; set; }

        /// <summary>The SnakeDirection property represents the direction in which snake will go.</summary>
        public Direction SnakeDirection { get; set; }
    }

    /// <summary>
    /// The <see cref="Direction"/> Enum defines constants that are used to specify the direction of the snake.
    /// </summary>
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom
    }
}
