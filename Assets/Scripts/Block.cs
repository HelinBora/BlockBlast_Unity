using UnityEngine; 
using System.Collections.Generic; // List<> yapýsý için gerekli

public class Block : MonoBehaviour
{
    // Bloðu oluþturan karelerin ýzgara üzerindeki göreceli yerleri
    public List<Vector2Int> relativeIndices = new List<Vector2Int>();

    void Awake()
    {
        List<Vector2Int> tempIndices = new List<Vector2Int>();
        int minX = int.MaxValue;
        int minY = int.MaxValue;

        // 1. Önce tüm parçalarýn ham koordinatlarýný bul ve en küçükleri tespit et
        foreach (Transform tile in transform)
        {
            int x = Mathf.RoundToInt(tile.localPosition.x / 1.1f);
            int y = Mathf.RoundToInt(tile.localPosition.y / 1.1f);
            tempIndices.Add(new Vector2Int(x, y));

            // En sol ve en alt noktayý buluyoruz
            if (x < minX) minX = x;
            if (y < minY) minY = y;
        }

        // 2. Tüm koordinatlarý minX ve minY deðerinden çýkararak (0,0) noktasýna çek
        // Böylece bloðun þekli ne olursa olsun her zaman 0'dan baþlar
        foreach (Vector2Int index in tempIndices)
        {
            relativeIndices.Add(new Vector2Int(index.x - minX, index.y - minY));
        }
    }
}