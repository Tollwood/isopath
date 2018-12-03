﻿using UnityEngine;

public class MouseInput: MonoBehaviour {

    public new Camera camera;
    private Transform draggedObject;
    private Vector3 startPosition;
    private Plane dragPlane = new Plane(Vector3.up, new Vector3(0, 3, 0));
    private Vector3 dragScale = new Vector3(.5f, .5f, .5f);
    private Vector3 dragOffSet = new Vector3(0, 0, 1);
    private BoardUi boardUi;

    private void Start()
    {
        boardUi = FindObjectOfType<BoardUi>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        bool hitting = Physics.Raycast(ray, out hit);

        if(Input.GetMouseButtonDown(0) && hitting){
            startDragging(hit.transform);
        } else if (Input.GetMouseButtonUp(0) && isDragging()){
            if (hitting)
            {
                stopDragging(hit.transform);
            }

            draggedObject.transform.localScale = new Vector3(1, 1, 1);
            draggedObject = null;

        }
        else if(isDragging()){
            draggedObject.position = GetDragPosition();
        } else if(hitting){
            highlightInteractable(hit.transform);
        }
    }

    private void highlightInteractable(Transform element)
    {
        boardUi.MouseOver(element, isDragging());
    }

    private bool isDragging(){
        return draggedObject != null;
    }
    private Vector3 GetDragPosition()
    {
        float enterDist;
        Ray rayCast = camera.ScreenPointToRay(Input.mousePosition);
        dragPlane.Raycast(rayCast, out enterDist);
        return rayCast.GetPoint(enterDist) + dragOffSet;
    }

    private void startDragging(Transform element)
    {
        Draggable draggable = element.GetComponent<Draggable>();
        if (draggable != null && draggable.isDraggable())
        {
            draggedObject = element;
            startPosition = draggedObject.position;
            draggedObject.transform.localScale = dragScale;
        }
    }

    private void stopDragging(Transform element)
    {
     
            Hexagon to = element.GetComponent<Hexagon>();
            if (to != null)
            {
                Hexagon draggingHexagon = draggedObject.GetComponent<Hexagon>();
                if (draggingHexagon != null && !boardUi.placeHexagon(draggingHexagon, to))
                {
                    draggedObject.position = startPosition;
                }

                Stone draggingStone = draggedObject.GetComponent<Stone>();
                if (draggingStone != null && !boardUi.placeStone(draggingStone, to))                    
                {
                    draggedObject.position = startPosition;
                }
            }
    }
}
