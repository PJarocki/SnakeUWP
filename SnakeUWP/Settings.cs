using Windows.UI;

namespace SnakeUWP
{
    public static class Settings
    {
        /// <summary>The NumberOfCellsPerSide property represents the lenght of the Row/Column.</summary>
        public static int NumberOfCellsPerSide = 40;

        /// <summary>The CellSize property represents the cell density in pixels.</summary>
        public static int CellSize = 20;

        /// <summary>The SideSize property represents the cell density per side in pixels.</summary>
        public static int SideSize = NumberOfCellsPerSide * CellSize;

        /// <summary>The PointMultipier property represents the number of points that user earn.</summary>
        public static int PointMultipier = 10;

        /// <summary>The RefreshTaskPeriod property represents the amount of time after which timer reactivates.</summary>
        public static int RefreshTaskPeriod = 150; //in milliseconds

        /// <summary>The SnakeColor property represents the color of the snake.</summary>
        public static Color SnakeColor = Colors.Green;

        /// <summary>The FoodColor property represents the color of the food.</summary>
        public static Color FoodColor = Colors.Red;
    }
}
