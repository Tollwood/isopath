using System;
using UnityEngine;

public class MouseInput: MonoBehaviour {

    public new Camera camera;
    private Transform draggedObject;
    private Vector3 startPosition;
    private Plane dragPlane;
    public Vector3 dragScale;
    private Vector3 dragOffSet = new Vector3(0, 0, 0);
    private BoardFactory boardFactory;
    private Game game;
    private void Start()
    {
        boardFactory = FindObjectOfType<BoardFactory>();
        game = FindObjectOfType<Game>();

        dragPlane = new Plane(Vector3.up, new Vector3(0, 3 * boardFactory.scale, 0));
        dragScale = new Vector3(.5f * boardFactory.scale, .5f * boardFactory.scale, .5f * boardFactory.scale);
    }

    void Update()
    {
        if(game.gameState != GameState.PLAYING){
            return;
        }
        if(Input.touchSupported){
            handleTouch();
        }
        else{
            handleMouseInput();
        }

    }

    private void handleTouch()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        bool hitting = Physics.Raycast(ray, out hit);


        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && hitting && !isDragging())
        {
            Stone stone = hit.transform.GetComponent<Stone>();
            if (stone != null && Rules.CanCapture(game.board.tiles, game.board.size, stone.getTile()))
            {
                boardFactory.CaptureStone(stone.getTile());
            }
            startDragging(hit.transform);
        }
        else if(Input.touchCount > 1 && isDragging()){
            draggedObject.position = startPosition;
        }
        else if(Input.touchCount == 1 && isDragging()){
            draggedObject.position = GetDragPosition();
        }
        else if (Input.touchCount == 0 && isDragging())
        {
            if (hitting) {
                stopDragging(hit.transform);
            }
            else
            {
                draggedObject.position = startPosition;
            }

            draggedObject.transform.localScale = new Vector3(boardFactory.scale, boardFactory.scale, boardFactory.scale);
            draggedObject.GetComponent<Collider>().enabled = true;
            draggedObject = null;

        }
    }


    private void handleMouseInput()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        bool hitting = Physics.Raycast(ray, out hit);

        if (Input.GetMouseButtonDown(0) && hitting)
        {
            Stone stone = hit.transform.GetComponent<Stone>();
            if (stone != null && Rules.CanCapture(game.board.tiles, game.board.size, stone.getTile()))
            {
                boardFactory.CaptureStone(stone.getTile());
            }
            startDragging(hit.transform);

        }
        else if (Input.GetMouseButtonUp(0) && isDragging())
        {
            Cursor.visible = true;
            if (hitting)
            {
                stopDragging(hit.transform);
            }
            else {
                draggedObject.position = startPosition;    
            }
            draggedObject.transform.localScale = new Vector3(boardFactory.scale, boardFactory.scale, boardFactory.scale);
            draggedObject.GetComponent<Collider>().enabled = true;
            draggedObject = null;
        }
        else if (isDragging())
        {
            draggedObject.position = GetDragPosition();
        }
        else if (hitting)
        {
            highlightInteractable(hit.transform);
        }
    }

    private void highlightInteractable(Transform element)
    {
        boardFactory.MouseOver(element, isDragging());
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
            Cursor.visible = false;
            draggedObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void stopDragging(Transform element)
    {

            Hexagon to = element.GetComponent<Hexagon>();
            if (to != null)
            {
                Hexagon draggingHexagon = draggedObject.GetComponent<Hexagon>();
            if (draggingHexagon != null && !boardFactory.placeHexagon(draggingHexagon, to.tile))
                {
                    draggedObject.position = startPosition;
                }

                Stone draggingStone = draggedObject.GetComponent<Stone>();
                if (draggingStone != null && !boardFactory.placeStone(draggingStone, to))                    
                {
                    draggedObject.position = startPosition;
                }
            }
    }
}
