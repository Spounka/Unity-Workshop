using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager GM;

    public Image respawnImage;

    public TMPro.TMP_Text startText;
    public GameObject startPanel;

    public AudioClip[] sounds = new AudioClip[(int)AudioSounds.ALL];

    public bool canPlay = false;
    public GameObject EnemyPrefab;

    public float maxEnemies = 4;

    public GameObject[] boosters = new GameObject[2];

    float time = 0, timeToRespawn = 3f;
    public float boosterTime = 0, boosterLimit = 5f;
    float startTime = 5;

    public List<GameObject> enabledBooster;

    [SerializeField]
    List<Transform> BoostersPositions;

    public static bool isGameOver = false;

    public List<GameObject> enemies;


    [SerializeField]
    public List<Transform> respawnPoints;

    public Transform enemySpawnPoint;

    void Awake()
    {
        
    }

    void Start()
    {
        // foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>())
        // {
        //     g.SetActive(false);
        //     this.gameObject.SetActive(true);
        //     Camera.main.gameObject.SetActive(true);
        // }
        
        if(GM != this && GM != null)
        {
            Destroy(GM.gameObject);
        }
        else
        {
            GM = this;
        }

        
        


        foreach(GameObject g in GameObject.FindGameObjectsWithTag("SpawnPoints"))
        {
            respawnPoints.Add(g.transform);
        }

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Boosters"))
        {
            BoostersPositions.Add(g.transform);
        }
    }

    void Update()
    {
        if(!canPlay)
        {
            startTime -= Time.deltaTime;
            startText.text = "Game Starts In: " + ((int)startTime).ToString();
            
            if(startTime <= 0)
            {
                canPlay = true;
                foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>())
                {
                    if(!g.activeInHierarchy)
                        g.SetActive(true);
                }
                startPanel.SetActive(false);
            }
        }
        else
        {
            Timer(ref time);
            Timer(ref boosterLimit);
            print(boosterTime);
            CreateBooster();
            if(GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies && time == 0)
            {
                time = timeToRespawn;
                GameObject obj = CreateObject(EnemyPrefab, Enemy.RandomNav(enemySpawnPoint.position, 20f, -1), "Enemy");
                enemies.Add(obj);
            }

            if(enemies.Count > maxEnemies)
            {
                Destroy(enemies[enemies.Count - 1]);
                enemies.Remove(enemies[enemies.Count - 1]);
            }
        }
    }
    public void Timer(ref float _time)
    {
        // _time = 0;
        while(_time > 0)
        {
            _time -= Time.deltaTime;
            // print(_time);
        }

        if(time <= 0)
            time = 0;
    }

    GameObject CreateObject(GameObject objToMake, Vector3 _postion, string objName)
    {
        GameObject @object;

        @object = Instantiate(objToMake, _postion, Quaternion.identity) as GameObject;
        @object.name = objName;

        return @object;
    }

    void CreateBooster()
    {
        if(boosterTime <= 0)
        {
            int randIndex = Random.Range(0, BoostersPositions.Count);
            if(enabledBooster.Count > 0)
            {
                foreach(GameObject g in enabledBooster)
                {
                    if((g.transform.position.x == BoostersPositions[randIndex].position.x
                        && g.transform.position.z == BoostersPositions[randIndex].position.z))
                    {
                        return;
                    }
                }
            }
            GameObject booster = CreateObject(boosters[Random.Range(0, boosters.Length)],
                                            BoostersPositions[randIndex].position,
                                            "Booster");
            enabledBooster.Add(booster);
            boosterTime = boosterLimit;
        }
        // else
        // {
        //     Timer(ref boosterLimit);
        // }
    }
}

enum AudioSounds : int {Shoot, ALL};

