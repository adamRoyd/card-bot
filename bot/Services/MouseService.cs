using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bot.Services
{
    public static class MouseService
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public static void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public static async Task LinearSmoothMove(Point newPosition, int steps)
        {
            Point start = Cursor.Position;
            PointF iterPoint = start;

            // Find the slope of the line segment defined by start and newPosition
            PointF slope = new PointF(newPosition.X - start.X, newPosition.Y - start.Y);

            // Divide by the number of steps
            slope.X = slope.X / steps;
            slope.Y = slope.Y / steps;

            // Move the mouse to each iterative point.
            for (int i = 0; i < steps; i++)
            {
                iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                Cursor.Position = Point.Round(iterPoint);
                await Task.Delay(1);
            }

            // Move the mouse to the final destination.
            Cursor.Position = newPosition;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
    }
}
