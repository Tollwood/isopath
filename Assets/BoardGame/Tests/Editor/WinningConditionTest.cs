using NUnit.Framework;
using System;

namespace Application
{
    [TestFixture()]
    public class WinningConditionTest
    {
        [Test()]
        public void no_winner_on_new_game(){
            Board board = BoardStateModifier.NewBoard(4);
            Assert.Null(Rules.CheckWinningCondition(board));
        }

        [Test()]
        public void digger_wins()
        {
            Board board = BoardStateModifier.NewBoard(4);
            Coord coord = new Coord(3, Rules.homeLine(Player.CLIMBER, 4));
            board.tiles[coord.q, coord.r] = new Tile(coord, TileLevel.UNDERGROUND, Player.DIGGER);
            Assert.True(Rules.CheckWinningCondition(board) == Player.DIGGER);
        }

        [Test()]
        public void climber_wins()
        {
            Board board = BoardStateModifier.NewBoard(4);
            Coord coord = new Coord(3, Rules.homeLine(Player.DIGGER,4));
            board.tiles[coord.q, coord.r] = new Tile(coord, TileLevel.HILL, Player.CLIMBER);
            Assert.True(Rules.CheckWinningCondition(board) == Player.CLIMBER);
        }
    }
}
