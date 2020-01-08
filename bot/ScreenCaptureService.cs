using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace bot
{
    public static class ScreenCaptureService
    {
        public static void TakeScreenCapture()
        {
            ScreenCapture sc = new ScreenCapture();
            // capture entire screen, and save it to a file
            Image img = sc.CaptureScreen();
            img.Save("..\\..\\..\\images\\testScreenShot.png");
            //// display image in a Picture control named imageDisplay
            //this.imageDisplay.Image = img;
            //// capture this window, and save it
            //sc.CaptureWindowToFile(this.Handle, "C:\\temp2.gif", ImageFormat.Gif);
        }
    }
}
