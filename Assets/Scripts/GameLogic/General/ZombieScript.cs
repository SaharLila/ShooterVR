using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GoogleVR.VideoDemo;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class ZombieScript : MonoBehaviour
{
    [SerializeField]
    private Animator m_ZombieAnimator;

    public GameManagerScript GameManagerScript { get; set; }

    [SerializeField]
    private Target m_TargetScript;

    [SerializeField]
    private Transform m_PlayerTransform;

    [SerializeField]
    private NavMeshAgent m_ZombieNavMeshAgent;

    private PlayerScript m_PlayerScript;

    [SerializeField]
    public PlayerScript PlayerScript
    {
        get
        {
            return this.m_PlayerScript;
        }
        set
        {
            this.m_PlayerScript = value;
        }
    }

    [SerializeField]
    private float m_AttackIntervalTime = 3f;

    private bool m_IsFirstAttack = true;

    private IEnumerator m_AttackCoroutine;

    private IEnumerator m_SoundCoroutine;

    [SerializeField]
    private AudioSource m_ZombieSound;

    public event Action ZombieAttackEventHandler;

    private BoxCollider m_ZombieCollider;

    private bool m_IsDead = false;

    // Start is called before the first frame update
    void Start()
    {
        this.m_ZombieCollider = this.gameObject.GetComponentInChildren<BoxCollider>();
        this.m_PlayerScript = GameObject.FindObjectOfType<PlayerScript>();
        this.m_PlayerTransform = this.m_PlayerScript.transform;
        this.m_TargetScript.GoingToDie += m_TargetScript_GoingToDie;
        this.m_TargetScript.GotHit += m_TargetScript_GotHit;
        this.m_ZombieNavMeshAgent.SetDestination(this.m_PlayerTransform.position);
        this.m_SoundCoroutine = sound();
        StartCoroutine(this.m_SoundCoroutine);
    }

   
    public void ZombieIsClose()
    {
        this.m_AttackCoroutine = attack();
        StartCoroutine(this.m_AttackCoroutine);
    }

    private IEnumerator attack()
    {
        while(!this.m_IsDead)
        {
            this.m_ZombieAnimator.SetTrigger("Attack");
            if (!this.m_IsFirstAttack)
            {
                this.PlayerScript.GetDamage();
            }

            this.m_IsFirstAttack = false;
            yield return new WaitForSeconds(this.m_ZombieAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private IEnumerator sound()
    {
        while (!this.m_IsDead) 
        {
            if(this.m_ZombieSound != null)
            {
                this.m_ZombieSound.Play();
            }

            yield return new WaitForSeconds(new System.Random().Next(5,10));
        }
    }

    private void m_TargetScript_GotHit()
    {
        this.m_ZombieAnimator.SetTrigger("GotHit");
    }

    private void m_TargetScript_GoingToDie()
    {
        this.m_IsDead = true;
        StopCoroutine(this.m_SoundCoroutine);

        if(this.m_AttackCoroutine != null)
        {
            StopCoroutine(this.m_AttackCoroutine);
        }

        Die();
    }

    public void Die()
    {
        if (this.m_ZombieCollider != null)
        {
            this.m_ZombieCollider.enabled = false;
        }
        this.m_ZombieNavMeshAgent.enabled = false;
        this.m_ZombieAnimator.applyRootMotion = true;
        this.m_ZombieAnimator.SetBool("Death", true);

        if(this.GameManagerScript != null)
        {
            this.GameManagerScript.IncreaseCountOfZombieKilled();
        }

        Destroy(this.gameObject, 5);
    }

    protected virtual void OnZombieAttack()
    {
        ZombieAttackEventHandler?.Invoke();
    }
}
