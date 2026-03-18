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
    // Bir bloðun yerleþip yerleþemeyeceðini kontrol eden fonksiyon (GÜNCELLENDÝ)
    public bool CanPlaceBlock(List<Transform> blockTiles)
    {
        foreach (Transform tile in blockTiles)
        {
            // ARTIK BURADA DA GÜVENLÝ HELPER'I KULLANIYORUZ
            Vector2Int index = GetGridIndex(tile.position);
            int x = index.x;
            int y = index.y;

            // Sýnýr kontrolü ve doluluk kontrolü
            if (x < 0 || x >= 8 || y < 0 || y >= 8 || gridMatrix[x, y] != null)
            {
                return false;
            }
        }
        return true;
    }
    // Bloðu matrise kaydet
    public void PlaceBlockOnGrid(List<Transform> blockTiles)
    {
        foreach (Transform tile in blockTiles)
        {
            Vector2Int index = GetGridIndex(tile.position); // Yeni helper'ý kullanacaðýz
            gridMatrix[index.x, index.y] = tile.gameObject;
        }
        CheckForFullLines();

        // Spawner'a haber ver ki satýrlar temizlendikten sonra kontrol etsin
        FindFirstObjectByType<BlockSpawner>().CheckGameOver();
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

    public bool CanAnyBlockFit(List<GameObject> activeBlocks)
    {
        foreach (GameObject blockObj in activeBlocks)
        {
            if (blockObj == null) continue; // Ölü objeleri geç

            Block blockData = blockObj.GetComponent<Block>();

            // --- GÜVENLÝK DUVARI ---
            if (blockData == null)
            {
                Debug.LogError(blockObj.name + " üzerinde 'Block' scripti eksik aþko! Hemen takmalýsýn.");
                continue; // Eðer script yoksa bu bloðu tarama, hata verme
            }
            // -----------------------

            for (int gridX = 0; gridX < 8; gridX++)
            {
                for (int gridY = 0; gridY < 8; gridY++)
                {
                    if (CanFitAt(blockData.relativeIndices, gridX, gridY))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }    // Dünyadaki koordinatý ýzgara indeksine (0-7) çeviren yöntem
    public Vector2Int GetGridIndex(Vector3 worldPos)
    {
        // Izgaranýn kendi pozisyonunu hesaptan çýkarýyoruz (Offset düzeltme)
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.RoundToInt(localPos.x / 1.1f);
        int y = Mathf.RoundToInt(localPos.y / 1.1f);

        return new Vector2Int(x, y);
    }

    private bool CanFitAt(List<Vector2Int> shapePattern, int startX, int startY)
    {
        foreach (Vector2Int offset in shapePattern)
        {
            int checkX = startX + offset.x;
            int checkY = startY + offset.y;

            // Izgara sýnýrlarý dýþý veya hücre zaten doluysa direkt false dön
            if (checkX < 0 || checkX >= 8 || checkY < 0 || checkY >= 8 || gridMatrix[checkX, checkY] != null)
            {
                return false;
            }
        }
        return true;
    }
    void OnDrawGizmos()
    {
        if (gridMatrix == null) return;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                // Eðer hücre doluysa kýrmýzý, boþsa yeþil bir küre çiz
                Gizmos.color = gridMatrix[x, y] != null ? Color.red : Color.green;
                Vector3 pos = new Vector3(x * 1.1f, y * 1.1f, 0) + transform.position;
                Gizmos.DrawSphere(pos, 0.2f);
            }
        }
    }
}
