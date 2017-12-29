using System;
using Dots.Core.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dots.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Initialize_RightSize_EmptyField()
        {
            // Arrenge
            var game = new Game();

            // Act
            game.Initialyze(5);

            // Assert
            Assert.IsTrue(game.FirstPlayerMove);
            Assert.IsTrue(game.Result.FirstPlayerScore == 0);
            Assert.IsTrue(game.Result.SecondPlayerScore == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MakeMove_WrongIndexes_Exception()
        {
            // Arrenge
            var game = new Game();

            // Act
            game.Initialyze(5);
            game.MakeMove(-1, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MakeMove_NotEmptyCell_Exception()
        {
            // Arrenge
            var game = new Game();

            // Act
            game.Initialyze(3);
            game.MakeMove(1, 1);
            game.MakeMove(1, 1);
        }

        [TestMethod]
        public void MakeMove_FewMoves_Result04()
        {
            // Arrenge
            var game = new Game();

            // Act
            game.Initialyze(10);
            game.MakeMove(2, 3);
            game.MakeMove(3, 3);
            game.MakeMove(3, 2);
            game.MakeMove(1, 3);
            game.MakeMove(3, 4);
            game.MakeMove(2, 2);
            game.MakeMove(4, 3);
            game.MakeMove(2, 4);
            game.MakeMove(1, 9);
            game.MakeMove(3, 1);
            game.MakeMove(2, 9);
            game.MakeMove(3, 5);
            game.MakeMove(3, 9);
            game.MakeMove(4, 2);
            game.MakeMove(4, 9);
            game.MakeMove(4, 4);
            game.MakeMove(5, 9);
            game.MakeMove(5, 3);

            // Assert
            Assert.IsTrue(game.Result.FirstPlayerScore == 0);
            Assert.IsTrue(game.Result.SecondPlayerScore == 4);
        }
    }
}
