using System;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour, Draggable{
    
    public  Material hoverMaterial;
    public Material originalMaterial;
    private MeshRenderer meshRenderer;
    public Coord coord;
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
        return Rules.canMoveTile(game.board,coord);
    }

    public Tile getTile(){
        return game.board.coordToTile(coord);
    }

    internal void Highlight()
    {
        hoverMaterial = originalMaterial;
        Color color = hoverMaterial.color;
        meshRenderer.material = hoverMaterial; 
    }
}
