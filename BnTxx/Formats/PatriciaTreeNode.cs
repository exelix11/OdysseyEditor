namespace BnTxx.Formats
{
    class PatriciaTreeNode
    {
        public uint   ReferenceBit   { get; set; }
        public ushort LeftNodeIndex  { get; set; }
        public ushort RightNodeIndex { get; set; }
        public string Name           { get; set; }
    }
}
