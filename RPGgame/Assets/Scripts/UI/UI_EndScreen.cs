using UnityEngine;

public class UI_EndScreen : MonoBehaviour
{

    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();

        Audio_Manager.instance.StartBGM("playlist_mainMenu");
    }
    public void GoToMainMenuBTN()
    {
        Audio_Manager.instance.PlayGlobalSFX("button_click");
        GameManager.instance.LoadScene("MainMenu");
    }
    public void QuitGameBTN()
    {
        Audio_Manager.instance.PlayGlobalSFX("button_click");
        Application.Quit();
    }
}

