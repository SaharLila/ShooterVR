using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float m_Health = 50f;

    [SerializeField]
    private float m_DestroyTime = 3f;

    [SerializeField]
    private AudioSource m_MyAudioSourceWhenImDead;

    public event Action GoingToDie; 
    public event Action GotHit; 

    public void TakeDamage(float i_DamageToTake)
    { 
        this.m_Health -= i_DamageToTake;
        OnGotHit();

        if(this.m_Health <= 0f)
        {
            die();
        }
    }

    private void die()
    {
        playSound();

        if (this.transform.tag != "Zombie")
        {
            MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
            BoxCollider boxCollider = this.GetComponent<BoxCollider>();
            MeshCollider meshCollider = this.GetComponent<MeshCollider>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            if(boxCollider != null)
            {
                boxCollider.enabled = false;
            }
            else if(meshCollider != null)
            {
                meshCollider.enabled = false;
            }

            
            Destroy(this.gameObject, this.m_DestroyTime);
        }

        OnGoingToDie();
    }


    protected virtual void OnGoingToDie()
    {
        GoingToDie?.Invoke();
    }

    private void playSound()
    {
        if(this.m_MyAudioSourceWhenImDead != null)
        {
            this.m_MyAudioSourceWhenImDead.Play();
        }
    }

    protected virtual void OnGotHit()
    {
        GotHit?.Invoke();
    }
}
