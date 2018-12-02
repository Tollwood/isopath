using UnityEngine;

public class Hexagon : MonoBehaviour, Draggable{

    public  Material hoverMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    public Coord coord;
    private Board board;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();   
    }

    private void Start()
    {
        originalMaterial = meshRenderer.material;
        board = FindObjectOfType<Game>().board;
    }

    public void SetOriginalMaterial(Material originalMaterial){
        this.originalMaterial = originalMaterial;
        meshRenderer.material = originalMaterial;
    }

    public void OnMouseEnter()
    {
        if(board.canMoveTile(coord)){
            meshRenderer.material = hoverMaterial;
            Debug.Log(coord);
        }

    }

    public void OnMouseExit(){
        meshRenderer.material = originalMaterial;
    }

    public bool isDraggable()
    {
        return board.canMoveTile(coord);
    }

    public Tile getTile(){
        return board.coordToTile(coord);
    }
}
