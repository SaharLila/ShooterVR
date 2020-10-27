using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobControllerScript : MonoBehaviour
{

    [SerializeField]
    private Transform m_CenterOfTheWorld;

    [SerializeField]
    private GameObject m_Zombie;

    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private GameManagerScript m_GameManagerScript;

    [SerializeField]
    private Transform m_MobOfZombies;

    [SerializeField]
    private float m_RotateSpeed = 20f;

    [SerializeField]
    private float m_ZombieFirstIntervalCreate = 10f;

    private float m_ZombieIntervalTime;

    private int m_ZombieCounter;

    [SerializeField]
    private readonly int r_ZombieappearanceFrequency = 5;


    // Start is called before the first frame update
    void Start()
    {
        this.m_ZombieIntervalTime = this.m_ZombieFirstIntervalCreate;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(this.m_CenterOfTheWorld.position, Vector3.up, this.m_RotateSpeed * Time.deltaTime);
        checkAndCreateZombie();
    }

    private void checkAndCreateZombie()
    {
        this.m_ZombieIntervalTime -= Time.deltaTime;

        if(this.m_ZombieIntervalTime <= 0)
        {
            GameObject zombie = Instantiate(this.m_Zombie, this.transform.position, Quaternion.identity, this.m_MobOfZombies);
            zombie.GetComponent<ZombieScript>().PlayerScript = this.m_Player.GetComponent<PlayerScript>();
            zombie.GetComponent<ZombieScript>().GameManagerScript = this.m_GameManagerScript;
            this.m_GameManagerScript.IncreaseCountOfZombieSpawn();
            this.m_ZombieCounter++;
            

            this.m_ZombieIntervalTime = this.m_ZombieFirstIntervalCreate;
            DecreaseIntervalTime();
        }
    }

    private void DecreaseIntervalTime()
    {
        if(this.m_ZombieCounter % this.r_ZombieappearanceFrequency == 0 && this.m_ZombieFirstIntervalCreate > 1)
        {
            this.m_ZombieFirstIntervalCreate--;
        }
    }

    public void KillAllZombies()
    {
        foreach(ZombieScript zombie in this.m_MobOfZombies.GetComponentsInChildren<ZombieScript>())
        {
            zombie.Die();
        }
    }
}
