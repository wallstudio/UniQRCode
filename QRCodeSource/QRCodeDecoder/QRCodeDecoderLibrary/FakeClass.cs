
using System;

namespace QRCodeDecoderLibrary
{
    public enum ImageLockMode { ReadOnly }
    public enum PixelFormat { Format24bppRgb }

    public class Bitmap
    {
        public string FileName { get; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Bitmap(string fileName) => FileName = fileName;

        public BitmapData LockBits(Rectangle rectangle, ImageLockMode readOnly, PixelFormat format24bppRgb) => throw new NotImplementedException();
        public void UnlockBits(BitmapData bitmapData) => throw new NotImplementedException();
    }

    public readonly struct BitmapData
    {
        public readonly object Reference;
        public readonly IntPtr Scan0;
        public readonly int Stride;
        public BitmapData(object reference, IntPtr scan0, int stride) => (Reference, Scan0, Stride) = (reference, scan0, stride);
    }

    public readonly struct Rectangle
    {
        public readonly int X, Y, ImageWidth, ImageHeight;
        public Rectangle(int x, int y, int imageWidth, int imageHeight) => (X, Y, ImageWidth, ImageHeight) = (x, y, imageWidth, imageHeight);
    }
}