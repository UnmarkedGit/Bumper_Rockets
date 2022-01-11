using UnityEngine;
using Photon.Pun;

public class ResourceSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject SatellitePrefab;
    [SerializeField]
    private GameObject Resource0Prefab;
    [SerializeField]
    private GameObject Resource1Prefab;
    [SerializeField]
    private GameObject Resource2Prefab;

    [SerializeField]
    private float Sc0SpawnTotal = 60;
    [SerializeField]
    private float SatSpawnTotal = 15;

    //[SerializeField]
    //private Vector3 CenterSpawn;
    [SerializeField]
    private Vector3 TopMapSides;
    [SerializeField]
    private float TopPos;//big is 410.0f
    [SerializeField]
    private float TopPosMaxL;//big is -275.0f
    [SerializeField]
    private float TopPosMaxR;//big is 275.0f
    [SerializeField]
    private Vector3 BottomMapSides;
    [SerializeField]
    private float BotPos;//big is -410.0f
    [SerializeField]
    private float BotPosMaxL;//big is -275.0f
    [SerializeField]
    private float BotPosMaxR;//big is 275.0f
    [SerializeField]
    private Vector3 LeftMapSides;
    [SerializeField]
    private float LeftPos;//big is -410.0f
    [SerializeField]
    private float LeftPosMaxT;//big is 275.0f
    [SerializeField]
    private float LeftPosMaxB;//big is -275.0f
    [SerializeField]
    private Vector3 RightMapSides;
    [SerializeField]
    private float RightPos;//big is 410.0f
    [SerializeField]
    private float RightPosMaxT;//big is 275.0f
    [SerializeField]
    private float RightPosMaxB;//big is -275.0f

    //public override void OnStartServer()
    //{
    //InvokeRepeating("SpawnSatellite", this.SatSpawnInterval, this.SatSpawnInterval);
    //InvokeRepeating("SpawnScrap0", this.Sc0SpawnInterval, this.Sc0SpawnInterval);

    //}

    //[Server]
    public void SpawnSatellite()
    {
        if (GameObject.FindGameObjectsWithTag("Satellite").Length <= SatSpawnTotal)
        {
            int sideSelect = Random.Range(0, 4);
            Vector2 spawnPosition;
            if (sideSelect == 0)
            {
                TopMapSides = new Vector2(Random.Range(TopPosMaxL, TopPosMaxR), TopPos);
                spawnPosition = TopMapSides;
                //GameObject Satellite = Instantiate(SatellitePrefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("satellite", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Satellite);
            }
            else if (sideSelect == 1)
            {
                BottomMapSides = new Vector2(Random.Range(BotPosMaxL, BotPosMaxR), BotPos);
                spawnPosition = BottomMapSides;
                //GameObject Satellite = Instantiate(SatellitePrefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("satellite", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Satellite);
            }
            else if (sideSelect == 2)
            {
                LeftMapSides = new Vector2(LeftPos, Random.Range(LeftPosMaxB, LeftPosMaxT));
                spawnPosition = LeftMapSides;
                //GameObject Satellite = Instantiate(SatellitePrefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("satellite", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Satellite);
            }
            else
            {
                RightMapSides = new Vector2(RightPos, Random.Range(RightPosMaxB, RightPosMaxT));
                spawnPosition = RightMapSides;
                //GameObject Satellite = Instantiate(SatellitePrefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("satellite", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Satellite);
            }
        }
    }
    //[Server]
    public void SpawnScrap0()
    {
        if (GameObject.FindGameObjectsWithTag("Sc2").Length <= Sc0SpawnTotal)
        {
            Vector2 spawnPosition;
            int sideSelect = Random.Range(0, 4);
            if (sideSelect == 0)
            {
                TopMapSides = new Vector2(Random.Range(TopPosMaxL, TopPosMaxR), TopPos);
                spawnPosition = TopMapSides;
                //GameObject Scrap = Instantiate(Resource0Prefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("scrap_2", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Scrap);
            }
            else if (sideSelect == 1)
            {
                BottomMapSides = new Vector2(Random.Range(BotPosMaxL, BotPosMaxR), BotPos);
                spawnPosition = BottomMapSides;
                //GameObject Scrap = Instantiate(Resource0Prefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("scrap_2", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Scrap);
            }
            else if (sideSelect == 2)
            {
                LeftMapSides = new Vector2(LeftPos, Random.Range(LeftPosMaxB, LeftPosMaxT));
                spawnPosition = LeftMapSides;
                //GameObject Scrap = Instantiate(Resource0Prefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("scrap_2", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Scrap);
            }
            else
            {
                RightMapSides = new Vector2(RightPos, Random.Range(RightPosMaxB, RightPosMaxT));
                spawnPosition = RightMapSides;
                //GameObject Scrap = Instantiate(Resource0Prefab, spawnPosition, Quaternion.identity) as GameObject;
                Vector3 force = -spawnPosition.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };
                PhotonNetwork.InstantiateRoomObject("scrap_2", spawnPosition, Quaternion.identity, 0, instantiationData);
                //NetworkServer.Spawn(Scrap);
            }
        }
    }

    //[Server]
    public void SpawnScrap1(Vector2 location)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 spawnPosition = location;
            //GameObject Scrap = Instantiate(Resource1Prefab, spawnPosition, Quaternion.identity) as GameObject;
            Vector3 force = -spawnPosition.normalized * 1000.0f;
            Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
            object[] instantiationData = { force, torque, true };
            PhotonNetwork.InstantiateRoomObject("scrap_1", spawnPosition, Quaternion.identity, 0, instantiationData);
            //Scrap.transform.Rotate(0.0f, 0.0f, Random.Range(0f, 359f), Space.Self);
            //Scrap.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8f, 8f), Random.Range(-8f, 8f));
            //NetworkServer.Spawn(Scrap);
            //Destroy(Scrap, 10f);
        }

    }
    //[Server]
    public void SpawnScrap2(int amount, Vector2 location)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPosition = location;
            //GameObject Scrap = Instantiate(Resource2Prefab, spawnPosition, Quaternion.identity) as GameObject;
            Vector3 force = -spawnPosition.normalized * 1000.0f;
            Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
            object[] instantiationData = { force, torque, true };
            PhotonNetwork.InstantiateRoomObject("scrap_2", spawnPosition, Quaternion.identity, 0, instantiationData);
            //Scrap.transform.Rotate(0.0f, 0.0f, Random.Range(0f, 359f), Space.Self);
            //Scrap.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8f, 8f), Random.Range(-8f, 8f));
            //NetworkServer.Spawn(Scrap);
            //Destroy(Scrap, 10f);
        }
    }
}
