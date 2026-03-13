using UnityEngine;
using System.Collections.Generic;

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
        // Bloðun içindeki tüm küçük kareleri (child) bir listeye alalým
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform) { children.Add(child); }

        GridManager gridManager = FindFirstObjectByType<GridManager>();

        // Eðer yer müsaitse ve ýzgara sýnýrlarýndaysa yerleþtir
        if (gridManager.CanPlaceBlock(children))
        {
            float x = Mathf.Round(transform.position.x / snapValue) * snapValue;
            float y = Mathf.Round(transform.position.y / snapValue) * snapValue;
            transform.position = new Vector3(x, y, 0);

            gridManager.PlaceBlockOnGrid(children); // Matrise kaydet
            this.enabled = false;
            spawner.BlockPlaced(this.gameObject);
        }
        else
        {
            // Yer doluysa veya dýþarýdaysa týpýþ týpýþ eski yerine dön
            transform.position = startPosition;
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