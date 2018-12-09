//using NUnit.Framework;
//using System;

//namespace Application
//{
//    [TestFixture()]
//    public class CaptureAndThreatenTest
//    {
//        [Test()]
//        public void should_threaten_digger(){

//            Board board = new Board(4);
//            Coord coord = new Coord(0, -2);
//            SetTileToDigger(board, coord);

//            Assert.True(Rules.IsThreaten(board, Player.DIGGER));
//        }

//        [Test()]
//        public void should_threaten_climber()
//        {

//            Board board = new Board(4);
//            Coord coord = new Coord(0, 2);
//            SetTileToClimber(board, coord);
//            Assert.True(Rules.IsThreaten(board, Player.CLIMBER));
//        }

//        [Test()]
//        public void should_be_able_to_capture()
//        {

//            Board board = new Board(4);
//            Coord coord = new Coord(0, 2);
//            SetTileToClimber(board, coord);

//            Assert.True(board.coordToTile(new Coord(0,3)).occupiatBy == Player.DIGGER);
//            Assert.True(board.coordToTile(new Coord(-1, 3)).occupiatBy == Player.DIGGER);
//            Assert.True(board.currentPlayer == Player.DIGGER);
//            Assert.True(Rules.CanCapture(board, coord));
//        }

//        [Test()]
//        public void should_be_able_to_capture_using_left_teleport()
//        {

//            Board board = new Board(4);
//            Coord coord = new Coord(-3, 0);
//            SetTileToClimber(board, coord);
//            SetTileToDigger(board, new Coord(3, 0));
//            SetTileToDigger(board, new Coord(-2, 0));
//            Assert.True(Rules.CanCapture(board, coord));
//        }

//        [Test()]
//        public void should_be_able_to_capture_using_right_teleport()
//        {

//            Board board = new Board(4);
//            Coord coord = new Coord(3, 0);
//            SetTileToClimber(board, coord);
//            SetTileToDigger(board, new Coord(-3, 0));
//            SetTileToDigger(board, new Coord(2, 0));
//            Assert.True(Rules.CanCapture(board, coord));
//        }

//        [Test()]
//        public void should_build_and_capture_climber()
//        {
//            Board board = new Board(4);
//            board.currentStep = Step.MOVE;
//            Coord coord = new Coord(0, 2);
//            SetTileToClimber(board, coord);
//            Assert.True(board.coordToTile(coord).occupiatBy == Player.CLIMBER);

//            Assert.True(Rules.Capture(board, coord));


//            Assert.True(board.currentStep == Step.BUILD);
//            Assert.True(board.currentPlayer == Player.CLIMBER);
//            Assert.Null(board.coordToTile(coord).occupiatBy);
//            Assert.False(board.coordToTile(coord).occupiat);
//        }



//        [Test()]
//        public void should_capture_climber_move()
//        {
//            Board board = new Board(4);
//            board.currentStep = Step.BUILD;
//            Coord coord = new Coord(0, 2);
//            SetTileToClimber(board, coord);
//            Assert.True(board.coordToTile(coord).occupiatBy == Player.CLIMBER);

//            Assert.True(Rules.Capture(board, coord));
//            Assert.True(board.currentStep == Step.MOVE);
//            Assert.True(board.currentPlayer == Player.DIGGER);
//            Assert.Null(board.coordToTile(coord).occupiatBy);
//            Assert.False(board.coordToTile(coord).occupiat);
//        }

//        private void SetTileToClimber(Board board, Coord coord)
//        {
//            Tile tileToCapture = board.coordToTile(coord);
//            tileToCapture.level = TileLevel.HILL;
//            tileToCapture.occupiat = true;
//            tileToCapture.occupiatBy = Player.CLIMBER;
//        }


//        private void SetTileToDigger(Board board, Coord coord)
//        {
//            Tile tile = board.coordToTile(coord);
//            tile.level = TileLevel.UNDERGROUND;
//            tile.occupiat = true;
//            tile.occupiatBy = Player.DIGGER;
//        }
//    }
//}
