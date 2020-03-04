﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace bot
{
    public interface IScreenCaptureService
    {
        Image CaptureScreen();
        void CaptureScreenToFile(string filename, ImageFormat format);
        Image CaptureWindow(IntPtr handle);
    }
}