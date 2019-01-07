using Dots.Core.Field.Models;
using NUnit.Framework;

namespace Dots.Tests
{
    [TestFixture]
    public class DotTests
    {
        [Test]
        public void Clone_Dot_SameValues()
        {
            var dot = new Dot();

            Dot newDot = dot.Clone();

            Assert.IsTrue(newDot.Value == dot.Value);
            Assert.IsTrue(newDot.Active == dot.Active);
            Assert.IsTrue(newDot.Chain == dot.Chain);
            Assert.IsTrue(newDot.Closed == dot.Closed);
        }
    }
}