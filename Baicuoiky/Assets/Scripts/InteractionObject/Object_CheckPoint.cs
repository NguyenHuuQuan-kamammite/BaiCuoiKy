using UnityEngine;

public class Object_CheckPoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;
    public bool isActive { get; private set; }

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public string GetCheckpointId() => checkpointId;
    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;
    public void ActivateCheckpoint(bool activate)
    {

        isActive = activate;
        if (anim != null)
            anim.SetBool("isActive", activate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.instance.SaveGame();
            ActivateCheckpoint(true);
            GameManager.instance.SetLastPlayerPosition(GetPosition());
        }
    }  

    public void LoadData(GameData data)
    {
        if (data.unlockedCheckpoints.TryGetValue(checkpointId, out bool isUnlocked))
            ActivateCheckpoint(isUnlocked);
        else
            ActivateCheckpoint(false);
    }

    public void SaveData(ref GameData data)
    {
        if (isActive == false)
            return;

        if (data.unlockedCheckpoints.ContainsKey(checkpointId) == false)
            data.unlockedCheckpoints.Add(checkpointId, true);
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
