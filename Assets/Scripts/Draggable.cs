using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 startPosition;
    private bool isDragging = false;

    void OnMouseDown()
    {
        //Bloðun ilk konumu
        startPosition = transform.position;

        // Fare ile objenin merkezi arasýndaki fark (Zýplama yapmamasý için)
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
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
        //Spacing deðeriyle ayný olmalý (1.1f)
        float snapValue = 1.1f;

        // Mevcut pozisyonu en yakýn 1.1'in katýna yuvarla
        // Formül: Round(Konum / Aralýk) * Aralýk
        float x = Mathf.Round(transform.position.x / snapValue) * snapValue;
        float y = Mathf.Round(transform.position.y / snapValue) * snapValue;

        // Izgara sýnýrlarýný kontrol et (0 ile 7*1.1 arasý)
        // Eðer blok ýzgaranýn çok dýþýndaysa eski yerine dönsün
        if (x >= -0.5f && x < 8 * snapValue && y >= -0.5f && y < 8 * snapValue)
        {
            transform.position = new Vector3(x, y, 0);
            Debug.Log("Izgaraya baþarýyla oturdu: " + x + ", " + y);
        }
        else
        {
            transform.position = startPosition;
            Debug.Log("Izgara dýþý! Eski konuma dönüldü.");
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