using UnityEngine;

public class Saveable : MonoBehaviour
{
    [Header("Should start after Resources folder")]
    public string prefabPath; // Resources path

    private void OnEnable()
    {
        SaveRegistry.Register(this);
    }

    private void OnDisable()
    {
        SaveRegistry.Unregister(this);
    }
}