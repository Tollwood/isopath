using NUnit.Framework;
using System;

namespace Application
{
    [TestFixture()]
    public class MoveStoneTest
    {
        [Test()]
        public void canMoveAnyWhere_initial()
        {
            Board board = BoardStateModifier.NewBoard(4);
            Assert.False(Rules.canMoveStoneAnyWhere(board,board.currentPlayer));
        }

        [Test()]
        public void canDiggerMoveAnyWhere_movable()
        {
            Board board = BoardStateModifier.NewBoard(4);
            board.tiles[3, 1] =new Tile(new Coord(3,1), TileLevel.UNDERGROUND);
            Assert.True(Rules.canMoveStoneAnyWhere(board,board.currentPlayer));
        }

        [Test()]
        public void canClimberMoveAnyWhere_movable()
        {
            Board board = BoardStateModifier.NewBoard(4);
            board.tiles[1, 5] = new Tile(new Coord(1,5), TileLevel.HILL);
            Assert.True(Rules.canMoveStoneAnyWhere(board,board.currentPlayer));
        }
    }
}
