using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Application
{
    [TestFixture()]
    public class RulesTest
    {
        [Test()]
        public void GetNeighbor_top_left()
        {
            Board board = BoardStateModifier.NewBoard(4);
            List<Tile> neighbors = Rules.GetNeighbors(board.tiles, board.size, board.tiles[3, 0]);
            Assert.True(neighbors.Count == 3);
        }

        [Test()]
        public void GetNeighbor_top_right()
        {
            Board board = BoardStateModifier.NewBoard(4);
            List<Tile> neighbors = Rules.GetNeighbors(board.tiles, board.size, board.tiles[6, 0]);
            Assert.True(neighbors.Count == 3);
        }

        [Test()]
        public void GetNeighbor_middle_left()
        {
            Board board = BoardStateModifier.NewBoard(4);
            List<Tile> neighbors = Rules.GetNeighbors(board.tiles, board.size, board.tiles[0, 3]);
            Assert.True(neighbors.Count == 3);
        }

        [Test()]
        public void GetNeighbor_middle_right()
        {
            Board board = BoardStateModifier.NewBoard(4);
            List<Tile> neighbors = Rules.GetNeighbors(board.tiles, board.size, board.tiles[6, 3]);
            Assert.True(neighbors.Count == 3);
        }

        [Test()]
        public void GetNeighbor_bottom_left()
        {
            Board board = BoardStateModifier.NewBoard(4);
            List<Tile> neighbors = Rules.GetNeighbors(board.tiles,board.size, board.tiles[0, 6]);
            Assert.True(neighbors.Count == 3);
        }

        [Test()]
        public void GetNeighbor_bottom_right()
        {
            Board board = BoardStateModifier.NewBoard(4);
            List<Tile> neighbors = Rules.GetNeighbors(board.tiles, board.size, board.tiles[3, 6]);
            Assert.True(neighbors.Count == 3);
        }

    }
}
