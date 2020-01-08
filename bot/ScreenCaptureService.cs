using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace bot
{
    public static class ScreenCaptureService
    {
        public static void TakeScreenCapture(string path)
        {
            try
            {
                ScreenCapture sc = new ScreenCapture();
                // capture entire screen, and save it to a file
                Image img = sc.CaptureScreen();
                var fileName = $"{path}\\board.png";
                img.Save(fileName);
                //// display image in a Picture control named imageDisplay
                //this.imageDisplay.Image = img;
                //// capture this window, and save it
                //sc.CaptureWindowToFile(this.Handle, "C:\\temp2.gif", ImageFormat.Gif);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
