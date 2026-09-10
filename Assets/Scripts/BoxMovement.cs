
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxMovement : MonoBehaviour
{

    private float startMouseY;
    private float startObjectY;
    void OnMouseDown()
    {
        startObjectY = transform.position.y;
        startMouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

    }
    void OnMouseDrag()
    {
        float currentMouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        float deltaY = currentMouseY - startMouseY;
        Vector3 objPosition = transform.position;
        objPosition.y = startObjectY + deltaY;
        transform.position = objPosition;
    }
    

}
