using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private int Coins
    {
        get { return PlayerPrefs.GetInt("coins", 0); }
        set
        {
            PlayerPrefs.SetInt("coins", value);
            PlayerPrefs.Save();
        }
    }

    public int GetCoins() => Coins;

    public void SetCoins(int coinsToSet)
    {
        Coins = coinsToSet;
        PlayerPrefs.Save();
    }

    public void AddCoins(int coinsToAdd)
    {
        Coins += coinsToAdd;
        PlayerPrefs.Save();
    }
    public void ResetCoins()
    {
        Coins = 0;
        PlayerPrefs.Save();
    }
}
