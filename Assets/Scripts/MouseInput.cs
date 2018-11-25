using System;
using UnityEngine;

public class MouseInput: MonoBehaviour {

    public new Camera camera;
    private Transform draggedObject;
    private Vector3 startPosition;
    private Plane dragPlane = new Plane(Vector3.up, new Vector3(0, 3, 0));
    private Vector3 dragScale = new Vector3(.5f, .5f, .5f);
    private Vector3 dragOffSet = new Vector3(0, 0, 1);
    private Board board;
    private BoardFactory boardFactory;

    private void Start()
    {
        board = FindObjectOfType<Game>().board;   
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            startDragging();
        
        if(draggedObject != null)
            draggedObject.position = GetDragPosition();

        if (Input.GetMouseButtonUp(0) && draggedObject != null)
            stopDragging();
    }

    private Vector3 GetDragPosition()
    {
        float enterDist;
        Ray rayCast = camera.ScreenPointToRay(Input.mousePosition);
        dragPlane.Raycast(rayCast, out enterDist);
        return rayCast.GetPoint(enterDist) + dragOffSet;
    }

    private void startDragging()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Draggable draggable = hit.transform.parent.GetComponent<Draggable>();
            if (draggable != null && draggable.isDraggable())
            {
                draggedObject = hit.transform.parent;
                startPosition = draggedObject.position;
                draggedObject.transform.localScale = dragScale;
            }
        }
    }

    private void stopDragging()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Hexagon to = hit.transform.parent.GetComponent<Hexagon>();
            if (to != null)
            {
                Hexagon draggingHexagon = draggedObject.GetComponent<Hexagon>();
                if (draggingHexagon != null)
                {
                    if (!placeHexagon(draggingHexagon, to))
                    {
                        draggedObject.position = startPosition;
                    }
                }

                Stone draggingStone = draggedObject.GetComponent<Stone>();
                if (draggingStone != null)
                {
                    if (!placeStone(draggingStone, to))
                    {
                        draggedObject.position = startPosition;
                    }
                }
            }
        }
        draggedObject.transform.localScale = new Vector3(1, 1, 1);
        draggedObject = null;
    }

    private bool placeStone(Stone stone, Hexagon to)
    {
        Tile fromTile = board.coordToTile(stone.coord);
        Tile toTile = board.coordToTile(to.coord);
        if (board.moveStone(fromTile, toTile)){
            Vector3 toPosition = getBoardFactory().GetPositionForStone(to.coord, to.getTile().level);
            stone.transform.position = toPosition;
            stone.coord = to.coord;
            board.currentStep = Step.BUILD;
            board.nextPlayer();
            return true;
        }
        return false;
    }

    private bool placeHexagon(Hexagon from, Hexagon to)
    {
        Tile fromTile = board.coordToTile(from.coord);
        Tile toTile = board.coordToTile(to.coord);
        if (board.build(fromTile, toTile))
        {
            if (board.canMoveStoneAnyWhere())
            {
                Vector3 toPosition = getBoardFactory().GetPositionForHexagon(to.coord, to.getTile().level);
                from.transform.position = toPosition;
                from.coord = to.coord;
                board.currentStep = Step.MOVE;
                return true;
            }
            else
            {
                board.build(toTile, fromTile);
                draggedObject.position = startPosition;
            }
        }
        return false;
    }

    private BoardFactory getBoardFactory()
    {
        if (boardFactory == null)
        {
            boardFactory = FindObjectOfType<BoardFactory>();
        }
        return boardFactory;
    }
}
