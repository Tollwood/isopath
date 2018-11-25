using UnityEngine;

public class Selector: MonoBehaviour {

    public Material selected;
    private Transform currentlySelected;
    private Material currentlySelectedMaterial;

    public void ColorizeSelection(Transform clickedObject)
    {
        ResetCurrentSelcted();
        currentlySelected = clickedObject;
        Material currentMaterial = clickedObject.GetComponent<MeshRenderer>().material;
        if (currentMaterial != selected)
        {
            currentlySelectedMaterial = currentMaterial;
        }
        clickedObject.GetComponent<MeshRenderer>().material = selected;
    }

    private void ResetCurrentSelcted()
    {
        if (currentlySelected != null)
        {
            currentlySelected.GetComponent<MeshRenderer>().material = currentlySelectedMaterial;
            currentlySelectedMaterial = null;
            currentlySelected = null;
        }
    }
}
