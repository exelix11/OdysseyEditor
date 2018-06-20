using BnTxx.Formats;
using System;
using System.IO;

namespace BnTxx
{
    static class ASTC
    {
        public static byte[] UnswizzleASTC(Texture Tex, int BlkWidth, int BlkHeight)
        {
            int W = (Tex.Width  + BlkWidth  - 1) / BlkWidth;
            int H = (Tex.Height + BlkHeight - 1) / BlkHeight;

            byte[] Output = new byte[W * H * 16];

            ISwizzle Swizzle = Tex.GetSwizzle();

            int OOffset = 0;

            for (int Y = 0; Y < H; Y++)
            {
                for (int X = 0; X < W; X++)
                {
                    int Offset = Swizzle.GetSwizzleOffset(X, Y);

                    Buffer.BlockCopy(Tex.Data, Offset, Output, OOffset, 16);

                    OOffset += 16;
                }
            }

            return Output;
        }

        public static void Save(Texture Tex, string FileName)
        {
            int BW = 0, BH = 0;

            switch (Tex.FormatType)
            {
                case TextureFormatType.ASTC4x4:   BW = 4;  BH = 4;  break;
                case TextureFormatType.ASTC5x4:   BW = 5;  BH = 4;  break;
                case TextureFormatType.ASTC5x5:   BW = 5;  BH = 5;  break;
                case TextureFormatType.ASTC6x5:   BW = 6;  BH = 5;  break;
                case TextureFormatType.ASTC6x6:   BW = 6;  BH = 6;  break;
                case TextureFormatType.ASTC8x5:   BW = 8;  BH = 5;  break;
                case TextureFormatType.ASTC8x6:   BW = 8;  BH = 6;  break;
                case TextureFormatType.ASTC8x8:   BW = 8;  BH = 8;  break;
                case TextureFormatType.ASTC10x5:  BW = 10; BH = 5;  break;
                case TextureFormatType.ASTC10x6:  BW = 10; BH = 6;  break;
                case TextureFormatType.ASTC10x8:  BW = 10; BH = 8;  break;
                case TextureFormatType.ASTC10x10: BW = 10; BH = 10; break;
                case TextureFormatType.ASTC12x10: BW = 12; BH = 10; break;
                case TextureFormatType.ASTC12x12: BW = 12; BH = 12; break;
            }

            byte[] Data = UnswizzleASTC(Tex, BW, BH);

            using (FileStream ASTCFile = new FileStream(FileName, FileMode.Create))
            {
                BinaryWriter Writer = new BinaryWriter(ASTCFile);

                int BZ = 1, Tex3DDepthZ = 1; //Should be > 1 for 3D textures only

                Writer.Write(0x5ca1ab13u);
                Writer.Write((byte)BW);
                Writer.Write((byte)BH);
                Writer.Write((byte)BZ);
                Writer.Write((byte)(Tex.Width >> 0));
                Writer.Write((byte)(Tex.Width >> 8));
                Writer.Write((byte)(Tex.Width >> 16));
                Writer.Write((byte)(Tex.Height >> 0));
                Writer.Write((byte)(Tex.Height >> 8));
                Writer.Write((byte)(Tex.Height >> 16));
                Writer.Write((byte)(Tex3DDepthZ >> 0));
                Writer.Write((byte)(Tex3DDepthZ >> 8));
                Writer.Write((byte)(Tex3DDepthZ >> 16));

                Writer.Write(Data);
            }
        }
    }
}
