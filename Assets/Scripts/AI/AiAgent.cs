
public class AiAgent
{
    private Player player;

    public AiAgent(Player player){
        this.player = player;
    }

    public Move build(Board board){
        return AiCalculator.calcBuild(board);
    }

    public Move moveStone(Board board){
        return null;
    }
}