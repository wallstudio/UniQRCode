
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

namespace QRCodeDecoderLibrary
{
    public enum ImageLockMode { ReadOnly }
    public enum PixelFormat { Format24bppRgb }

    public class Bitmap
    {
        public (byte r, byte g, byte b)[] Data { get; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Bitmap(string fileName) => throw new NotImplementedException();

        public Bitmap((byte r, byte g, byte b)[] data, int width) => (Data, Width, Height) = (data, width, data.Length / width);

        public BitmapData LockBits(Rectangle rectangle, ImageLockMode readOnly, PixelFormat format24bppRgb)
        {
            var handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            var ptr = handle.AddrOfPinnedObject();
            return new BitmapData(handle, ptr, Width * 3);
        }

        public void UnlockBits(BitmapData bitmapData) => bitmapData.Handle.Free();
    }

    public readonly struct BitmapData
    {
        public readonly GCHandle Handle;
        public readonly IntPtr Scan0;
        public readonly int Stride;
        public BitmapData(GCHandle reference, IntPtr scan0, int stride) => (Handle, Scan0, Stride) = (reference, scan0, stride);
    }

    public readonly struct Rectangle
    {
        public readonly int X, Y, ImageWidth, ImageHeight;
        public Rectangle(int x, int y, int imageWidth, int imageHeight) => (X, Y, ImageWidth, ImageHeight) = (x, y, imageWidth, imageHeight);
    }
}