using UnityEngine;
using UnityEngine.SceneManagement;
using VTemplate.UI;

public class MainMenu : MonoBehaviour
{
    public ButtonUI startBtn;
    public int sceneToLoad;
    public void Init()
    {
        startBtn.Init(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });
    }
    private void Start()
    {
        Init();
    }

}
