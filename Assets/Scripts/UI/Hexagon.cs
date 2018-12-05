using System;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour, Draggable{
    
    public  Material hoverMaterial;
    public Material originalMaterial;
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

    public void OnMouseExit(){
        meshRenderer.material = originalMaterial;
    }

    public bool isDraggable()
    {
        return Rules.canMoveTile(board,coord);
    }

    public Tile getTile(){
        return board.coordToTile(coord);
    }

    internal void Highlight()
    {
        hoverMaterial = originalMaterial;
        Color color = hoverMaterial.color;
        meshRenderer.material = hoverMaterial; 
    }
}
