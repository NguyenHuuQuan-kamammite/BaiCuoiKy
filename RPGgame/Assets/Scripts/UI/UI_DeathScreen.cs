using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void GoToCampBTN()
    {
        GameManager.instance.ChangeScene("Level_0", Respawn_Type.NoneSpecific);
    }

    public void GoToCheckpointBTN()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu", Respawn_Type.NoneSpecific);
    }
}
