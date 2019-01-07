namespace Dots.Core.Field.Models
{
    public partial class Dot
    {
        // The dot is active (it's not inside a chain)
        public bool Active { get; set; } = true;

        // The dot is in the chain
        public bool Chain { get; set; }

        // The dot inside the chain with this number
        public byte ChainValue { get; set; }

        // The dot can be inside a chain
        public bool Closed { get; set; }

        // 0 - empty dot, 1..n - player's dot
        public byte Value { get; set; }
    }
}