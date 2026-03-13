using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    public List<GameObject> blockPrefabs;
    public Transform[] spawnSlots;
    private List<GameObject> activeBlocks = new List<GameObject>(); // Sahnedeki bloklarý tutar

    void Start() { SpawnNewRound(); }

    public void SpawnNewRound()
    {
        activeBlocks.Clear();
        foreach (Transform slot in spawnSlots)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Count);
            GameObject newBlock = Instantiate(blockPrefabs[randomIndex], slot.position, Quaternion.identity);
            newBlock.transform.localScale = Vector3.one * 0.8f;

            activeBlocks.Add(newBlock); // Bloðu listeye ekle
        }
    }

    // Bir blok yerleþtiðinde listeden çýkar ve liste boþsa yeni turu baþlat
    public void BlockPlaced(GameObject block)
    {
        activeBlocks.Remove(block);
        if (activeBlocks.Count == 0)
        {
            SpawnNewRound();
        }
    }
}