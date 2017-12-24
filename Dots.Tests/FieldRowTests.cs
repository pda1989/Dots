using System;
using Dots.Core.Field;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dots.Tests
{
    [TestClass]
    public class FieldRowTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldRow_WrongSize_Exception()
        {
            var fieldRow = new FieldRow(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThisRead_WrongIndex_Exception()
        {
            var fieldRow = new FieldRow(2);

            Dot dot = fieldRow[-1];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThisWrite_WrongIndex_Exception()
        {
            var fieldRow = new FieldRow(2) {[-1] = new Dot()};
        }

        [TestMethod]
        public void FieldRow_RightSize_InitializedDots()
        {
            // Arrange
            var fieldRow = new FieldRow(2);

            // Assert
            Assert.IsNotNull(fieldRow[0]);
            Assert.IsNotNull(fieldRow[1]);
        }

        [TestMethod]
        public void ThisWrite_RightIndex_InitializedDots()
        {
            // Arrange
            var fieldRow = new FieldRow(2);

            // Act
            fieldRow[0] = new Dot {Value = 10};

            // Assert
            Assert.IsTrue(fieldRow[0].Value == 10);
        }
    }
}
