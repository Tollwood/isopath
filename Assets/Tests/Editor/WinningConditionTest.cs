using NUnit.Framework;
using System;

namespace Application
{
    [TestFixture()]
    public class WinningConditionTest
    {
        [Test()]
        public void no_winner_on_new_game(){

            Board board = new Board(4);
            Assert.Null(Rules.CheckWinningCondition(board));
        }

        [Test()]
        public void digger_wins()
        {
            Board board = new Board(4);
            Coord coord = new Coord(0, -3);
            Tile winningTile = board.coordToTile(coord);
            winningTile.level = TileLevel.UNDERGROUND;
            winningTile.occupiat = true;
            winningTile.occupiatBy = Player.DIGGER;
            Assert.True(Rules.CheckWinningCondition(board) == Player.DIGGER);
        }

        [Test()]
        public void climber_wins()
        {
            Board board = new Board(4);
            Coord coord = new Coord(0, 3);
            Tile winningTile = board.coordToTile(coord);
            winningTile.level = TileLevel.HILL;
            winningTile.occupiat = true;
            winningTile.occupiatBy = Player.CLIMBER;
            Assert.True(Rules.CheckWinningCondition(board) == Player.CLIMBER);
        }
    }
}
