using Dots.Core.Field;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dots.Tests
{
    [TestClass]
    public class DotTests
    {
        [TestMethod]
        public void Clone_Dot_SameValues()
        {
            // Arrange
            var dot = new Dot();

            // Act
            Dot newDot = dot.Clone();

            // Assert
            Assert.IsTrue(newDot.Value == dot.Value);
            Assert.IsTrue(newDot.Active == dot.Active);
            Assert.IsTrue(newDot.Chain == dot.Chain);
            Assert.IsTrue(newDot.Closed == dot.Closed);
        }
    }
}