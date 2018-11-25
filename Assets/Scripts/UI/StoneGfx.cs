
using UnityEngine;

public class StoneGfx : MonoBehaviour{
    
    private Stone stone;

    private void Awake()
    {
        stone = GetComponentInParent<Stone>();
    }

    internal void OnMouseEnter()
    {
        stone.OnMouseEnter();
    }

    internal void OnMouseExit()
    {
        stone.OnMouseExit();
    }
}
