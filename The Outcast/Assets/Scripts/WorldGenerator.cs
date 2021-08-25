using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private const int PLATFORM_RANGE = 50;
    private const int MAX_BUFFER_SIZE = 10;
    private const int RANDOM_LENGTH = 400;

    [SerializeField] private GameObject[] platformsArray;
    [SerializeField] private GameObject[] baobabTreePrefab;
    [SerializeField] private Transform platformContainer;
    [SerializeField] private GameObject player;

    private GameObject[] spawnedPlatforms = new GameObject[MAX_BUFFER_SIZE];
    private Transform lastPlatfrom = null;
    private int indexLastPlatform = 0;
    private int indexDeletePlatform = 0;
    private GameObject baobabTree;
    void Start()
    {        
        CreatePlatform();
        CreatePlatform();
        CreatePlatform();
        CreatePlatform();
        CreatePlatform();
        SpawnBaobabTree();
    }

    void Update()
    {
        float playerDistanceScore = player.transform.position.z;
        if (playerDistanceScore > spawnedPlatforms[indexLastPlatform-1].transform.position.z - PLATFORM_RANGE * 6)
        {
            CreatePlatform();
            DeletePlatform();
        }
        if (baobabTree != null)
        {
            if (playerDistanceScore > baobabTree.transform.position.z + 20) SpawnBaobabTree();
        }
    }

    void CreatePlatform()
    {
        Vector3 pos = (lastPlatfrom == null) ?
            platformContainer.position :
            lastPlatfrom.Find("endPoint").position;

        pos.z += PLATFORM_RANGE;

        int indexRand = UnityEngine.Random.Range(0, platformsArray.Length);
        GameObject res = Instantiate(platformsArray[indexRand], pos, Quaternion.identity, platformContainer);
        lastPlatfrom = res.transform;
        spawnedPlatforms[indexLastPlatform] = res;
        indexLastPlatform++;
        if (indexLastPlatform == MAX_BUFFER_SIZE)
        {
            indexLastPlatform = 0;
            CreatePlatform();
        }
    }
    void DeletePlatform()
    {
        Destroy(spawnedPlatforms[indexDeletePlatform]);
        indexDeletePlatform++;
        if (indexDeletePlatform == MAX_BUFFER_SIZE) indexDeletePlatform = 0;
    }

    public void SpawnBaobabTree()
    {
        if (baobabTree != null) Destroy(baobabTree);
        int lenRand = UnityEngine.Random.Range(RANDOM_LENGTH, RANDOM_LENGTH * 4);
        int xPosRand = UnityEngine.Random.Range(-30, 30);
        Vector3 pos = new Vector3(xPosRand, 0.5f, player.transform.position.z + lenRand);
        baobabTree = Instantiate(baobabTreePrefab[UnityEngine.Random.Range(0, 2)], pos, Quaternion.Euler(0,90,0), platformContainer);
    }
}
