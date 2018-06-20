using BnTxx.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BnTxx.Formats
{
    public class BinaryTexture : IList<Texture>
    {
        public List<Texture> Textures;

        private PatriciaTree NameTree;

        public Texture this[int Index]
        {
        	get { return Textures[Index]; }
        	set { Textures[Index] = value; }
        }

        public int Count { get { return Textures.Count; } }

        public bool IsReadOnly { get { return false; } }

        /* Initialization and loading */

        public BinaryTexture()
        {
            Textures = new List<Texture>();

            NameTree = new PatriciaTree();
        }

        public BinaryTexture(Stream Data) : this(new BinaryReader(Data)) { }

        public BinaryTexture(BinaryReader Reader) : this()
        {
            string BnTxSignature = Reader.ReadString(8);

            CheckSignature("BNTX", BnTxSignature);

            int    DataLength     = Reader.ReadInt32();
            ushort ByteOrderMark  = Reader.ReadUInt16();
            ushort FormatRevision = Reader.ReadUInt16();
            int    NameAddress    = Reader.ReadInt32();
            int    StringsAddress = Reader.ReadInt32() >> 16;
            int    RelocAddress   = Reader.ReadInt32();
            int    FileLength     = Reader.ReadInt32();

            ReadBinaryTextureInfo(Reader);
        }

        public string Name;
        private void ReadBinaryTextureInfo(BinaryReader Reader)
        {
            string NXSignature = Reader.ReadString(4);

            CheckSignature("NX  ", NXSignature);

            uint TexturesCount   = Reader.ReadUInt32();
            long InfoPtrsAddress = Reader.ReadInt64();
            long DataBlkAddress  = Reader.ReadInt64();
            long DictAddress     = Reader.ReadInt64();
            uint StrDictLength   = Reader.ReadUInt32();

            Reader.BaseStream.Seek(DictAddress, SeekOrigin.Begin);

            NameTree = new PatriciaTree(Reader);

            for (int Index = 0; Index < TexturesCount; Index++)
            {
                Reader.BaseStream.Seek(InfoPtrsAddress + Index * 8, SeekOrigin.Begin);
                Reader.BaseStream.Seek(Reader.ReadInt64(),          SeekOrigin.Begin);

                string BRTISignature = Reader.ReadString(4);

                CheckSignature("BRTI", BRTISignature);

                int    BRTILength0      = Reader.ReadInt32();
                long   BRTILength1      = Reader.ReadInt64();
                byte   Flags            = Reader.ReadByte();
                byte   Dimensions       = Reader.ReadByte();
                ushort TileMode         = Reader.ReadUInt16();
                ushort SwizzleSize      = Reader.ReadUInt16();
                ushort MipmapCount      = Reader.ReadUInt16();
                ushort MultiSampleCount = Reader.ReadUInt16();
                ushort Reversed1A       = Reader.ReadUInt16();
                uint   Format           = Reader.ReadUInt32();
                uint   AccessFlags      = Reader.ReadUInt32();
                int    Width            = Reader.ReadInt32();
                int    Height           = Reader.ReadInt32();
                int    Depth            = Reader.ReadInt32();
                int    ArrayCount       = Reader.ReadInt32();
                int    BlockHeightLog2  = Reader.ReadInt32();
                int    Reserved38       = Reader.ReadInt32();
                int    Reserved3C       = Reader.ReadInt32();
                int    Reserved40       = Reader.ReadInt32();
                int    Reserved44       = Reader.ReadInt32();
                int    Reserved48       = Reader.ReadInt32();
                int    Reserved4C       = Reader.ReadInt32();
                int    DataLength       = Reader.ReadInt32();
                int    Alignment        = Reader.ReadInt32();
                int    ChannelTypes     = Reader.ReadInt32();
                int    TextureType      = Reader.ReadInt32();
                long   NameAddress      = Reader.ReadInt64();
                long   ParentAddress    = Reader.ReadInt64();
                long   PtrsAddress      = Reader.ReadInt64();

                Reader.BaseStream.Seek(NameAddress, SeekOrigin.Begin);

                Name = Reader.ReadShortString();

                long[] MipOffsets = new long[MipmapCount];

                Reader.BaseStream.Seek(PtrsAddress, SeekOrigin.Begin);

                long BaseOffset = Reader.ReadInt64();

                for (int Mip = 1; Mip < MipmapCount; Mip++)
                {
                    MipOffsets[Mip] = Reader.ReadInt64() - BaseOffset;
                }

                Reader.BaseStream.Seek(BaseOffset, SeekOrigin.Begin);

                byte[] Data = Reader.ReadBytes(DataLength);

                Textures.Add(new Texture()
                {
                    Name            = Name,
                    Width           = Width,
                    Height          = Height,
                    ArrayCount      = ArrayCount,
                    BlockHeightLog2 = BlockHeightLog2,
                    MipmapCount     = MipmapCount,
                    MipOffsets      = MipOffsets,
                    Data            = Data,

                    Channel0Type    = (ChannelType)((ChannelTypes >>  0) & 0xff),
                    Channel1Type    = (ChannelType)((ChannelTypes >>  8) & 0xff),
                    Channel2Type    = (ChannelType)((ChannelTypes >> 16) & 0xff),
                    Channel3Type    = (ChannelType)((ChannelTypes >> 24) & 0xff),

                    Type            = (TextureType)TextureType,

                    FormatType      = (TextureFormatType)((Format >> 8) & 0xff),
                    FormatVariant   = (TextureFormatVar) ((Format >> 0) & 0xff)
                });
            }
        }

        private void CheckSignature(string Expected, string Actual)
        {
            if (Actual != Expected)
            {
                throw new InvalidSignatureException(Expected, Actual);
            }
        }

        /* Public facing methods */

        public IEnumerator<Texture> GetEnumerator()
        {
            return Textures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Texture item)
        {
            Textures.Add(item);
        }

        public void Clear()
        {
            Textures.Clear();
        }

        public bool Contains(Texture item)
        {
            return Textures.Contains(item);
        }

        public void CopyTo(Texture[] array, int arrayIndex)
        {
            Textures.CopyTo(array, arrayIndex);
        }

        public int IndexOf(Texture item)
        {
            return Textures.IndexOf(item);
        }

        public void Insert(int index, Texture item)
        {
            Textures.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Textures.RemoveAt(index);
        }

        bool ICollection<Texture>.Remove(Texture item)
        {
            return Textures.Remove(item);
        }
    }
}
