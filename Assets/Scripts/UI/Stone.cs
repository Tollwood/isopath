
using UnityEngine;

public class Stone : MonoBehaviour, Draggable{

    public  Material hoverMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    public Coord coord;
    private Board board;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        originalMaterial = meshRenderer.material;
        board = FindObjectOfType<Game>().board;
    }

    internal void OnMouseEnter()
    {
        //if(board.canMoveStone(coord)){
        meshRenderer.material = hoverMaterial;
        //}
    }

    internal void OnMouseExit()
    {
        meshRenderer.material = originalMaterial;
    }

    public void SetOriginalMaterial(Material originalMaterial){
        this.originalMaterial = originalMaterial;
        meshRenderer.material = originalMaterial;
    }

    public bool isDraggable()
    {
        return board.canMoveStone(coord);
    }

    public Tile getTile(){
        return board.coordToTile(coord);
    }
}
