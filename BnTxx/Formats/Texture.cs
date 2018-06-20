using BnTxx.Utilities;

using System;

namespace BnTxx.Formats
{
    public struct Texture
    {
        public string Name;

        public int Width;
        public int Height;
        public int ArrayCount;
        public int BlockHeightLog2;
        public int MipmapCount;

        public long[] MipOffsets;

        public byte[] Data;

        public ChannelType Channel0Type;
        public ChannelType Channel1Type;
        public ChannelType Channel2Type;
        public ChannelType Channel3Type;

        public TextureType       Type;
        public TextureFormatType FormatType;
        public TextureFormatVar  FormatVariant;

        public ISwizzle GetSwizzle()
        {
            return new BlockLinearSwizzle(
                GetWidthInTexels(),
                GetBytesPerTexel(),
                GetBlockHeight());
        }

        public int GetWidthInTexels()
        {
            switch (FormatType)
            {
                case TextureFormatType.BC1:
                case TextureFormatType.BC2:
                case TextureFormatType.BC3:
                case TextureFormatType.BC4:
                case TextureFormatType.BC5:
                case TextureFormatType.ASTC4x4:
                    return (Width + 3) / 4;

                case TextureFormatType.ASTC5x4:
                case TextureFormatType.ASTC5x5:
                    return (Width + 4) / 5;

                case TextureFormatType.ASTC6x5:
                case TextureFormatType.ASTC6x6:
                    return (Width + 5) / 6;

                case TextureFormatType.ASTC8x5:
                case TextureFormatType.ASTC8x6:
                case TextureFormatType.ASTC8x8:
                    return (Width + 7) / 8;

                case TextureFormatType.ASTC10x5:
                case TextureFormatType.ASTC10x6:
                case TextureFormatType.ASTC10x8:
                case TextureFormatType.ASTC10x10:
                    return (Width + 9) / 10;

                case TextureFormatType.ASTC12x10:
                case TextureFormatType.ASTC12x12:
                    return (Width + 11) / 12;
            }

            return Width;
        }

        public int GetPow2HeightInTexels()
        {
            int Pow2Height = BitUtils.Pow2RoundUp(Height);

            switch (FormatType)
            {
                case TextureFormatType.BC1:
                case TextureFormatType.BC2:
                case TextureFormatType.BC3:
                case TextureFormatType.BC4:
                case TextureFormatType.BC5:
                case TextureFormatType.ASTC4x4:
                case TextureFormatType.ASTC5x4:
                    return (Pow2Height + 3) / 4;

                case TextureFormatType.ASTC5x5:
                case TextureFormatType.ASTC6x5:
                case TextureFormatType.ASTC8x5:
                    return (Pow2Height + 4) / 5;

                case TextureFormatType.ASTC6x6:
                case TextureFormatType.ASTC8x6:
                case TextureFormatType.ASTC10x6:
                    return (Pow2Height + 5) / 6;

                case TextureFormatType.ASTC8x8:
                case TextureFormatType.ASTC10x8:
                    return (Pow2Height + 7) / 8;

                case TextureFormatType.ASTC10x10:
                case TextureFormatType.ASTC12x10:
                    return (Pow2Height + 9) / 10;

                case TextureFormatType.ASTC12x12:
                    return (Pow2Height + 11) / 12;
            }

            return Pow2Height;
        }

        public int GetBytesPerTexel()
        {
            switch (FormatType)
            {
                case TextureFormatType.R5G6B5:
                case TextureFormatType.R8G8:
                case TextureFormatType.R16:
                    return 2;

                case TextureFormatType.R8G8B8A8:
                case TextureFormatType.R11G11B10:
                case TextureFormatType.R32:
                    return 4;

                case TextureFormatType.BC1:
                case TextureFormatType.BC4:
                    return 8;

                case TextureFormatType.BC2:
                case TextureFormatType.BC3:
                case TextureFormatType.BC5:
                case TextureFormatType.ASTC4x4:
                case TextureFormatType.ASTC5x4:
                case TextureFormatType.ASTC5x5:
                case TextureFormatType.ASTC6x5:
                case TextureFormatType.ASTC6x6:
                case TextureFormatType.ASTC8x5:
                case TextureFormatType.ASTC8x6:
                case TextureFormatType.ASTC8x8:
                case TextureFormatType.ASTC10x5:
                case TextureFormatType.ASTC10x6:
                case TextureFormatType.ASTC10x8:
                case TextureFormatType.ASTC10x10:
                case TextureFormatType.ASTC12x10:
                case TextureFormatType.ASTC12x12:
                    return 16;
            }

            throw new NotImplementedException();
        }

        public int GetBlockHeight()
        {
            return 1 << BlockHeightLog2;
        }

        public override string ToString()
        {
            return Name;
        }
       
    }
}
