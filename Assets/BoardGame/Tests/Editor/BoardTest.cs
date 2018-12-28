//using NUnit.Framework;
//using System;

//namespace Application
//{
//    [TestFixture()]
//    public class BoardTest
//    {
//        [Test()]
//        public void initBoardWith4Tiles()
//        {
//            Board board = new Board(4);

//            Assert.AreEqual(37,board.tiles.Count);

//            AssertTile(board, new Coord(0, -3), TileLevel.HILL, true);
//            AssertTile(board, new Coord(1, -3), TileLevel.HILL, true);
//            AssertTile(board, new Coord(2, -3), TileLevel.HILL, true);
//            AssertTile(board, new Coord(3, -3), TileLevel.HILL, true);

//            AssertTile(board, new Coord(0, 3), TileLevel.UNDERGROUND, true);
//            AssertTile(board, new Coord(-1, 3), TileLevel.UNDERGROUND, true);
//            AssertTile(board, new Coord(-2, 3), TileLevel.UNDERGROUND, true);
//            AssertTile(board, new Coord(-3, 3), TileLevel.UNDERGROUND, true);

//        }

//        [Test()]
//        public void initBoardWith5Tiles()
//        {
//            Board board = new Board(5);
//            Assert.AreEqual(61, board.tiles.Count);
//        }

//        [Test()]
//        public void initBoardWith3Tiles()
//        {
//            Board board = new Board(3);
//            Assert.AreEqual(19, board.tiles.Count);
//        }

//        [Test()]
//        public void initBoardWith2Tiles()
//        {
//            Board board = new Board(2);
//            Assert.AreEqual(7, board.tiles.Count);
//        }

//        private void AssertTile(Board board, Coord coord, TileLevel tileLevel, bool occupiat)
//        {
//            Assert.True(board.tiles.ContainsKey(coord), coord + " not found");
//            Tile tile = board.tiles[coord];
//            Assert.That(tile.level == tileLevel, "Tile at "+ coord+ " does not have expected "+ tileLevel);
//            Assert.That(tile.occupiat == occupiat);
//        }
//    }
//}
