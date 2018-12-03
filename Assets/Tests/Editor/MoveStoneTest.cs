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
            Board board = new Board(4);
            Assert.False(Rules.canMoveStoneAnyWhere(board));
        }

        [Test()]
        public void canDiggerMoveAnyWhere_movable()
        {
            Board board = new Board(4);
            Tile tile = board.coordToTile(new Coord(-2, 2));
            tile.level = TileLevel.UNDERGROUND;
            Assert.True(Rules.canMoveStoneAnyWhere(board));
        }

        [Test()]
        public void canClimberMoveAnyWhere_movable()
        {
            Board board = new Board(4);
            board.currentPlayer = Player.CLIMBER;
            Tile tile = board.coordToTile(new Coord(1, -2));
            tile.level = TileLevel.HILL;
            Assert.True(board.coordToTile(new Coord(2, -3)).occupiatBy == Player.CLIMBER);
            Assert.True(Rules.canMoveStoneAnyWhere(board));
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
