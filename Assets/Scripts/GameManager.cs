using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> players = new List<GameObject>();
    public static bool endScene = false;
    public AudioSource BGM;
    private static GameManager _instance;
    private static int _sceneNumber = 0;
    private Rigidbody _rb;

    #region Scene1
    private float _cameraVelocity = 0.5f;
    #endregion

    #region Scene3
    private float _scene3TimerCurrentTime = 0;
    private float _scene3TimerTotalTime = 5;
    #endregion

    #region Scene7
    private float _explosionTimer = 1f;
    private ParticleSystem _boomBoom;
    private CamShake _cameraShake;
    private bool _once = false;
    private bool _onceTwo = false;
    #endregion

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
            DontDestroyOnLoad(this.gameObject);
        }
        BGM.Play();
        BGM.loop = true;
        _sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        CheckSceneEnd();
        string name = SceneManager.GetActiveScene().name;
        switch (name)
        {
            case "Scene1":
                Scene1();
                break;

            case "Scene2":

                break;

            case "Scene3":
                _scene3TimerCurrentTime += Time.deltaTime;
                if (_scene3TimerCurrentTime >= _scene3TimerTotalTime) NextScene();
                break;

            case "Scene4":

                break;

            case "Scene5":

                break;

            case "Scene6":

                break;

            case "Scene7":
                if (endScene && !_once)
                {
                    _boomBoom = Camera.main.gameObject.GetComponentInChildren<ParticleSystem>();
                    _boomBoom.Play();
                    Camera.main.gameObject.GetComponentInChildren<AudioSource>().Play();
                    _cameraShake = FindObjectOfType<CamShake>();
                    StartCoroutine(_cameraShake.Shake(0.1f, 0.5f));
                    _once = true;
                }
                if (_once)
                {
                    if(_explosionTimer <= 0) _onceTwo = true;
                    _explosionTimer -= Time.deltaTime;
                }
                break;

            case "Scene8":
                Application.Quit();
                break;

            default:
                break;
        }
        if (endScene && name != "Scene7") NextScene();
        else if (name == "Scene7" && _onceTwo) NextScene();
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
            //Debug.Log($"Distance: {distance} DIST {dist}");
            if (distance <= dist)
            {
                nearest = player;
                dist = distance;
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
        _rb = Camera.main.gameObject.GetComponent<Rigidbody>();
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
