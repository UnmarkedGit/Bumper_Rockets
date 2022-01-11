using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;


//[System.Serializable]
//public class SyncListGameObj : SyncList<GameObject> { }
public class RankingAndScoring : MonoBehaviourPunCallbacks
{
    //public SyncListGameObj playerList = new SyncListGameObj();
    //public SyncListGameObj RankSortedPlayerList = new SyncListGameObj();
    public List<GameObject> playerList;
    public List<GameObject> RankSortedPlayerList;
    public GameObject[] players = new GameObject[20];
    public void Update()
    {
        FillPlayerList();
    }
    public void Start()
    {
        GameObject[] playerCus = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in playerCus)
        {
            //p.GetComponent<PlayerUI>().RpcPaint( new Color(p.GetComponent<PlayerUI>().RedInput, p.GetComponent<PlayerUI>().GreenInput, p.GetComponent<PlayerUI>().BlueInput, 1.0f) );
            //p.GetComponent<SpriteRenderer>().material.color = new Color(p.GetComponent<PlayerUI>().RedInput, p.GetComponent<PlayerUI>().GreenInput, p.GetComponent<PlayerUI>().BlueInput, 1.0f);
            p.GetComponent<PlayerUI>().CmdPaint(new Color(p.GetComponent<PlayerUI>().RedInput, p.GetComponent<PlayerUI>().GreenInput, p.GetComponent<PlayerUI>().BlueInput, 1.0f));
        }
    }
    void FillPlayerList()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        PlayerRankSorting();
        if (players.Length == playerList.Count)
            return;

        playerList = new List<GameObject>();
        foreach (GameObject p in players)
        {
            playerList.Add(p);
        }
    }
    void PlayerRankSorting()
    {
        RankSortedPlayerList = new List<GameObject>();
        RankSortedPlayerList = Quicksort(playerList);

    }

    //QuickSort function for scrap and size
    static List<GameObject> Quicksort(List<GameObject> list)
    {
        if (list.Count <= 1) return list;
        int pivotPosition = list.Count / 2;
        GameObject pivotValue = list[pivotPosition];
        list.RemoveAt(pivotPosition);
        List<GameObject> smaller = new List<GameObject>();
        List<GameObject> greater = new List<GameObject>();
        foreach (GameObject item in list)
        {
            if (((item.GetComponent<PlayerControls>().GetSize() - 1) * 100) + (int)item.GetComponent<PlayerControls>().GetScrap()
                > ((pivotValue.GetComponent<PlayerControls>().GetSize() - 1) * 100) + (int)pivotValue.GetComponent<PlayerControls>().GetScrap())
            {
                smaller.Add(item);
            }
            else
            {
                greater.Add(item);
            }
        }
        List<GameObject> sorted = Quicksort(smaller);
        sorted.Add(pivotValue);
        sorted.AddRange(Quicksort(greater));
        return sorted;
    }


    public List<GameObject> GetSortedList()
    {
        List<GameObject> giveSorted = new List<GameObject>();
        giveSorted.AddRange(RankSortedPlayerList);
        return giveSorted;
    }
    /*[PunRPC]
    public List<GameObject> RpcGetSortedList()
    {
        List<GameObject> giveSorted = new List<GameObject>();
        giveSorted.AddRange(RankSortedPlayerList);
        return giveSorted;
    }
    public void CmdGetSortedList()
    {
        //List<GameObject> giveSorted = new List<GameObject>();
        //giveSorted.AddRange(RankSortedPlayerList);
        //return giveSorted;
       // List<GameObject> giveSorted = photonView.RPC("RpcGetSortedList", RpcTarget.AllViaServer);
    }*/

}
