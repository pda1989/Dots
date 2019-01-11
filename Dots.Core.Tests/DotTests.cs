using Dots.Core.Field.Models;
using NUnit.Framework;

namespace Dots.Tests
{
    [TestFixture]
    public class DotTests
    {
        [Test]
        public void Clone_Dot_ReturnsNewDotWithSameValues()
        {
            var dot = new Dot();

            Dot newDot = dot.Clone();

            Assume.That(newDot, Is.Not.SameAs(dot));
            Assume.That(newDot.Value, Is.EqualTo(dot.Value));
            Assume.That(newDot.Active, Is.EqualTo(dot.Active));
            Assume.That(newDot.Chain, Is.EqualTo(dot.Chain));
            Assume.That(newDot.Closed, Is.EqualTo(dot.Closed));
            Assume.That(newDot.ChainValue, Is.EqualTo(dot.ChainValue));
        }

        [Test]
        public void Equals_ForDotsWithDifferentValues_ReturnsFalse()
        {
            var dot1 = new Dot { Value = 1 };
            var dot2 = new Dot { Value = 2 };

            bool result = dot1.Equals(dot2);

            Assume.That(result, Is.False);
        }

        [Test]
        public void Equals_ForDotsWithSameValues_ReturnsTrue()
        {
            var dot1 = new Dot();
            var dot2 = new Dot();

            bool result = dot1.Equals(dot2);

            Assume.That(result, Is.True);
        }

        [Test]
        public void Equals_ForSameDot_ReturnsTrue()
        {
            var dot1 = new Dot();
            var dot2 = dot1;

            bool result = dot1.Equals(dot2);

            Assume.That(result, Is.True);
        }
    }
}