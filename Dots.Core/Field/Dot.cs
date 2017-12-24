namespace Dots.Core.Field
{
    public class Dot
    {
        #region Properties

        public byte Value { get; set; }
        public bool Chain { get; set; }
        public bool Active { get; set; } = true;
        public bool Closed { get; set; }

        #endregion

        #region Methods

        public Dot Clone()
        {
            return new Dot
            {
                Active = Active,
                Chain = Chain,
                Closed = Closed,
                Value = Value
            };
        }

        #endregion
    }
}