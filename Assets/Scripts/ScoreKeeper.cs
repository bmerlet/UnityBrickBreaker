using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreKeeper : MonoBehaviour
{
    // Singleton
    public static ScoreKeeper Instance { get; private set; }

    // Get top score
    public Score TopScore => scores.Count == 0 ? null : scores[0];

    // Get scores
    public IEnumerable<Score> Scores => scores;

    // Get notified when scores change
    // ZZZ Later public UnityEvent OnScoreChange; 

    // Save path
    private string savePath;

    // Best scores
    private readonly List<Score> scores = new List<Score>();

    // Constructor: Make a singleton of this instance
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "Benoit.json");
        LoadScores();
    }

    public void SaveScores()
    {
        var ss = new SerialScores() { Scores = new SerialScore[scores.Count] };
        for(int i = 0; i < scores.Count; i++)
        {
            var serialScore = new SerialScore();
            serialScore.name = scores[i].name;
            serialScore.score = scores[i].score;
            serialScore.binaryDate = scores[i].date.ToBinary();
            ss.Scores[i] = serialScore;
        }
        File.WriteAllText(savePath, JsonUtility.ToJson(ss));
    }

    private void LoadScores()
    {
        scores.Clear();
        if (File.Exists(savePath))
        {
            var ss = JsonUtility.FromJson<SerialScores>(File.ReadAllText(savePath));
            foreach(var serialScore in ss.Scores)
            {
                var score = new Score();
                score.name = serialScore.name;
                score.score = serialScore.score;
                score.date = DateTime.FromBinary(serialScore.binaryDate);
                scores.Add(score);
            }
        }
    }

    public void ResetScores()
    {
        scores.Clear();
    }

    public bool IsHighScore(int score)
    {
        return scores.Count == 0 || score > scores[0].score;
    }

    public void AddHighScore(string name, int score)
    {
        var newScore = new Score() { name = name, score = score, date = DateTime.Today };
        scores.Insert(0, newScore);
        if (scores.Count > 5)
        {
            scores.RemoveAt(5);
        }
        SaveScores();
    }

    //
    // Class to store a score, both at runtime and when serialized
    //
    public class Score
    {
        public string name;
        public int score;
        public DateTime date;
    }

    //
    // Scores, as saved
    //
    [Serializable]
    class SerialScores
    {
        public SerialScore[] Scores;
    }

    [Serializable]
    class SerialScore
    {
        public string name;
        public int score;
        public long binaryDate;
    }
}
