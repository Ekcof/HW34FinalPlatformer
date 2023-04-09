using UnityEngine;
using System.IO;


public class PlayerDataStore : MonoBehaviour
{
    [SerializeField] private Health health;
    private string _filePath;

    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/dataStorage.csv";
        Debug.Log("Button clicked!");
    }


    [ContextMenu("Save Data")]
    public void SaveData()
    {
        string str = $"{health.DataHealth},{transform.position}";

        File.WriteAllText(_filePath, str);

        Debug.Log($"CSV file saved to {_filePath}");
    }


}
