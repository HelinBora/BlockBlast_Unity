using UnityEngine;

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
}
