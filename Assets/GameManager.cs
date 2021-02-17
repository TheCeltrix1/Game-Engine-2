using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> players = new List<GameObject>();

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void AddObject(GameObject obj)
    {
        players.Add(obj);
    }

    public static void RemoveObject(GameObject obj)
    {
        players.Remove(obj);
    }

    public static GameObject NearestPlayer(GameObject obj)
    {
        float dist = Mathf.Infinity;
        float distance;
        GameObject nearest = players[0];
        foreach (GameObject gameObject in players)
        {
            distance = Vector3.Distance(gameObject.transform.position, obj.transform.position);
            if (distance <= dist)
            {
                nearest = gameObject;
            }
        }
        return nearest;
    }
}
