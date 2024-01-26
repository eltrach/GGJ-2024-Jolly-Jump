namespace VTemplate
{
    public delegate void ScoreEvent(int score);


    public interface IScore
    {
        /// <summary>
        /// Event that you can subscribe to, it's called every time the score has changed, giving the new value as parameter
        /// </summary>
        event ScoreEvent OnScoreChanged;
        /// <summary>
        /// Event that you can subscribe to, it's called every time the highscore has changed, giving the new value as parameter
        /// </summary>
        event ScoreEvent OnHighscoreChanged;

        /// <summary>
        /// Represents the name that you can use to display the score, it is also used as key with the IDataManager, be careful to use different currency names if you use multiple currencies
        /// </summary>
        string ScoreName { get; }
        /// <summary>
        /// Represents the current score
        /// </summary>
        int Score { get; }
        /// <summary>
        /// Represents the current highscore
        /// </summary>
        int Highscore { get; }

        /// <summary>
        /// Used to setup the score, it must be called before using the score
        /// You can give it a IDataManager to store the Score and Highscore inside with the ScoreName as key
        /// </summary>
        void Init(IDataManager dataManager = null);
        /// <summary>
        /// Used to set the score to a new value, also sets the highscore if score > highscore
        /// </summary>
        /// <param name="score">New score</param>
        void SetScore(int score);
        /// <summary>
        /// Used to increase the score by a value, also sets the highscore if score > highscore
        /// </summary>
        /// <param name="scoreGain">Score to gain</param>
        void IncreaseScore(int scoreGain);
        /// <summary>
        /// Used to reset the current score value
        /// </summary>
        void ResetScore();
        /// <summary>
        /// Used to reset the score and highscore values
        /// </summary>
        void Reset();
    }
}