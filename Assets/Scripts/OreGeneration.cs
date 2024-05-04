using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OreGeneration : MonoBehaviour
{
    public Tilemap tileMap;
    private TileBase[] oreSpawns;

    public int spawnChance = 300;
    public int stoneChance = 856;
    public int mutantChance = 12;
    public int copperChance = 50;
    public int rubyChance = 35;
    public int emeraldChance = 20;
    public int goldChance = 15;
    public int diamondChance = 10;
    public int platinumChance = 2;

    public int theForgottenChance = 40;
    public int slimeChance = 450;
    public int snailChance = 260;
    public int weepingSpiritChance = 250;

    private GameObject[] prefabs = new GameObject[7];
    
    private List<GameObject> spawned = new List<GameObject>();

    void Start() {
        BoundsInt bounds = tileMap.cellBounds;
        oreSpawns = tileMap.GetTilesBlock(bounds);
        if (stoneChance + mutantChance + copperChance + rubyChance + emeraldChance + goldChance + diamondChance +
            platinumChance != 1000) {
            Debug.LogError("Sum of ore chances is not 1000");
            Destroy(this);
        }

        prefabs[0] = Resources.Load<GameObject>("Rocks/Stone Ore");
        prefabs[1] = Resources.Load<GameObject>("Rocks/Copper Ore");
        prefabs[2] = Resources.Load<GameObject>("Rocks/Ruby Ore");
        prefabs[3] = Resources.Load<GameObject>("Rocks/Emerald Ore");
        prefabs[4] = Resources.Load<GameObject>("Rocks/Gold Ore");
        prefabs[5] = Resources.Load<GameObject>("Rocks/Diamond Ore");
        prefabs[6] = Resources.Load<GameObject>("Rocks/Platinum Ore");

        Generate();
    }
    
    public void Generate() {
        foreach (Vector3Int pos in tileMap.cellBounds.allPositionsWithin)
        {
            if (tileMap.GetTile(pos) != null) {
                int rand = Random.Range(0, 1000);
                if (rand <= spawnChance) {
                    // Offset pos so that it is in the center of the tile
                    Vector3 spawnPos = new Vector3(pos.x+ 0.5f, pos.y + 0.5f, pos.z);
                    int oreRand = Random.Range(0, 1000);
                    if (oreRand <= stoneChance) {
                        spawned.Add(Instantiate(prefabs[0], spawnPos, Quaternion.identity));
                    }
                    else if (oreRand <= stoneChance + mutantChance) {
                        SpawnEnemy(spawnPos);
                    }
                    else if (oreRand <= stoneChance + mutantChance + copperChance) {
                        spawned.Add(Instantiate(prefabs[1], spawnPos, Quaternion.identity));                    
                    }
                    else if (oreRand <= stoneChance + mutantChance + copperChance + rubyChance) {
                        spawned.Add(Instantiate(prefabs[2], spawnPos, Quaternion.identity));
                    }
                    else if (oreRand <= stoneChance + mutantChance + copperChance + rubyChance + emeraldChance) {
                        spawned.Add(Instantiate(prefabs[3], spawnPos, Quaternion.identity));
                    }
                    else if (oreRand <= stoneChance + mutantChance + copperChance + rubyChance + emeraldChance + goldChance) {
                        spawned.Add(Instantiate(prefabs[4], spawnPos, Quaternion.identity));
                    }
                    else if (oreRand <= stoneChance + mutantChance + copperChance + rubyChance + emeraldChance + goldChance + diamondChance) {
                        spawned.Add(Instantiate(prefabs[5], spawnPos, Quaternion.identity));
                    }
                    else if (oreRand <= stoneChance + mutantChance + copperChance + rubyChance + emeraldChance + goldChance + diamondChance + platinumChance) {
                        spawned.Add(Instantiate(prefabs[6], spawnPos, Quaternion.identity));
                    }
                }
            }
        }
    }

    private void SpawnEnemy(Vector3 pos) {
        int rand = Random.Range(0, theForgottenChance + slimeChance + snailChance + weepingSpiritChance);
        Debug.Log(rand);
        if (rand <= theForgottenChance) {
            spawned.Add(Instantiate(Resources.Load<GameObject>("Enemies/The Forgotten"), pos, Quaternion.identity));
        }
        else if (rand <= theForgottenChance + slimeChance) {
            spawned.Add(Instantiate(Resources.Load<GameObject>("Enemies/Slime"), pos, Quaternion.identity));
        }
        else if (rand <= theForgottenChance + slimeChance + snailChance) {
            spawned.Add(Instantiate(Resources.Load<GameObject>("Enemies/Snail"), pos, Quaternion.identity));
        }
        else if (rand <= theForgottenChance + slimeChance + snailChance + weepingSpiritChance) {
            spawned.Add(Instantiate(Resources.Load<GameObject>("Enemies/Weeping Spirit"), pos, Quaternion.identity));
            Debug.Log("Spawned spirit");
        }
    }

    public void ClearExisting() {
        foreach (GameObject obj in spawned) {
            Destroy(obj);
        }
        spawned.Clear();
    }
}
