using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    [SerializeField]
    private float m_Damage = 10f;

    [SerializeField]
    private float m_Range = 100f;

    [SerializeField]
    private Camera m_playerCamera;

    [SerializeField]
    private ParticleSystem m_MuzzleFlush;

    [SerializeField]
    private GameObject m_HitEffect;

    [SerializeField]
    private GameObject m_BloodHitEffect;

    private bool m_IsAuto;

    private bool m_IsDoubleDamage;

    public event Action HitKillAllZombiesTargetEventHandler;
    
    private float m_TimeToBeAuto;

    private float m_AutoTime = 10f;

    [SerializeField]
    private float m_FireRate = 7f;

    private float m_nextTimeToFire = 0f;

    private float m_TimeToBeDoubleDamage;

    [SerializeField]
    private float m_DoubleDamageTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
        this.m_TimeToBeDoubleDamage = this.m_DoubleDamageTime;
        this.m_IsDoubleDamage = false;
        this.m_IsAuto = false;
        this.m_TimeToBeAuto = this.m_AutoTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        if(this.m_IsAuto)
        {
            countDownAuto();
        }

        if(this.m_IsDoubleDamage)
        {
            countDownDoubleDamage();
        }
    }

    private void countDownDoubleDamage()
    {
        this.m_TimeToBeDoubleDamage -= Time.deltaTime;

        if (this.m_TimeToBeDoubleDamage <= 0)
        {
            this.m_Damage /= 2;
            this.m_IsDoubleDamage = false;
            this.m_TimeToBeDoubleDamage = this.m_DoubleDamageTime;
        }
    }

    private void countDownAuto()
    {
        this.m_TimeToBeAuto -= Time.deltaTime;

        if(this.m_TimeToBeAuto <= 0)
        {
            this.m_IsAuto = false;
            this.m_TimeToBeAuto = this.m_AutoTime;
        }
    }

    private void CheckInput()
    {
        if(Time.timeScale != 0f)
        {
            if(this.m_IsAuto)
            {
                if(Time.fixedTime >= this.m_nextTimeToFire)
                {
                    this.m_nextTimeToFire = Time.fixedTime + 1f / this.m_FireRate;
                    shoot();
                }
            }
            else
            {
                if(Input.anyKeyDown)
                {
                    shoot();
                }
            }
        }
        else
        {
            if(Input.anyKeyDown)
            {
                RaycastHit hitInfo;
                bool isHit = raycastForword(out hitInfo);

                if (isHit)
                {
                    checkIfRayIntoUIBtn(hitInfo);
                }
            }
        }
    }

    private bool raycastForword(out RaycastHit i_HitInfo)
    {
        Vector3 from = this.m_playerCamera.transform.position;
        Vector3 forward = this.m_playerCamera.transform.forward;
        return Physics.Raycast(from, forward, out i_HitInfo, this.m_Range);
    }

    private void shoot()
    {
        RaycastHit hitInfo;
        this.m_MuzzleFlush.Play();
        this.gameObject.GetComponent<AudioSource>().Play();
        bool isHit = raycastForword(out hitInfo);

        if (isHit)
        {
            fetchHitInfo(hitInfo);
            makeHitEffect(hitInfo ,hitInfo.transform.tag);
        }
    }

    private void makeHitEffect(RaycastHit i_HitInfo , string i_HitObjectTag)
    {
        GameObject effectGameObject;

        if (i_HitObjectTag == "Zombie")
        {
            effectGameObject = Instantiate(this.m_BloodHitEffect, i_HitInfo.point, Quaternion.LookRotation(i_HitInfo.normal));
            effectGameObject.transform.parent = i_HitInfo.transform;
        }
        else
        {
            effectGameObject = Instantiate(this.m_HitEffect, i_HitInfo.point, Quaternion.LookRotation(i_HitInfo.normal));
        }

        Destroy(effectGameObject, 3f);
    }

    private void fetchHitInfo(RaycastHit i_HitInfo)
    {
        Target theTarget = i_HitInfo.transform.gameObject.GetComponent<Target>();

        if(theTarget != null)
        {
            theTarget.TakeDamage(this.m_Damage);
        }

        if(i_HitInfo.transform.tag == "MakeGunAutoTarget")
        {
            this.m_IsAuto = true;
        }

        if (i_HitInfo.transform.tag == "KillAllZombiesTarget")
        {
            OnHitKillAllZombiesTarget();
        }

        if (i_HitInfo.transform.tag == "DoubleDamageTarget")
        {
            this.m_Damage *= 2;
            this.m_IsDoubleDamage = true;
        }

        checkIfRayIntoUIBtn(i_HitInfo);
    }

    public void checkIfRayIntoUIBtn(RaycastHit i_HitInfo)
    {
        if (i_HitInfo.transform.tag == "UI")
        {
            Button theButton = i_HitInfo.transform.GetComponent<Button>();

            if (theButton != null)
            {
                theButton.onClick.Invoke();
            }
        }
    }

    protected virtual void OnHitKillAllZombiesTarget()
    {
        HitKillAllZombiesTargetEventHandler?.Invoke();
    }
}
