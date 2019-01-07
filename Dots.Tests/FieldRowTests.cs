using Dots.Core.Field.Models;
using NUnit.Framework;
using System;

namespace Dots.Tests
{
    [TestFixture]
    public class FieldRowTests
    {
        [Test]
        public void FieldRow_RightSize_InitializedDots()
        {
            var fieldRow = new FieldRow(2);

            Assert.IsNotNull(fieldRow[0]);
            Assert.IsNotNull(fieldRow[1]);
        }

        [Test]
        public void FieldRow_WrongSize_Exception()
        {
            Assert.Throws<ArgumentException>(() => new FieldRow(-1));
        }

        [Test]
        public void ThisRead_WrongIndex_Exception()
        {
            var fieldRow = new FieldRow(2);

            Assert.Throws<ArgumentOutOfRangeException>(() => { var dot = fieldRow[-1]; });
        }

        [Test]
        public void ThisWrite_RightIndex_InitializedDots()
        {
            var fieldRow = new FieldRow(2);

            fieldRow[0] = new Dot { Value = 10 };

            Assert.IsTrue(fieldRow[0].Value == 10);
        }

        [Test]
        public void ThisWrite_WrongIndex_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FieldRow(2) { [-1] = new Dot() });
        }
    }
}