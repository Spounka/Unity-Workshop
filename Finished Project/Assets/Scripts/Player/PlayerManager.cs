using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Slider HealthBar;
    public ParticleSystem muzzleFlush;

    [SerializeField]
    private float shootRange = 50f, rateOfFire = 0.6f;
    [SerializeField]
    private int health = 100, maxHealth = 100;
    private float time;

    public AudioSource m_PlayerSource;

    PlayerMovement playerMovement;


    #region Properties

    public int Health{get{return health;} set{health = value;}}
    public int MaxHealth{get => maxHealth;  set => maxHealth = value;}

    public int Damage{get;set;}

    public bool isRespawning{get;set;}

    #endregion

    void Start()
    {
        Damage = 10;
        m_PlayerSource = GetComponent<AudioSource>();
        isRespawning = false;
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    void Update()
    {

        if(isRespawning)
        {
            Health = MaxHealth;
        }

        HealthBar.maxValue = MaxHealth;
        HealthBar.value = Health;
        Shoot();

        if(Health <= 0 && !isRespawning)
        {
            StartCoroutine(Respawn());
        }

        if(time > 0)
            time -= Time.deltaTime;
        if(time < 0)
            time = 0;
        
    }

    void Shoot()
    {
        if(Input.GetMouseButton(0) && time == 0)
        {
            time = rateOfFire;
            Camera m_Camera = Camera.main;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, shootRange))
            {
                if(hit.collider.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<Enemy>().Health -= Damage;
                    hit.transform.gameObject.GetComponent<Enemy>().isHit = true;
                }
            }
            muzzleFlush.gameObject.SetActive(true);
            muzzleFlush.Play();
            m_PlayerSource.clip = GameManager.GM.sounds[(int)AudioSounds.Shoot];
            m_PlayerSource.Play();
        }
        if(muzzleFlush.IsAlive() == false)
        {
            muzzleFlush.gameObject.SetActive(false);
        }
        

    }

    IEnumerator Respawn()
    {
        
        yield return new WaitForSeconds(0);
        isRespawning = true;
        Transform x = GameManager.GM.respawnPoints[Random.Range(0, GameManager.GM.respawnPoints.Count)];
        gameObject.transform.position = x.position;
        isRespawning = true;
        GameManager.GM.respawnImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        GameManager.GM.respawnImage.gameObject.SetActive(false);
        isRespawning = false;
    }
}
