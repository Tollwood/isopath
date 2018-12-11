
public class AiAgent
{
    private Player player;
    private int seed;

    public AiAgent(Player player){
        this.player = player;
        this.seed = 1;
    }

    public Moves GetNextMove(Board board){
        return AiCalculator.BestMove(board, seed);
    }

}