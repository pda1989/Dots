namespace Dots.Core.Field
{
    public class Dot
    {
        #region Properties

        // 0 - empty dot, 1..n - player's dot
        public byte Value { get; set; }
        // The dot is in the chain
        public bool Chain { get; set; }
        // The dot is active (it's not inside a chain)
        public bool Active { get; set; } = true;
        // The dot can be inside a chain
        public bool Closed { get; set; }
        // The dot inside the chain with this number 
        public byte ChainValue { get; set; }

        #endregion

        #region Methods

        public Dot Clone()
        {
            return new Dot
            {
                Active = Active,
                Chain = Chain,
                Closed = Closed,
                Value = Value,
                ChainValue = ChainValue
            };
        }

        #endregion
    }
}