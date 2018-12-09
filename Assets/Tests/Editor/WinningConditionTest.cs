//using NUnit.Framework;
//using System;

//namespace Application
//{
//    [TestFixture()]
//    public class WinningConditionTest
//    {
//        [Test()]
//        public void no_winner_on_new_game(){

//            Board board = new Board(4);
//            Assert.Null(Rules.CheckWinningCondition(board));
//        }

//        [Test()]
//        public void digger_wins()
//        {
//            Board board = new Board(4);
//            Coord coord = new Coord(0, -3);
//            Tile winningTile = board.tiles[coord.q,coord.r];
//            Tile newTile = new Tile(winningTile.coord,TileLevel.UNDERGROUND,Player.DIGGER);
//            board.tiles[winningTile.coord.q, winningTile.coord.r] = newTile;
//            Assert.True(Rules.CheckWinningCondition(board) == Player.DIGGER);
//        }

//        [Test()]
//        public void climber_wins()
//        {
//            Board board = new Board(4);
//            Coord coord = new Coord(0, 3);
//            Tile winningTile = board.tiles[coord.q, coord.r];
//            Tile newTile = new Tile(winningTile.coord, TileLevel.HILL, Player.CLIMBER);
//            board.tiles[winningTile.coord.q, winningTile.coord.r] = newTile;
//            Assert.True(Rules.CheckWinningCondition(board) == Player.CLIMBER);
//        }
//    }
//}
