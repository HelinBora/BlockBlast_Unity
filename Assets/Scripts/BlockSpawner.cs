using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    public List<GameObject> blockPrefabs; // Eldeki L, T, I, Kare prefablarýný buraya sürükleyeceðiz
    public Transform[] spawnSlots;        // Oluþturduðum 3 boþ slotu buraya atayacaðým

    void Start()
    {
        SpawnNewNewRound();
    }

    public void SpawnNewNewRound()
    {
        // 3 slotun her biri için rastgele bir blok üretelim
        foreach (Transform slot in spawnSlots)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Count);
            // Bloðu slotun tam üzerinde oluþturalým
            GameObject newBlock = Instantiate(blockPrefabs[randomIndex], slot.position, Quaternion.identity);

            // Bloðu biraz küçültelim
            newBlock.transform.localScale = Vector3.one * 0.6f;
        }
    }
}