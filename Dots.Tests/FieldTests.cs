using System;
using Dots.Core.Field;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dots.Tests
{
    [TestClass]
    public class FieldTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Field_WrongSize_Exception()
        {
            var field = new Field(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThisRead_WrongIndex_Exception()
        {
            // Arrange 
            var field = new Field(2);

            // Act
            Dot dot = field[-1][0];
        }

        [TestMethod]
        public void Clone_Field_SameValues()
        {
            // Arrange
            var field = new Field(2);

            // Act
            var newField = field.Clone();

            // Assert
            Assert.IsTrue(field[0][0].Value == newField[0][0].Value);
            Assert.IsTrue(field[0][1].Value == newField[0][1].Value);
            Assert.IsTrue(field[1][0].Value == newField[1][0].Value);
            Assert.IsTrue(field[1][1].Value == newField[1][1].Value);
        }
    }
}
