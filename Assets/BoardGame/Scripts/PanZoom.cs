﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}