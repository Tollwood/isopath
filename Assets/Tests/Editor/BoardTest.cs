using NUnit.Framework;
using System;

namespace Application
{
    [TestFixture()]
    public class BoardTest
    {
        [Test()]
        public void initBoardWith4Tiles()
        {
            Board board = new Board(4);

            Assert.AreEqual(37,board.tiles.Count);

            AssertTile(board, new Coord(0, -3), TileLevel.HILL, true);
            AssertTile(board, new Coord(1, -3), TileLevel.HILL, true);
            AssertTile(board, new Coord(2, -3), TileLevel.HILL, true);
            AssertTile(board, new Coord(3, -3), TileLevel.HILL, true);

            AssertTile(board, new Coord(0, 3), TileLevel.UNDERGROUND, true);
            AssertTile(board, new Coord(-1, 3), TileLevel.UNDERGROUND, true);
            AssertTile(board, new Coord(-2, 3), TileLevel.UNDERGROUND, true);
            AssertTile(board, new Coord(-3, 3), TileLevel.UNDERGROUND, true);

        }

        [Test()]
        public void initBoardWith5Tiles()
        {
            Board board = new Board(5);
            Assert.AreEqual(61, board.tiles.Count);
        }

        [Test()]
        public void initBoardWith3Tiles()
        {
            Board board = new Board(3);
            Assert.AreEqual(19, board.tiles.Count);
        }

        [Test()]
        public void initBoardWith2Tiles()
        {
            Board board = new Board(2);
            Assert.AreEqual(7, board.tiles.Count);
        }

        [Test()]
        public void buildUndergroundOnUnderground()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            from.level = TileLevel.UNDERGROUND;
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.UNDERGROUND;

            Assert.False(board.build(from, to));
            Assert.That(from.level == TileLevel.UNDERGROUND);
            Assert.That(to.level == TileLevel.UNDERGROUND);
        }

        [Test()]
        public void buildUndergroundOnGround()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            from.level = TileLevel.UNDERGROUND;
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.GROUND;

            Assert.False(board.build(from, to));
            Assert.That(from.level == TileLevel.UNDERGROUND);
            Assert.That(to.level == TileLevel.GROUND);
        }

        [Test()]
        public void buildUndergroundOnHill()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            from.level = TileLevel.UNDERGROUND;
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.HILL;

            Assert.False(board.build(from, to));
            Assert.That(from.level == TileLevel.UNDERGROUND);
            Assert.That(to.level == TileLevel.HILL);
        }


        [Test()]
        public void buildGroundOnUnderground()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.UNDERGROUND;

            Assert.True(board.build(from, to));
            Assert.That(from.level == TileLevel.UNDERGROUND);
            Assert.That(to.level == TileLevel.GROUND);
        }

        [Test()]
        public void buildGroundOnGround()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            Tile to= board.coordToTile(toCoord);

            Assert.True(board.build(from, to));
            Assert.That(from.level == TileLevel.UNDERGROUND);
            Assert.That(to.level == TileLevel.HILL);
        }

        [Test()]
        public void buildGroundOnHill()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.HILL;

            Assert.False(board.build(from, to));
            Assert.That(from.level == TileLevel.GROUND);
            Assert.That(to.level == TileLevel.HILL);
        }

        [Test()]
        public void buildHillOnUnderground()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            from.level = TileLevel.HILL;
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.UNDERGROUND;

            Assert.True(board.build(from, to));
            Assert.That(from.level == TileLevel.GROUND);
            Assert.That(to.level == TileLevel.GROUND);
        }

        [Test()]
        public void buildHillOnGround()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            from.level = TileLevel.HILL;
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.GROUND;

            Assert.True(board.build(from, to));
            Assert.That(from.level == TileLevel.GROUND);
            Assert.That(to.level == TileLevel.HILL);
        }

        [Test()]
        public void buildHillOnHill()
        {
            Board board = new Board(4);
            Coord fromCoord = new Coord(0, 0);
            Coord toCoord = new Coord(0, 1);

            Tile from = board.coordToTile(fromCoord);
            from.level = TileLevel.HILL;
            Tile to = board.coordToTile(toCoord);
            to.level = TileLevel.HILL;

            Assert.False(board.build(from, to));
            Assert.That(from.level == TileLevel.HILL);
            Assert.That(to.level == TileLevel.HILL);
        }

        [Test()]
        public void canMoveAnyWhere_initial()
        {
            Board board = new Board(4);
            Assert.False(board.canMoveStoneAnyWhere());
        }

        [Test()]
        public void canDiggerMoveAnyWhere_movable()
        {
            Board board = new Board(4);
            Tile tile = board.coordToTile(new Coord(-2, 2));
            tile.level = TileLevel.UNDERGROUND;
            Assert.True(board.canMoveStoneAnyWhere());
        }

        [Test()]
        public void canClimberMoveAnyWhere_movable()
        {
            Board board = new Board(4);
            board.currentPlayer = Player.CLIMBER;
            Tile tile = board.coordToTile(new Coord(1, -2));
            tile.level = TileLevel.HILL;
            Assert.True(board.coordToTile(new Coord(2, -3)).occupiatBy == Player.CLIMBER);
            Assert.True(board.canMoveStoneAnyWhere());
        }


        private void AssertTile(Board board, Coord coord, TileLevel tileLevel, bool occupiat)
        {
            Assert.True(board.tiles.ContainsKey(coord), coord + " not found");
            Tile tile = board.tiles[coord];
            Assert.That(tile.level == tileLevel, "Tile at "+ coord+ " does not have expected "+ tileLevel);
            Assert.That(tile.occupiat == occupiat);
        }
    }
}
