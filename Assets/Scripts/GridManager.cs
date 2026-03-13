using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefabs; 
    public int width = 8;
    public int height = 8;
    public float spacing = 1.1f; 

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Pozisyonu hesapla
                Vector3 position = new Vector3(x * spacing, y * spacing, 0);
                // Tile'ý oluþtur
                Instantiate(tilePrefabs, position, Quaternion.identity);
            }
        }

        Camera.main.transform.position = new Vector3((width * spacing) / 2f - 0.5f, (height * spacing) / 2f - 0.5f, -10f);
    }

    // Izgaradaki doluluk durumunu tutan matrisimiz
    public GameObject[,] gridMatrix = new GameObject[8, 8];

    // Bir bloðun yerleþip yerleþemeyeceðini kontrol eden fonksiyon
    public bool CanPlaceBlock(List<Transform> blockTiles)
    {
        foreach (Transform tile in blockTiles)
        {
            // Bloðun her bir karesinin koordinatlarýný 1.1'e bölerek matris indeksini buluyoruz
            int x = Mathf.RoundToInt(tile.position.x / 1.1f);
            int y = Mathf.RoundToInt(tile.position.y / 1.1f);

            // Sýnýr kontrolü ve doluluk kontrolü
            if (x < 0 || x >= 8 || y < 0 || y >= 8 || gridMatrix[x, y] != null)
            {
                return false; // Eðer dýþarýdaysa veya yer doluysa koyamazsýn
            }
        }
        return true;
    }

    // Bloðu matrise kaydet
    public void PlaceBlockOnGrid(List<Transform> blockTiles)
    {
        foreach (Transform tile in blockTiles)
        {
            int x = Mathf.RoundToInt(tile.position.x / 1.1f);
            int y = Mathf.RoundToInt(tile.position.y / 1.1f);
            gridMatrix[x, y] = tile.gameObject;
        }
        CheckForFullLines(); // Blok konunca satýr doldu mu diye bak
    }

    private void CheckForFullLines()
    {
        List<int> fullRows = new List<int>();
        List<int> fullCols = new List<int>();

        // 1. Satýrlarý Kontrol Et
        for (int y = 0; y < 8; y++)
        {
            bool isFull = true;
            for (int x = 0; x < 8; x++)
            {
                if (gridMatrix[x, y] == null) isFull = false;
            }
            if (isFull) fullRows.Add(y);
        }

        // 2. Sütunlarý Kontrol Et
        for (int x = 0; x < 8; x++)
        {
            bool isFull = true;
            for (int y = 0; y < 8; y++)
            {
                if (gridMatrix[x, y] == null) isFull = false;
            }
            if (isFull) fullCols.Add(x);
        }

        // 3. Patlatma Ýþlemi (Ayný anda hem satýr hem sütun patlayabilir)
        foreach (int row in fullRows) ClearRow(row);
        foreach (int col in fullCols) ClearColumn(col);
    }

    private void ClearColumn(int col)
    {
        for (int y = 0; y < 8; y++)
        {
            if (gridMatrix[col, y] != null)
            {
                Destroy(gridMatrix[col, y]);
                gridMatrix[col, y] = null;
            }
        }
        Debug.Log("Aþko, SÜTUN patladý! Müthiþsin!");
    }

    private void ClearRow(int row)
    {
        for (int x = 0; x < 8; x++)
        {
            Destroy(gridMatrix[x, row]); // Görseli yok et
            gridMatrix[x, row] = null;   // Matrisi boþalt
        }
        Debug.Log("Aþko, SATIR patladý! +100 Puan");
    }

}
