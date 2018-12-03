using NUnit.Framework;
using System;

namespace Application
{
    [TestFixture()]
    public class TeleportTest
    {
        [Test()]
        public void teleportTop()
        {
            Board board = new Board(4);
            Tile leftTile = board.coordToTile(new Coord(-3, 3));
            leftTile.level = TileLevel.UNDERGROUND;
            Tile rightTile = board.coordToTile(new Coord(0, 3));
            leftTile.level = TileLevel.UNDERGROUND;
            leftTile.occupiat = true;
            leftTile.occupiatBy = Player.DIGGER;
            Assert.True(Rules.teleport(board, leftTile, rightTile));
        }


        [Test()]
        public void teleportMiddle()
        {
            Board board = new Board(4);
            Tile leftTile = board.coordToTile(new Coord(3, 0));
            leftTile.level = TileLevel.UNDERGROUND;
            Tile rightTile = board.coordToTile(new Coord(-3, 0));
            leftTile.level = TileLevel.UNDERGROUND;
            leftTile.occupiat = true;
            leftTile.occupiatBy = Player.DIGGER;
            Assert.True(Rules.teleport(board, leftTile, rightTile));
        }

        [Test()]
        public void teleportBottom()
        {
            Board board = new Board(4);
            Tile leftTile = board.coordToTile(new Coord(0, -3));
            leftTile.level = TileLevel.UNDERGROUND;
            leftTile.occupiat = true;
            leftTile.occupiatBy = Player.DIGGER;
            Tile rightTile = board.coordToTile(new Coord(3, -3));
            rightTile.level = TileLevel.UNDERGROUND;
            Assert.True(Rules.teleport(board, leftTile, rightTile));
        }

    }
}
