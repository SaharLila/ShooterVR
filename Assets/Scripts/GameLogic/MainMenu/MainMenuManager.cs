
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_ZombieStartPointTransform;

    [SerializeField]
    private GameObject m_ZombiePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Survival Mode");
    }

    public void CreateZombie()
    {
        if(GameObject.FindObjectOfType<ZombieScript>() == null)
        {
            Instantiate(this.m_ZombiePrefab, this.m_ZombieStartPointTransform.position, Quaternion.identity);
        }
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
