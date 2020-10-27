using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HighScoreController
    {
        private readonly List<(int, DateTime)> r_Scores;
        private const int k_MaxCapacity = 10;
        private const string k_KeyScorePrefix = "HighScoreNumber";
        private const string k_KeyDatePrefix = "HighScoreDateNumber";

        public HighScoreController()
        {
            this.r_Scores = new List<(int, DateTime)>();
            Load();
        }

        public void AddScore(int i_Score)
        {
            this.r_Scores.Add((i_Score, DateTime.Now));
            this.r_Scores.Sort();
            this.r_Scores.Reverse();

            if (this.r_Scores.Count > k_MaxCapacity)
            {
                this.r_Scores.RemoveAt(this.r_Scores.Count - 1);
            }

            save();
        }

        public void Load()
        {
            this.r_Scores.Clear();

            for (int i = 0; i < k_MaxCapacity; i++)
            {
                if (PlayerPrefs.HasKey(k_KeyScorePrefix + i.ToString()) && PlayerPrefs.HasKey(k_KeyDatePrefix + i.ToString()))
                {
                    int score = PlayerPrefs.GetInt(k_KeyScorePrefix + i.ToString());
                    string date = PlayerPrefs.GetString(k_KeyDatePrefix + i.ToString());

                    this.r_Scores.Add((score, DateTime.Parse(date)));
                }
            }
        }

        private void save()
        {
            CleanMem();

            for (int i = 0; i < k_MaxCapacity && i < this.r_Scores.Count ; i++)
            {
                PlayerPrefs.SetInt(k_KeyScorePrefix + i.ToString(), this.r_Scores[i].Item1);
                PlayerPrefs.SetString(k_KeyDatePrefix + i.ToString(), this.r_Scores[i].Item2.ToString());
            }
        }

        public void CleanMem()
        {
            for (int i = 0; i < k_MaxCapacity; i++)
            {
                if (PlayerPrefs.HasKey(k_KeyScorePrefix + i.ToString()))
                {
                    PlayerPrefs.DeleteKey(k_KeyScorePrefix + i.ToString());
                }

                if (PlayerPrefs.HasKey(k_KeyDatePrefix + i.ToString()))
                {
                    PlayerPrefs.DeleteKey(k_KeyDatePrefix + i.ToString());
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            int i = 1;

            if(this.r_Scores.Count != 0)
            {
                foreach((int, DateTime) score in this.r_Scores)
                {
                    if(score.Item2 != DateTime.MinValue)
                    {
                        result.AppendLine(string.Format("{0}. Zombies Killed : {1} \t Recorded At : {2}", i, score.Item1, score.Item2.ToString("dd/MM/yyyy")));
                    }

                    i++;
                }
            }
            else
            {
                result.AppendLine("No Highscore Records Have Been Saved Yet");
            }

            return result.ToString();
        }
    }
}
