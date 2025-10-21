using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();

        Audio_Manager.instance.StartBGM("playlist_mainMenu");
    }

    public void PlayBTN()
    {
       Audio_Manager.instance.PlayGlobalSFX("button_click");
       GameManager.instance.ContinuePlay();
    }


    public void QuitGameBTN()
    {
        Application.Quit();
    }
}
