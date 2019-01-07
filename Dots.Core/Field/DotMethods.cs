using System;

namespace Dots.Core.Field.Models
{
    public partial class Dot
    {
        public static bool operator !=(Dot dot1, Dot dot2)
        {
            return !(dot1 == dot2);
        }

        public static bool operator ==(Dot dot1, Dot dot2)
        {
            return dot1?.Value == dot2?.Value &&
                   dot1?.ChainValue == dot2?.ChainValue &&
                   dot1?.Active == dot2?.Active &&
                   dot1?.Chain == dot2?.Chain;
        }

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

        public override bool Equals(object obj)
        {
            if (!(obj is Dot dot))
                return false;
            else
                return dot == this;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Active, Chain, ChainValue, Closed, Value).GetHashCode();
        }
    }
}