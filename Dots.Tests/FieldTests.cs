using Dots.Core.Field.Models;
using NUnit.Framework;
using System;

namespace Dots.Tests
{
    [TestFixture]
    public class FieldTests
    {
        [Test]
        public void Clone_Field_SameValues()
        {
            var field = new Field(2);

            var newField = field.Clone();

            Assert.IsTrue(field[0][0].Value == newField[0][0].Value);
            Assert.IsTrue(field[0][1].Value == newField[0][1].Value);
            Assert.IsTrue(field[1][0].Value == newField[1][0].Value);
            Assert.IsTrue(field[1][1].Value == newField[1][1].Value);
        }

        [Test]
        public void Field_WrongSize_Exception()
        {
            Assert.Throws<ArgumentException>(() => new Field(-1));
        }

        [Test]
        public void ThisRead_WrongIndex_Exception()
        {
            var field = new Field(2);

            Assert.Throws<ArgumentOutOfRangeException>(() => { Dot dot = field[-1][0]; });
        }
    }
}