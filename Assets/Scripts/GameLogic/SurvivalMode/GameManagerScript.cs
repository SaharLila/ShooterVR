using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript m_PlayerScript;

    [SerializeField]
    private int m_ZombiesParameterForCreateKillAllZombiesTarget = 20;

    [SerializeField]
    private MobControllerScript m_MobController;

    [SerializeField]
    private TargetSpawnController m_TargetSpawnController;

    [SerializeField]
    private GunScript m_GunScript;

    [SerializeField]
    private Canvas m_GameOverCanvas;

    [SerializeField]
    private Text m_CurrentHpText;

    [SerializeField]
    private Text m_ZombiesKilledCountText;

    private int m_CountOfZombiesKilled = 0;

    private int m_CountOfZombiesSpawn = 0;

    private HighScoreController m_HighScoreController;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        this.m_HighScoreController = new HighScoreController();
        this.m_PlayerScript.GoingToDie += M_PlayerScript_GoingToDieEventHandler;
        this.m_PlayerScript.GotDamage += M_PlayerScript_GotDamage;
        this.m_GunScript.HitKillAllZombiesTargetEventHandler += M_GunScript_HitKillAllZombiesTargetEventHandler;
    }

    private void M_PlayerScript_GotDamage(int i_PlayerCurrentHealth)
    {
        this.m_CurrentHpText.text = string.Format("HP : {0}",i_PlayerCurrentHealth);
    }

    private void M_PlayerScript_GoingToDieEventHandler()
    {
        this.m_HighScoreController.AddScore(this.m_CountOfZombiesKilled);
        StartCoroutine(gameOver());
    }

    private IEnumerator gameOver()
    {
        this.m_GameOverCanvas.gameObject.SetActive(true);
        Text text = this.m_GameOverCanvas.GetComponentInChildren<Text>();
        text.text = string.Format("{0} Zombies Killed", this.m_CountOfZombiesKilled);
        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene("MainMenu");
    }

    private void M_GunScript_HitKillAllZombiesTargetEventHandler()
    {
        this.m_MobController.KillAllZombies();
    }

    public void IncreaseCountOfZombieKilled()
    {
        this.m_CountOfZombiesKilled++;
        this.m_ZombiesKilledCountText.text = string.Format("{0} Zombies Killed", this.m_CountOfZombiesKilled);

        checkAndImplementBonusesAndTargets();
    }

    private void checkAndImplementBonusesAndTargets()
    {
        int countOfAliveZombies = this.m_CountOfZombiesSpawn - this.m_CountOfZombiesKilled;

        if(this.m_CountOfZombiesKilled % 100 == 0 && this.m_CountOfZombiesKilled != 0)
        {
            this.m_PlayerScript.AddHealth();
        }

        if(countOfAliveZombies >= this.m_ZombiesParameterForCreateKillAllZombiesTarget)
        {
            this.m_TargetSpawnController.InstantiateNewTarget(TargetSpawnController.eTargetType.eKillAllZombies);
        }

        if(this.m_CountOfZombiesKilled % 10 == 0 && this.m_CountOfZombiesKilled != 0) 
        {
            this.m_TargetSpawnController.InstantiateNewTarget(TargetSpawnController.eTargetType.eMakeGunAuto);
        }

        if (this.m_CountOfZombiesKilled % 25 == 0 && this.m_CountOfZombiesKilled != 0)
        {
            this.m_TargetSpawnController.InstantiateNewTarget(TargetSpawnController.eTargetType.eDoubleDamage);
        }
    }
        

    public void IncreaseCountOfZombieSpawn()
    {
        this.m_CountOfZombiesSpawn++;
        checkAndImplementBonusesAndTargets();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void BackToTheMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
