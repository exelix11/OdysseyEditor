namespace BnTxx.Utilities
{
    static class BitUtils
    {
        public static int Pow2RoundUp(int Value)
        {
            Value--;

            Value |= (Value >>  1);
            Value |= (Value >>  2);
            Value |= (Value >>  4);
            Value |= (Value >>  8);
            Value |= (Value >> 16);

            return ++Value;
        }

        public static int Pow2RoundDown(int Value)
        {
            return IsPow2(Value) ? Value : Pow2RoundUp(Value) >> 1;
        }

        public static bool IsPow2(int Value)
        {
            return Value != 0 && (Value & (Value - 1)) == 0;
        }

        public static int CountZeros(int Value)
        {
            int Count = 0;

            for (int i = 0; i < 32; i++)
            {
                if ((Value & (1 << i)) != 0)
                {
                    break;
                }

                Count++;
            }

            return Count;
        }
    }
}
