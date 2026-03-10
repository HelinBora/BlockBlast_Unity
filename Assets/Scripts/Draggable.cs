using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    void OnMouseDown()
    {
        Debug.Log("Objeye týklandý!");

        // Fare ile objenin merkezi arasýndaki fark (Zýplama yapmamasý için)
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        // Fare hareket ettikçe objeyi yeni konuma taþýma
        transform.position = GetMouseWorldPos() + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Ekran koordinatlarýný (piksel) dünya koordinatlarýna çevirme
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}