using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] private string musicGroupName;

    private void Start()
    {
        Audio_Manager.instance.StartBGM(musicGroupName);
    }
}
