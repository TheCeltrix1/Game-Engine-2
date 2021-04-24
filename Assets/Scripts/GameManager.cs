using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> players = new List<GameObject>();
    public static bool endScene = false;
    private static GameManager _instance;
    private static int _sceneNumber = 0;
    private static Rigidbody _rb;

    #region Scene1
    private float _cameraVelocity = 0.5f;
    #endregion

    #region Scene3
    private float _scene3TimerCurrentTime = 0;
    private float _scene3TimerTotalTime = 5;
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
            DontDestroyOnLoad(this.gameObject);
        }
        _sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        CheckSceneEnd();
        switch (_sceneNumber)
        {
            case 0:
                Scene1();
                break;

            case 1:

                break;

            case 2:
                _scene3TimerCurrentTime += Time.deltaTime;
                if (_scene3TimerCurrentTime >= _scene3TimerTotalTime) NextScene();
                break;

            case 3:
                
                break;

            case 4:
                
                break;

            case 5:
                
                break;

            case 6:
                
                break;

            default:
                break;
        }
        if (endScene) NextScene();
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
        foreach (GameObject player in players)
        {
            distance = Vector3.Distance(player.transform.position, obj.transform.position);
            if (distance <= dist)
            {
                nearest = player;
            }
        }
        return nearest;
    }
    #endregion

    public static void NextScene()
    {
        endScene = false;
        players.Clear();
        _sceneNumber++;
        if (_sceneNumber >= SceneManager.sceneCountInBuildSettings) Application.Quit();
        else SceneManager.LoadScene(_sceneNumber);
    }

    private void Scene1()
    {
        _rb.velocity = new Vector3(0,0, -_cameraVelocity);
    }

    private void CheckSceneEnd() 
    {
        int i = 0;
        foreach (GameObject item in players)
        {
            if (item.GetComponent<PlayerAI>().finalPointReached)
            {
                i++;
                if (i == (players.Count - 1)) endScene = true;
            }
            else break;
        }
    }
}
