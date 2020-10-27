using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ReplacementBarrel;

    [SerializeField]
    private GameObject m_MobOfZombies;

    [SerializeField]
    private Target m_TargetScript;

    [SerializeField]
    private float m_KillingRadius;


    // Start is called before the first frame update
    void Start()
    {
        this.m_TargetScript = this.GetComponent<Target>();

        if (this.m_TargetScript != null)
        {
            this.m_TargetScript.GoingToDie += Explode;
        }

    }

    private void Explode()
    {
        List<ZombieScript> zombies = getZombiesInRange();

        foreach(ZombieScript zombie in zombies)
        {
            zombie.Die();
        }

        createReplacement();
    }

    private void createReplacement()
    {
        Instantiate(this.m_ReplacementBarrel, this.transform.position, Quaternion.identity);
    }

    private List<ZombieScript> getZombiesInRange()
    {
        List<ZombieScript> result = new List<ZombieScript>();

        if (this.m_MobOfZombies != null)
        {
            ZombieScript[] zombies = this.m_MobOfZombies.GetComponentsInChildren<ZombieScript>();

            foreach(ZombieScript zombie in zombies)
            {
                if (Vector3.Distance(zombie.transform.position, this.transform.position) < this.m_KillingRadius)
                {
                    result.Add(zombie);
                }
            }
        }

        return result;
    }
}
