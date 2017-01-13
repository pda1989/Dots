namespace Dots
{
    public class Dot
    {
        public byte Value { get; set; }
        public bool Chain { get; set; }
        public bool Active { get; set; }
        public bool Closed { get; set; }
        public Dot Clone()
        {
            return new Dot
            {
                Active = this.Active,
                Chain = this.Chain,
                Closed = this.Closed,
                Value = this.Value
            };
        }
    }
}
