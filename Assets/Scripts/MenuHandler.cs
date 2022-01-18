using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuHandler : MonoBehaviour
{
    //
    // Construction
    //
    private void Start()
    {
        UpdateScores();               
    }

    //
    // Buttons
    //
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ResetScores()
    {
        ScoreKeeper.Instance.ResetScores();
        UpdateScores();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    public List<TextMeshProUGUI> NameTexts;
    public List<TextMeshProUGUI> ScoreTexts;
    public List<TextMeshProUGUI> DateTexts;

    public void UpdateScores()
    {
        foreach (var t in NameTexts) t.text = "";
        foreach (var t in ScoreTexts) t.text = "";
        foreach (var t in DateTexts) t.text = "";

        //if (ScoreKeeper.Instance != null)
        {
            int i = 0;
            foreach (var score in ScoreKeeper.Instance.Scores)
            {
                NameTexts[i].text = score.name;
                ScoreTexts[i].text = score.score.ToString();
                DateTexts[i].text = score.date.ToString("d");
                i++;
            }
        }
    }
}
