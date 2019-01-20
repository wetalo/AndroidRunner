using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePrefabs;

    private Transform playerTransform;
    private float spawnZ = -5.0f;
    private float tileLength = 10.0f;
    private float safeZone = 20.0f;
    private int amountTilesOnScreen = 10;

    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

    [Space]
    [Header("Item Variables")]
    [SerializeField]
    int numTilesBeforeSpawnItem = 2;
    int numTilesSpawnedSinceLastItem = 0;

    public GameObject[] itemPrefabs;

	// Use this for initialization
	void Start () {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for(int i=0; i<amountTilesOnScreen; i++)
        {
            if (i < 4)
            {
                SpawnTile(0);
            } else
            {
                SpawnTile();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(playerTransform.position.z - safeZone > (spawnZ - amountTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
	}

    private void LateUpdate()
    {
        //PrintFrameRate();
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;

        if(prefabIndex == -1)
        {
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        } else
        {
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        }
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);

        numTilesSpawnedSinceLastItem++;
        if(numTilesSpawnedSinceLastItem >= numTilesBeforeSpawnItem)
        {
            SpawnItem(go);
            numTilesSpawnedSinceLastItem = 0;
        }
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if(tilePrefabs.Length <= 1)
        {
            return 0;
        }

        int randomIndex = lastPrefabIndex;
        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(1, tilePrefabs.Length);
        }

        return randomIndex;
    }

    void PrintFrameRate()
    {

        GameObject.Find("FramerateText").GetComponent<Text>().text = "framerate:\n" + 1.0f / Time.deltaTime;
    }

    void SpawnItem(GameObject tile)
    {
        Transform tileChild = tile.transform.GetChild(0);
        Transform objectSpawnPointHolder = null;

        for(int i=0; i<tileChild.childCount; i++)
        {
            Transform child = tileChild.GetChild(i);
            if(child.gameObject.tag == "ObjectSpawnPointHolder" || child.gameObject.name == "ObjectSpawnPoints")
            {
                objectSpawnPointHolder = child;
                break;
            }
        }

        if(objectSpawnPointHolder != null)
        {
            Transform itemSpawnPoint = objectSpawnPointHolder.GetChild(Random.Range(0, objectSpawnPointHolder.childCount));
            GameObject itemInstance = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);

            itemInstance.transform.parent = itemSpawnPoint;
            itemInstance.transform.localPosition = Vector3.zero;
            itemInstance.transform.rotation = Quaternion.identity;
        }
    }
}
