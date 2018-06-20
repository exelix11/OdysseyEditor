using BnTxx.Formats;
using BnTxx.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace BnTxx
{
    public static class PixelDecoder
    {
        private delegate Bitmap DecodeFunc(Texture Tex, int Offset);

        private static
            Dictionary<TextureFormatType, DecodeFunc> DecodeFuncs = new
            Dictionary<TextureFormatType, DecodeFunc>()
        {
            { TextureFormatType.R5G6B5,    DecodeR5G6B5    },
            { TextureFormatType.R8G8,      DecodeR8G8      },
            { TextureFormatType.R16,       DecodeR16       },
            { TextureFormatType.R8G8B8A8,  DecodeR8G8B8A8  },
            { TextureFormatType.R11G11B10, DecodeR11G11B10 },
            { TextureFormatType.R32,       DecodeR32       },
            { TextureFormatType.BC1,       BCn.DecodeBC1   },
            { TextureFormatType.BC2,       BCn.DecodeBC2   },
            { TextureFormatType.BC3,       BCn.DecodeBC3   },
            { TextureFormatType.BC4,       BCn.DecodeBC4   },
            { TextureFormatType.BC5,       BCn.DecodeBC5   }
        };

        public static bool TryDecode(Texture Tex, out Bitmap Img, int Offset = 0)
        {
            if (DecodeFuncs.ContainsKey(Tex.FormatType))
            {
                Img = DecodeFuncs[Tex.FormatType](Tex, Offset);

                if (Img.Width  != Tex.Width ||
                    Img.Height != Tex.Height)
                {
                    Bitmap Output = new Bitmap(Tex.Width, Tex.Height);

                    using (Graphics g = Graphics.FromImage(Output))
                    {
                        Rectangle Rect = new Rectangle(0, 0, Tex.Width, Tex.Height);

                        g.DrawImage(Img, Rect, Rect, GraphicsUnit.Pixel);
                    }

                    Img.Dispose();

                    Img = Output;
                }

                return true;
            }

            Img = null;

            return false;
        }

        public static Bitmap DecodeR5G6B5(Texture Tex, int Offset)
        {
            byte[] Output = new byte[Tex.Width * Tex.Height * 4];

            int OOffset = 0;

            ISwizzle Swizzle = Tex.GetSwizzle();

            for (int Y = 0; Y < Tex.Height; Y++)
            {
                for (int X = 0; X < Tex.Width; X++)
                {
                    int IOffs = Offset + Swizzle.GetSwizzleOffset(X, Y);

                    int Value =
                        Tex.Data[IOffs + 0] << 0 |
                        Tex.Data[IOffs + 1] << 8;

                    int R = ((Value >>  0) & 0x1f) << 3;
                    int G = ((Value >>  5) & 0x3f) << 2;
                    int B = ((Value >> 11) & 0x1f) << 3;

                    Output[OOffset + 0] = (byte)(B | (B >> 5));
                    Output[OOffset + 1] = (byte)(G | (G >> 6));
                    Output[OOffset + 2] = (byte)(R | (R >> 5));
                    Output[OOffset + 3] = 0xff;

                    OOffset += 4;
                }
            }

            return GetBitmap(Output, Tex.Width, Tex.Height);
        }

        public static Bitmap DecodeR8G8(Texture Tex, int Offset)
        {
            byte[] Output = new byte[Tex.Width * Tex.Height * 4];

            int OOffset = 0;

            ISwizzle Swizzle = Tex.GetSwizzle();

            for (int Y = 0; Y < Tex.Height; Y++)
            {
                for (int X = 0; X < Tex.Width; X++)
                {
                    int IOffs = Offset + Swizzle.GetSwizzleOffset(X, Y);

                    Output[OOffset + 1] = Tex.Data[IOffs + 1];
                    Output[OOffset + 2] = Tex.Data[IOffs + 0];
                    Output[OOffset + 3] = 0xff;

                    OOffset += 4;
                }
            }

            return GetBitmap(Output, Tex.Width, Tex.Height);
        }

        public static Bitmap DecodeR16(Texture Tex, int Offset)
        {
            //TODO: What should be done with the extra precision?
            //TODO: Can this be used with Half floats too?
            byte[] Output = new byte[Tex.Width * Tex.Height * 4];

            int OOffset = 0;

            ISwizzle Swizzle = Tex.GetSwizzle();

            for (int Y = 0; Y < Tex.Height; Y++)
            {
                for (int X = 0; X < Tex.Width; X++)
                {
                    int IOffs = Offset + Swizzle.GetSwizzleOffset(X, Y);

                    Output[OOffset + 2] = Tex.Data[IOffs + 1];
                    Output[OOffset + 3] = 0xff;

                    OOffset += 4;
                }
            }

            return GetBitmap(Output, Tex.Width, Tex.Height);
        }

        public static Bitmap DecodeR8G8B8A8(Texture Tex, int Offset)
        {
            byte[] Output = new byte[Tex.Width * Tex.Height * 4];

            int OOffset = 0;

            ISwizzle Swizzle = Tex.GetSwizzle();

            for (int Y = 0; Y < Tex.Height; Y++)
            {
                for (int X = 0; X < Tex.Width; X++)
                {
                    int IOffs = Offset + Swizzle.GetSwizzleOffset(X, Y);

                    Output[OOffset + 0] = Tex.Data[IOffs + 2];
                    Output[OOffset + 1] = Tex.Data[IOffs + 1];
                    Output[OOffset + 2] = Tex.Data[IOffs + 0];
                    Output[OOffset + 3] = Tex.Data[IOffs + 3];

                    OOffset += 4;
                }
            }

            return GetBitmap(Output, Tex.Width, Tex.Height);
        }

        public static Bitmap DecodeR11G11B10(Texture Tex, int Offset)
        {
            //TODO: What should be done with the extra precision?
            byte[] Output = new byte[Tex.Width * Tex.Height * 4];

            int OOffset = 0;

            ISwizzle Swizzle = Tex.GetSwizzle();

            for (int Y = 0; Y < Tex.Height; Y++)
            {
                for (int X = 0; X < Tex.Width; X++)
                {
                    int IOffs = Offset + Swizzle.GetSwizzleOffset(X, Y);

                    int Value = IOUtils.Get32(Tex.Data, IOffs);

                    int R = (Value >>  0) & 0x7ff;
                    int G = (Value >> 11) & 0x7ff;
                    int B = (Value >> 22) & 0x3ff;

                    Output[OOffset + 0] = (byte)(B >> 2);
                    Output[OOffset + 1] = (byte)(G >> 3);
                    Output[OOffset + 2] = (byte)(R >> 3);
                    Output[OOffset + 3] = 0xff;

                    OOffset += 4;
                }
            }

            return GetBitmap(Output, Tex.Width, Tex.Height);
        }

        public static Bitmap DecodeR32(Texture Tex, int Offset)
        {
            //TODO: What should be done with the extra precision?
            //TODO: Can this be used with 32 bits fixed point 0.0.32 values too?
            byte[] Output = new byte[Tex.Width * Tex.Height * 4];

            int OOffset = 0;

            ISwizzle Swizzle = Tex.GetSwizzle();

            using (MemoryStream MS = new MemoryStream(Tex.Data))
            {
                BinaryReader Reader = new BinaryReader(MS);

                for (int Y = 0; Y < Tex.Height; Y++)
                {
                    for (int X = 0; X < Tex.Width; X++)
                    {
                        int IOffs = Offset + Swizzle.GetSwizzleOffset(X, Y);

                        MS.Seek(IOffs, SeekOrigin.Begin);

                        float Value = Reader.ReadSingle();

                        Output[OOffset + 2] = (byte)(Value * ((1f / 32) * 0xff));
                        Output[OOffset + 3] = 0xff;

                        OOffset += 4;
                    }
                }
            }

            return GetBitmap(Output, Tex.Width, Tex.Height);
        }

        public static Bitmap PermChAndGetBitmap(
            byte[]               Buffer,
            int                  Width,
            int                  Height,
            params ChannelType[] ChTypes)
        {
            if (ChTypes.Length == 4 && (
                ChTypes[0] != ChannelType.Red   ||
                ChTypes[1] != ChannelType.Green ||
                ChTypes[2] != ChannelType.Blue  ||
                ChTypes[3] != ChannelType.Alpha))
            {
                for (int Offset = 0; Offset < Buffer.Length; Offset += 4)
                {
                    byte B = Buffer[Offset + 0];
                    byte G = Buffer[Offset + 1];
                    byte R = Buffer[Offset + 2];
                    byte A = Buffer[Offset + 3];

                    int j = 0;

                    foreach (int i in new int[] { 2, 1, 0, 3 })
                    {
                        switch (ChTypes[j++])
                        {
                            case ChannelType.Zero:  Buffer[Offset + i] = 0;    break;
                            case ChannelType.One:   Buffer[Offset + i] = 0xff; break;
                            case ChannelType.Red:   Buffer[Offset + i] = R;    break;
                            case ChannelType.Green: Buffer[Offset + i] = G;    break;
                            case ChannelType.Blue:  Buffer[Offset + i] = B;    break;
                            case ChannelType.Alpha: Buffer[Offset + i] = A;    break;
                        }
                    }
                }
            }

            return GetBitmap(Buffer, Width, Height);
        }

        public static Bitmap GetBitmap(byte[] Buffer, int Width, int Height)
        {
            Rectangle Rect = new Rectangle(0, 0, Width, Height);

            Bitmap Img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            BitmapData ImgData = Img.LockBits(Rect, ImageLockMode.WriteOnly, Img.PixelFormat);

            Marshal.Copy(Buffer, 0, ImgData.Scan0, Buffer.Length);

            Img.UnlockBits(ImgData);

            return Img;
        }
    }
}
