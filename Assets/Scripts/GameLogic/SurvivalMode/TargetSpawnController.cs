using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MakeAutoGunTarget;

    [SerializeField]
    private GameObject m_KillAllZombiesTarget;

    [SerializeField]
    private GameObject m_DoubleDamageTarget;

    [SerializeField]
    private Transform m_MakeAutoGunTargetPlace;

    [SerializeField]
    private Transform m_KillAllZombiesTargetPlace;

    [SerializeField]
    private Transform m_DoubleDamageTargetPlace;

    [SerializeField]
    private Transform m_PlayerTransform;

    private void createTarget(GameObject i_Target, Transform i_ParentTransform)
    {
        if (i_ParentTransform.childCount == 0)
        {
            GameObject Target = Instantiate(i_Target, i_ParentTransform.position, Quaternion.identity, i_ParentTransform);

            Target.transform.LookAt(this.m_PlayerTransform);
        }
    }

    public enum eTargetType
    {
        eKillAllZombies,
        eMakeGunAuto,
        eDoubleDamage
    }

    public void InstantiateNewTarget(eTargetType i_TargetType)
    {
        switch(i_TargetType)
        {
            case eTargetType.eDoubleDamage:
                createTarget(this.m_DoubleDamageTarget, this.m_DoubleDamageTargetPlace);
                break;

            case eTargetType.eKillAllZombies:
                createTarget(this.m_KillAllZombiesTarget, this.m_KillAllZombiesTargetPlace);
                break;

            case eTargetType.eMakeGunAuto:
                createTarget(this.m_MakeAutoGunTarget, this.m_MakeAutoGunTargetPlace);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}