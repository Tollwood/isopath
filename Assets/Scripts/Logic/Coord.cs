[System.Serializable]
public class Coord {
    public int r;
    public int q;

    public Coord(int q,int r){
        this.r = r;
        this.q = q;
    }

    public override bool Equals(object obj)
    {
        var coord = obj as Coord;
        return coord != null &&
               r == coord.r &&
               q == coord.q;
    }

    public override int GetHashCode()
    {
        var hashCode = -1147757127;
        hashCode = hashCode * -1521134295 + r.GetHashCode();
        hashCode = hashCode * -1521134295 + q.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return "q: " + q + " r: " + r ;
    }
}
