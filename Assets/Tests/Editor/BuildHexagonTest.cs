//using NUnit.Framework;
//using System;

//namespace Application
//{
//    [TestFixture()]
//    public class BuildHexagonTest
//    {
//        [Test()]
//        public void buildUndergroundOnUnderground()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            from.level = TileLevel.UNDERGROUND;
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.UNDERGROUND;

//            Assert.False(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.UNDERGROUND);
//            Assert.That(to.level == TileLevel.UNDERGROUND);
//        }

//        [Test()]
//        public void buildUndergroundOnGround()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            from.level = TileLevel.UNDERGROUND;
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.GROUND;

//            Assert.False(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.UNDERGROUND);
//            Assert.That(to.level == TileLevel.GROUND);
//        }

//        [Test()]
//        public void buildUndergroundOnHill()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            from.level = TileLevel.UNDERGROUND;
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.HILL;

//            Assert.False(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.UNDERGROUND);
//            Assert.That(to.level == TileLevel.HILL);
//        }


//        [Test()]
//        public void buildGroundOnUnderground()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.UNDERGROUND;

//            Assert.True(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.UNDERGROUND);
//            Assert.That(to.level == TileLevel.GROUND);
//        }

//        [Test()]
//        public void buildGroundOnGround()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            Tile to= board.coordToTile(toCoord);

//            Assert.True(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.UNDERGROUND);
//            Assert.That(to.level == TileLevel.HILL);
//        }

//        [Test()]
//        public void buildGroundOnHill()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.HILL;

//            Assert.False(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.GROUND);
//            Assert.That(to.level == TileLevel.HILL);
//        }

//        [Test()]
//        public void buildHillOnUnderground()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            from.level = TileLevel.HILL;
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.UNDERGROUND;

//            Assert.True(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.GROUND);
//            Assert.That(to.level == TileLevel.GROUND);
//        }

//        [Test()]
//        public void buildHillOnGround()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            from.level = TileLevel.HILL;
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.GROUND;

//            Assert.True(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.GROUND);
//            Assert.That(to.level == TileLevel.HILL);
//        }

//        [Test()]
//        public void buildHillOnHill()
//        {
//            Board board = new Board(4);
//            Coord fromCoord = new Coord(0, 0);
//            Coord toCoord = new Coord(0, 1);

//            Tile from = board.coordToTile(fromCoord);
//            from.level = TileLevel.HILL;
//            Tile to = board.coordToTile(toCoord);
//            to.level = TileLevel.HILL;

//            Assert.False(Rules.build(board,from, to));
//            Assert.That(from.level == TileLevel.HILL);
//            Assert.That(to.level == TileLevel.HILL);
//        }

//        [Test()]
//        public void canNotBuildFromDiggerHomeRow()
//        {
//            Board board = new Board(4);
//            Coord coord = new Coord(0, 3);
//            Tile tile = board.coordToTile(coord);
//            Assert.True(tile.occupiatBy == Player.DIGGER);
//            tile.occupiatBy = null;
//            tile.occupiat = false;

//            Tile zeroTile = board.coordToTile(new Coord(0, 0));
//            Assert.False(Rules.build(board,tile, zeroTile));
//        }

//        [Test()]
//        public void canNotbuildFromClimberHomeRow()
//        {
//            Board board = new Board(4);
//            board.currentPlayer = Player.CLIMBER;
//            Coord coord = new Coord(0, -3);
//            Tile tile = board.coordToTile(coord);
//            Assert.True(tile.occupiatBy == Player.CLIMBER);
//            tile.occupiatBy = null;
//            tile.occupiat = false;

//            Tile zeroTile = board.coordToTile(new Coord(0, 0));
//            Assert.False(Rules.build(board,tile,zeroTile));
//        }

//        [Test()]
//        public void canNotbuildOnSameTile()
//        {
//            Board board = new Board(4);
//            board.currentPlayer = Player.CLIMBER;
//            Coord coord = new Coord(0, 0);
//            Tile tile = board.coordToTile(coord);

//            Assert.False(Rules.build(board, tile, tile));
//        }

//        [Test()]
//        public void canNotMoveTileInHomeRowDigger()
//        {
//            Board board = new Board(4);
//            Coord coord = new Coord(0, 3);

//            Assert.False(Rules.canMoveTile(board, coord));
//        }

//        [Test()]
//        public void canNotMoveTileInHomeRowClimber()
//        {
//            Board board = new Board(4);
//            board.currentPlayer = Player.CLIMBER;
//            Coord coord = new Coord(0, -3);

//            Assert.False(Rules.canMoveTile(board, coord));
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
