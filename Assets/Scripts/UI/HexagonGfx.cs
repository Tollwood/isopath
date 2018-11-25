
using UnityEngine;

public class HexagonGfx : MonoBehaviour{
    
    private Hexagon hexagon;

    private void Awake()
    {
        hexagon = GetComponentInParent<Hexagon>();
    }

    internal void OnMouseEnter()
    {
        hexagon.OnMouseEnter();
    }

    internal void OnMouseExit()
    {
        hexagon.OnMouseExit();
    }
}
