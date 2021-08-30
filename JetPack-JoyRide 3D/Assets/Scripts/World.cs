using System;
using System.Collections;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private GameObject VehicleIcon;
    [SerializeField] private GameObject[] freePlatforms;
    [SerializeField] private GameObject[] freeCoins;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject npcObj;
    [SerializeField] private Transform platformContainer;

    [SerializeField] private PLControl2 playerController;
    [SerializeField] private BuyMenu menu;
    [SerializeField] private GameObject player;
    
    private GameObject[] spawnedPlatforms = new GameObject[10];
    private Transform lastPlatfrom = null;
    private int indexPlatform = 0;
    private int indexDelPlatform = 0;

    private GameObject BRocket;
    private GameObject BLaser;
    private GameObject SpawnedCoinSet;
    private GameObject SpawnedNpc;
    private bool isLaser = false;
    private bool isEnum = false;
    private int xPosLaser = -1000;
    private int xPosRocket = -400;
    private int xPosCoin = -70;
    private int xPosNpc = -15;
    private int xPosVehicle = -555;
    private int yPosLaser = 10;
    private int[] zPosNpc = new int[2] { -5, 6 };

    void Start()
    {
        CreateFreePlatform();
        CreateFreePlatform();
        CreateFreePlatform();
        CreateFreePlatform();
        CreateFreePlatform();
        CreateFreePlatform();
    }


    void Update()
    {
        //array resizing
        if (indexPlatform >= spawnedPlatforms.Length - 2)
        {
            Array.Resize(ref spawnedPlatforms, spawnedPlatforms.Length*2);
        }

        //Coins generation
        if (player.transform.position.x <= xPosCoin)
        {
            SpawnedCoinSet = Instantiate(freeCoins[UnityEngine.Random.Range(0, freeCoins.Length)], new Vector3(player.transform.position.x - 100, 8/*8units for Y-axis*/, player.transform.position.z), Quaternion.identity, platformContainer); ;
            xPosCoin -= UnityEngine.Random.Range(80, 200);
        }

        //NPC generation
        if (player.transform.position.x <= xPosNpc)
        {
            SpawnedNpc = Instantiate(npcObj, new Vector3(player.transform.position.x - 180, 0.3f/*0.3 units for Y-axis*/, zPosNpc[UnityEngine.Random.Range(0, 2)]), Quaternion.identity, platformContainer);
            xPosNpc -= 125;
            StartCoroutine(DeleteNpc());
        }

        //Vehicle generation
        if (playerController.CanVehicle)
        {
            if (player.transform.position.x <= xPosVehicle)
            {
                SpawnedCoinSet = Instantiate(VehicleIcon, new Vector3(player.transform.position.x - 100, 8/*8units for Y-axis*/, player.transform.position.z), Quaternion.identity, platformContainer);
                xPosVehicle -= UnityEngine.Random.Range(500, 1000);
            }
        }
        
        //Dangers generation
        try
        {
            //Laser
            if (player.transform.position.x <= xPosLaser)
            {
                BLaser = Instantiate(laser, new Vector3(player.transform.position.x - 19, yPosLaser, player.transform.position.z), Quaternion.identity, platformContainer);
                isLaser = true;
                xPosLaser -= 1000;
            }
            if (isLaser)
            {
                BLaser.transform.position = new Vector3(player.transform.position.x - 19, yPosLaser, player.transform.position.z);
                if (!isEnum)
                {
                    StartCoroutine(FollowLaser());
                }
            }
            //rocket
            if (player.transform.position.x <= xPosRocket)
            {
                 BRocket = Instantiate(rocket, new Vector3(player.transform.position.x - 100, player.transform.position.y+1, player.transform.position.z), Quaternion.identity, platformContainer);
                 xPosRocket -= 473;
                 StartCoroutine(DeleteRocket());
            }

        }
        catch(Exception)
        {

        }


        //auto-add/delete
        if (player.transform.position.x < spawnedPlatforms[indexPlatform-4].transform.position.x)
        {
            CreateFreePlatform();
            if(indexPlatform > 6)
            {
                DelPlatform();
            }
        }
    }

    public void CreateFreePlatform()
    {
        Vector3 pos = (lastPlatfrom == null) ?
            platformContainer.position :
            lastPlatfrom.GetComponent<PlatformController>().endPoint.position;

        int index = UnityEngine.Random.Range(0, freePlatforms.Length);
        GameObject res = Instantiate(freePlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatfrom = res.transform;
        spawnedPlatforms[indexPlatform] = res;
        indexPlatform++;
    }
    public void DelPlatform()
    {
        Destroy(spawnedPlatforms[indexDelPlatform]);
        indexDelPlatform++;
    }
   

    public IEnumerator FollowLaser()
    {
            isLaser = true;
            yield return new WaitForSeconds(8f);
            Destroy(BLaser);
            isLaser = false;
            isEnum = false;
            yPosLaser = UnityEngine.Random.Range(2, 14);
    }

    public IEnumerator DeleteRocket()
    {
        yield return new WaitForSeconds(10f);
        Destroy(BRocket);
    }
    public IEnumerator DeleteNpc()
    {
        yield return new WaitForSeconds(9f);
        Destroy(SpawnedNpc);
    }

}
