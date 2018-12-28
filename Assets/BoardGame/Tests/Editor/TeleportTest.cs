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
            
            Tile leftTile = new Tile(new Coord(0,6), TileLevel.UNDERGROUND);
            Tile rightTile = new Tile(new Coord(3,6), TileLevel.UNDERGROUND, Player.DIGGER);
            Assert.True(Rules.teleport(4, leftTile, rightTile));
        }


        [Test()]
        public void teleportMiddle()
        {
            Tile leftTile = new Tile(new Coord(6, 3), TileLevel.UNDERGROUND);
            Tile rightTile = new Tile(new Coord(0, 3), TileLevel.UNDERGROUND, Player.DIGGER);
            Assert.True(Rules.teleport(4, leftTile, rightTile));
        }

        [Test()]
        public void teleportBottom()
        {
            Tile leftTile = new Tile(new Coord(3, 0), TileLevel.UNDERGROUND);
            Tile rightTile = new Tile(new Coord(6, 0), TileLevel.UNDERGROUND, Player.DIGGER);
            Assert.True(Rules.teleport(4, leftTile, rightTile));
        }

    }
}
