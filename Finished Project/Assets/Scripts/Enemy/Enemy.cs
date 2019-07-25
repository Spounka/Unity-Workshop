using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    PlayerManager playerManager;

    AudioSource m_PlayerSource;
    public ParticleSystem muzzleFlush;
    CharacterController m_CharacterController;

    public bool isHit = false;

    public int Damage {get;set;}
    private Vector3 yVelocity;

    [SerializeField]
    float detectionRange = 35f, shootRange = 20f, rateOfFire = 3f;

    [SerializeField]
    int health = 100;

    public int Health{get => health; set => health = value;}

    float time = 0;

    bool isShooting;

    public bool isRespawning{get;set;}

    void OnEnable()
    {
        Damage = 10;
        Health = 100;
        isRespawning = false;
        m_PlayerSource = GetComponent<AudioSource>();
        m_CharacterController = GetComponent<CharacterController>();
        playerManager = FindObjectOfType<PlayerManager>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
        if (time > 0)
            time -= Time.deltaTime;
        if( time < 0)
            time = 0;

        
        // RaycastHit hit;
        // bool s = Physics.Raycast(transform.position, transform.forward, out hit, detectionRange);
        Collider[] results = new Collider[10];
        Physics.OverlapSphereNonAlloc(transform.position, detectionRange, results);
        foreach( Collider c in results)
        {
            if(c != null)
            {
                if(c.tag == "Player" || c.tag == "Enemy")
                {
                    Vector3 difference = Vector3.zero;
                    Move(c.transform,ref difference, detectionRange);
                    if(difference.magnitude <= shootRange)
                    {
                        // isShooting = true;
                        transform.LookAt(c.transform);
                        RaycastHit hit;
                        if(Physics.Raycast(this.transform.position, difference,out hit, shootRange))
                        {
                            if(hit.collider.tag == c.tag)
                                Shoot(c.transform);
                            else
                            {
                                Move(c.transform, ref difference, detectionRange);
                            }
                        }
                    }
                }
                else
                {
                    if(agent.isStopped)
                        this.agent.SetDestination(RandomNav(this.transform.position, 50f, -1));

                }
            }
        
        }

        if(Health <= 0)
        {
            GameManager.GM.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    
    void Move(Transform _target, ref Vector3 diff, float detectRange)
    {
        diff = (_target.position - this.transform.position);
        
        if(diff.magnitude <= detectRange)
        {
            // print("Magnitude For Player Working");
            if(this.agent.SetDestination(_target.position))
            {
                // print("Set Destination For Player Working");
                transform.LookAt(_target.transform);
                // this.agent.updateRotation = true;
            }

        }
    }

    void Shoot(Transform target)
    {
        if(time == 0)
        {
            time = rateOfFire;
            transform.LookAt(target);
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, shootRange))
            {
                if(hit.collider.tag == "Player")
                {
                    playerManager.Health -= Damage;
                    
                }
                else if(hit.collider.tag == "Enemy")
                {
                    hit.transform.GetComponent<Enemy>().Health -= Damage;
                }

                muzzleFlush.gameObject.SetActive(true);
                muzzleFlush.Play();
                m_PlayerSource.clip = GameManager.GM.sounds[(int)AudioSounds.Shoot];
                m_PlayerSource.Play();
            }
            
            
        }
        if(muzzleFlush.IsAlive() == false)
        {
            muzzleFlush.gameObject.SetActive(false);
        }
    }

    public static Vector3 RandomNav(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance,layerMask);
        return navHit.position;
    }
    
}


