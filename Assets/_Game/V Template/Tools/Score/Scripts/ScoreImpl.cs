using System;


namespace VTemplate
{
    public class ScoreImpl : IScore
    {
        public event ScoreEvent OnScoreChanged;
        public event ScoreEvent OnHighscoreChanged;

        public string ScoreName { get; }
        public int Score { get; private set; }
        public int Highscore { get; private set; }

        private string ScoreKey => $"Score_{ScoreName}_score";
        private string HighscoreKey => $"_Score_{ScoreName}_highscore";

        private IDataManager _dataManager;

        public ScoreImpl(string scoreName)
        {
            ScoreName = scoreName;
        }
        public void Init(IDataManager dataManager = null)
        {
            Precondition.CheckNotNull(ScoreName);
            _dataManager = dataManager;
            Score = GetScoreFromDataManager(ScoreKey);
            Highscore = GetScoreFromDataManager(HighscoreKey);
        }
        public void SetScore(int score)
        {
            Score = score;
            _dataManager?.SetData(ScoreKey, Score);
            OnScoreChanged?.Invoke(Score);
            if (Score > Highscore)
                SetHighscore(Score);
        }
        public void IncreaseScore(int scoreGain)
        {
            SetScore(Score + scoreGain);
        }
        public void ResetScore()
        {
            SetScore(0);
        }
        public void Reset()
        {
            SetScore(0);
            SetHighscore(0);
        }

        private int GetScoreFromDataManager(string key)
        {
            if (_dataManager == null)
                return 0;
            return _dataManager.HasData(key) ? Convert.ToInt32(_dataManager.GetData(key)) : 0;
        }

        private void SetHighscore(int score)
        {
            Highscore = score;
            _dataManager?.SetData(HighscoreKey, Highscore);
            OnHighscoreChanged?.Invoke(Highscore);
        }
    }
}