using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class SaveData
{
    public string prefabPath;
    public Vector3 position;
    public Quaternion rotation;
}
public class SaveSystem : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public void Save()
    {
        List<SaveData> data = new List<SaveData>();

        foreach (var saveable in SaveRegistry.GetAll())
        {
            if (saveable == null) continue;

            SaveData d = new SaveData
            {
                prefabPath = saveable.prefabPath,
                position = saveable.transform.position,
                rotation = saveable.transform.rotation
            };

            data.Add(d);
        }

        string json = JsonUtility.ToJson(new Wrapper { list = data }, true);
        File.WriteAllText(SavePath, json);
    }
    public void Load()
    {
        if (!File.Exists(SavePath))
            return;

        string json = File.ReadAllText(SavePath);
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);

        // Clear existing objects
        foreach (var s in new List<Saveable>(SaveRegistry.GetAll()))
        {
            if (s != null)
                Destroy(s.gameObject);
        }

        SaveRegistry.Clear();

        // Recreate
        foreach (var data in wrapper.list)
        {
            GameObject prefab = Resources.Load<GameObject>(data.prefabPath);
            if (prefab == null)
            {
                Debug.LogWarning($"Missing prefab at {data.prefabPath}");
                continue;
            }

            GameObject obj = Instantiate(prefab, data.position, data.rotation);

            Saveable saveable = obj.GetComponent<Saveable>();
            if (saveable == null)
                saveable = obj.AddComponent<Saveable>();

            saveable.prefabPath = data.prefabPath;
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<SaveData> list;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            Save();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            Load();
        }
    }
}