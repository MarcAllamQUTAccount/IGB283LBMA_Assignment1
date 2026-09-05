
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxMovement : MonoBehaviour
{
    private bool dragging = false;
    public Transform[] objectsToMove;
    private float startMouseY;
    private float[] startObjectY;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            startObjectY = new float[objectsToMove.Length];
            for (int i = 0; i < objectsToMove.Length; i++)
            {
                startObjectY[i] = objectsToMove[i].position.y;
            }
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        if (dragging)
        {
            float currentMouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            float deltaY = currentMouseY - startMouseY;

            for (int i = 0; i < objectsToMove.Length; i++)
            {
                Vector3 p = objectsToMove[i].position;
                p.y = startObjectY[i] + deltaY;
                objectsToMove[i].position = p;
            }
        }
    }

}
