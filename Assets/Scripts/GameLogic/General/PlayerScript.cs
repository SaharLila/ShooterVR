using System;
using System.Collections;
using System.Collections.Generic;
using Gvr.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public event Action GoingToDie;
    public event Action<int> GotDamage;

    private bool m_IsDead = false;

    [SerializeField]
    private int m_Health = 40;

    [SerializeField]
    private BloodScript m_BloodScript;

    public void AddHealth()
    {
        this.m_Health += 10;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Zombie")
        {
            ZombieScript zombieScript = collision.gameObject.GetComponentInParent<ZombieScript>();
            zombieScript.ZombieIsClose();
        }
    }

    public void GetDamage()
    {
        this.m_BloodScript.Show();
        this.m_Health -= 10;

        OnGotDamage();
        if (this.m_Health <= 0 && SceneManager.GetActiveScene().name != "MainMenu" && !this.m_IsDead)
        {
            this.m_IsDead = true;
            OnGoingToDie();
        }
    }

    protected virtual void OnGoingToDie()
    {
        GoingToDie?.Invoke();
    }

    protected virtual void OnGotDamage()
    {
        GotDamage?.Invoke(Math.Max(0, this.m_Health));
    }
}
