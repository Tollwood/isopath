
using System;

public class ScoredMove : IComparable
{
    public Tile buildFrom { get; private set; }
    public Tile buildTo { get; private set; }
    public Tile moveFrom { get; private set; }
    public Tile moveTo { get; private set; }
    public Tile captureStone { get; private set; }
    public int score { get; private set; }

    protected ScoredMove(Tile buildFrom, Tile buildTo, Tile moveFrom, Tile moveTo, Tile captureStone, int score) {
        this.score = score;
        this.captureStone = captureStone;
        this.moveTo = moveTo;
        this.moveFrom = moveFrom;
        this.buildTo = buildTo;
        this.buildFrom = buildFrom;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        ScoredMove otherMove = obj as ScoredMove;
        if (otherMove != null)
            return this.score.CompareTo(otherMove.score);
        else
            throw new ArgumentException("Object is not a Move");
    }


    public static ScoredMoveBuilder Builder()
    {
        return new ScoredMoveBuilder();
    }

    public class ScoredMoveBuilder
    {
        private Tile buildFrom = null;
        private Tile buildTo = null;
        private Tile moveFrom = null;
        private Tile moveTo = null;
        private Tile captureStone = null;
        private int score = 0;

        public ScoredMoveBuilder withBuildStep(Tile buildFrom, Tile buildTo)
        {
            this.buildFrom = buildFrom;
            this.buildTo = buildTo;
            return this;
        }

        public ScoredMoveBuilder addToScore(int score)
        {
            this.score += score;
            return this;
        }
        public ScoredMoveBuilder withMoveStep(Tile moveFrom, Tile moveTo)
        {
            this.moveFrom = moveFrom;
            this.moveTo = moveTo;
            return this;
        }

        public ScoredMoveBuilder withCaptureStep(Tile captureFrom)
        {
            this.captureStone = captureFrom;
            return this;
        }

        public ScoredMove build()
        {
            return new ScoredMove(buildFrom, buildTo, moveFrom, moveTo, captureStone, score);
        }
    }
}