# Score

## Description

The **Score** is a simple score system that you can use. You can create as many scores as you want, all contains their own Highscore. \
The interface used is **IScore**, the C# implementation is **ScoreImpl**.

## Usage

### ScoreImpl

ScoreImpl is a native C# class implementing IScore, managed only via code. You can create a score using (you must give your score a name) :

```
IScore score = new ScoreImpl("MyScore");
```

### IScore

Once your score is created, you need to Init() before using it :

```plaintext
score.Init();
```

If you want to store the score, you can pass him a IDataManager in Init(), it will only be in RAM without an IDataManager, in disk with IDataManager :

```plaintext
IDataManager dataManager;
score.Init(dataManager);
```

Then, there are a few methods that you can use :

```
event ScoreEvent OnScoreChanged;
event ScoreEvent OnHighscoreChanged;
        
string ScoreName { get; }
int Score { get; }
int Highscore { get; }

void Init(IDataManager dataManager = null);
void SetScore(int score);
void IncreaseScore(int scoreGain);
void ResetScore();
void Reset();
```

* **OnScoreChanged** is an event that you can subscribe to, it's called every time the score has changed, giving the new value as parameter
* **OnHighscoreChanged** is an event that you can subscribe to, it's called every time the highscore has changed, giving the new value as parameter
* **ScoreName** represents the name that you can use to display the score, it is also used as key with the IDataManager, be careful to use different currency names if you use multiple currencies
* **Score** represents the current score
* **Highscore** represents the current highscore
* **Init** is used to setup the score, it must be called before using the score\
  You can give it a IDataManager to store the Score and Highscore inside with the ScoreName as key
* **SetScore** is used to set the score to a new value, also sets the highscore if score > highscore
* **IncreaseScore** is used to increase the score by a value, also sets the highscore if score > highscore
* **ResetScore** is used to reset the current score value
* **Reset** is used to reset the score and highscore values