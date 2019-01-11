using Dots.Core.Game;
using NUnit.Framework;
using System;

namespace Dots.Tests
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void Initialize_WithRightSize_ReturnsEmptyField()
        {
            var game = new Game();

            game.Initialyze(5);

            Assume.That(game.FirstPlayerMove, Is.True);
            Assume.That(game.Result.FirstPlayerScore, Is.EqualTo(0));
            Assume.That(game.Result.SecondPlayerScore, Is.EqualTo(0));
        }

        [Test]
        public void MakeMove_MadeFewMoves_Result04()
        {
            var game = new Game();

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

            Assume.That(game.Result.FirstPlayerScore, Is.EqualTo(0));
            Assume.That(game.Result.SecondPlayerScore, Is.EqualTo(4));
        }

        [Test]
        public void MakeMove_ToCellWithWrongIndexes_ThrowsArgumentOutOfRangeExceptionException()
        {
            var game = new Game();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                game.Initialyze(5);
                game.MakeMove(-1, -1);
            });
        }

        [Test]
        public void MakeMove_ToNotEmptyCell_ThrowsArgumentExceptionException()
        {
            var game = new Game();

            Assert.Throws<ArgumentException>(() =>
            {
                game.Initialyze(3);
                game.MakeMove(1, 1);
                game.MakeMove(1, 1);
            });
        }
    }
}