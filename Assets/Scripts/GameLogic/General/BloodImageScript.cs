
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodImageScript : MonoBehaviour
{
    private Image m_MyImage;

    [SerializeField]
    private float m_VanishSpeed = 10;

    private Coroutine m_ShowCoroutine;

    public void Show()
    {
        this.m_ShowCoroutine = StartCoroutine(IenumShow());
    }

    // Start is called before the first frame update
    void Start()
    {
        this.m_MyImage = GetComponent<Image>();
    }

    private IEnumerator IenumShow()
    {
        float CurrentVal = 1f;
        this.m_MyImage.color = new Color(1, 1, 1, CurrentVal);

        while (CurrentVal > 0)
        {
            if (CurrentVal - this.m_VanishSpeed / 1000 > 0)
            {
                CurrentVal -= this.m_VanishSpeed / 1000;
                this.m_MyImage.color = new Color(1, 1, 1, CurrentVal);
            }
            else
            {
                this.m_MyImage.color = new Color(1, 1, 1, 0);

                if (this.m_ShowCoroutine != null)
                {
                    StopCoroutine(this.m_ShowCoroutine);
                }
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
