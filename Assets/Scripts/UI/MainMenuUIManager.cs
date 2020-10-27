using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    private HighScoreController m_HighScoreController;

    [SerializeField]
    private Text m_HighScoreText;

    public void Start()
    {
        this.m_HighScoreController = new HighScoreController();
        this.m_HighScoreText.text = this.m_HighScoreController.ToString();
    }

    public void ShowPopup(GameObject i_Popup)
    {
        i_Popup.SetActive(true);
    }

    public void ClosePopup(GameObject i_Popup)
    {
        i_Popup.SetActive(false);
    }
}
