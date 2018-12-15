[System.Serializable]
public class Settings
{
    public Settings(int size, bool aiDigger, bool aiClimber){
        this.size = size;
        this.aiDigger = aiDigger;
        this.aiClimber = aiClimber;
    }

    public int size;
    public bool aiDigger;
    public bool aiClimber;
}