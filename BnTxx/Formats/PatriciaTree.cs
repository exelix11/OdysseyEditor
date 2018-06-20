using BnTxx.Utilities;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace BnTxx.Formats
{
    class PatriciaTree : IEnumerable<string>
    {
        private List<PatriciaTreeNode> Nodes;

        private List<string> Names;

        public int Count { get { return Names.Count; } }

        public PatriciaTree()
        {
            Nodes = new List<PatriciaTreeNode> { new PatriciaTreeNode() };

            Names = new List<string>();
        }

        public PatriciaTree(BinaryReader Reader) : this()
        {
            string DictSignature = Reader.ReadString(4);

            if (DictSignature != "_DIC")
            {
                throw new InvalidSignatureException("_DIC", DictSignature);
            }

            Nodes.Clear();

            int NodesCount = Reader.ReadInt32();

            long Position = Reader.BaseStream.Position;

            for (int Index = 0; Index < NodesCount + 1; Index++)
            {
                Reader.BaseStream.Seek(Position + Index * 0x10, SeekOrigin.Begin);

                uint   ReferenceBit   = Reader.ReadUInt32();
                ushort LeftNodeIndex  = Reader.ReadUInt16();
                ushort RightNodeIndex = Reader.ReadUInt16();
                int    NameAddress    = Reader.ReadInt32();
                int    DataAddress    = Reader.ReadInt32(); //Uninitialized?

                Reader.BaseStream.Seek(NameAddress, SeekOrigin.Begin);

                string Name = Reader.ReadShortString();

                Nodes.Add(new PatriciaTreeNode()
                {
                    ReferenceBit   = ReferenceBit,
                    LeftNodeIndex  = LeftNodeIndex,
                    RightNodeIndex = RightNodeIndex,
                    Name           = Name
                });

                if (Index > 0)
                {
                    Names.Add(Name);
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Names.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
