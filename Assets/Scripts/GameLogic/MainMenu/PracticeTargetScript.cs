using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class PracticeTargetScript : MonoBehaviour
{
    [SerializeField]
    private Target m_MyTarget;

    [SerializeField]
    private GameObject m_PracticeTargetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.m_MyTarget.GoingToDie += M_MyTarget_GoingToDie;
    }

    private void M_MyTarget_GoingToDie()
    {
        StartCoroutine(createNewTarget());
    }

    private IEnumerator createNewTarget()
    {
         yield return new WaitForSeconds(1f);
        
         if(this.transform.childCount == 0)
         {
             GameObject theNewTarget = Instantiate(
                 this.m_PracticeTargetPrefab,
                 this.transform.position,
                 new Quaternion(0, 0, 0, 0),
                 this.transform);

             this.m_MyTarget = theNewTarget.GetComponent<Target>();
             this.m_MyTarget.GoingToDie += M_MyTarget_GoingToDie;
         }
    }
}
