using UnityEngine;


public class Draggable : MonoBehaviour
{
    private BlockSpawner spawner;
    private Vector3 offset;
    private Vector3 startPosition;
    private bool isDragging = false;

    void Start()
    {
        spawner = FindFirstObjectByType<BlockSpawner>(); // Spawner'ý otomatik bulur
    }

    void OnMouseDown()
    {
        //Bloðun ilk konumu
        startPosition = transform.position;

        // Fare ile objenin merkezi arasýndaki fark (Zýplama yapmamasý için)
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;

         // Bloðu tutunca orijinal boyutuna (1.0) getir
         transform.localScale = Vector3.one;
        
    }

    void OnMouseDrag()
    {
        // Fare hareket ettikçe objeyi yeni konuma taþýma
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToGrid();
    }

    private void SnapToGrid()
    {
        float snapValue = 1.1f;
        float x = Mathf.Round(transform.position.x / snapValue) * snapValue;
        float y = Mathf.Round(transform.position.y / snapValue) * snapValue;

        // Izgara sýnýrlarý kontrolü 
        if (x >= 0 && x < 8 * snapValue && y >= 0 && y < 8 * snapValue)
        {
            transform.position = new Vector3(x, y, 0);
            this.enabled = false; // Yerleþen blok bir daha sürüklenebilir olmasýn
            spawner.BlockPlaced(this.gameObject); // Spawner'a haber ver
        }
        else
        {
            transform.position = startPosition; // Izgara dýþýysa eski yerine dön
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Ekran koordinatlarýný (piksel) dünya koordinatlarýna çevirme
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}