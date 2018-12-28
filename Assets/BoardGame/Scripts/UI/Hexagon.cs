using UnityEngine;

public class Hexagon : MonoBehaviour, Draggable{
    
    public  Material hoverMaterial;
    public Material originalMaterial;
    private MeshRenderer meshRenderer;
    public Tile tile;
    private Game game;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();   
    }

    private void Start()
    {
        originalMaterial = meshRenderer.material;
        game = FindObjectOfType<Game>();
    }

    public void SetOriginalMaterial(Material originalMaterial){
        this.originalMaterial = originalMaterial;
        meshRenderer.material = originalMaterial;
    }

    public void OnMouseExit(){
        meshRenderer.material = originalMaterial;
    }

    public bool isDraggable()
    {
        return Rules.canMoveTile(game.board,tile.coord);
    }

    internal void Highlight()
    {
        hoverMaterial = originalMaterial;
        Color color = hoverMaterial.color;
        meshRenderer.material = hoverMaterial; 
        Debug.Log(tile.coord);
    }

    public Coord GetCoord()
    {
        return tile.coord;
    }
}
