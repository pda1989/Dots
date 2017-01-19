namespace Dots
{
    public class Dot : IDot
    {
        public byte Value { get; set; } = 0;
        public bool Chain { get; set; } = false;
        public bool Active { get; set; } = true;
        public bool Closed { get; set; } = false;
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
        public override string ToString() => 
            $"{(Active? "" : "{")}{(Chain? "[" : "")}{Value}{(Chain? "]" : "")}{(Active? "" : "}")}{(Closed? "*" : "")}";
    }
}
