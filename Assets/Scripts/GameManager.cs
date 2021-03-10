using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> players = new List<GameObject>();
    private static GameManager _instance;
    private static int _sceneNumber = 0;
    private static Rigidbody _rb;

    #region Scene1
    private float _cameraVelocity = 0.5f;
    #endregion

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        _rb = Camera.main.gameObject.GetComponent<Rigidbody>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        Scene1();
    }

    #region PlayerObjects
    public static void AddPlayerObject(GameObject obj)
    {
        players.Add(obj);
    }

    public static void RemovePlayerObject(GameObject obj)
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
    #endregion

    public static void NextScene()
    {
        _sceneNumber++;
        switch (_sceneNumber)
        {
            case 1:

                break;

            default:
                break;
        }

    }

    private void Scene1()
    {
        _rb.velocity = new Vector3(0,0, -_cameraVelocity);
    }
}
