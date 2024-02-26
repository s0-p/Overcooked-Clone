using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager _instance;
    public static DataManager Instance => _instance;
    //---------------------------------------------------------------------------
    string _dataFolderPath = Application.dataPath + "/Data/";
    string _ingredientFileName = "ingredient.json";
    string _menuFileName = "menu.json";
    string _stageFileName = "stage.json";
    //---------------------------------------------------------------------------
    private void Awake()
    {
        if(_instance == null) _instance = new DataManager();
        DontDestroyOnLoad(gameObject);
    }
    public T[] LoadData<T>(string fileName)
    {
        string path = Path.Combine(_dataFolderPath, fileName);
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            return JsonUtility.FromJson<JsonWrapper<T>>(jsonString).items;
        }

        return null;
    }
    public SStage GetStage(int chapterValue, int stageValue)
    {
        List<SStage> stages = new List<SStage>();
        stages.AddRange(LoadData<SStage>(_stageFileName));
        return stages.Find(i => i.chapter == chapterValue && i.stage == stageValue); 
    }
    public SMenu[] GetMenus(int menusBit) 
    {
        List<SMenu> menus = new List<SMenu>();
        menus.AddRange(LoadData<SMenu>(_menuFileName));
        return menus.FindAll(i => (i.bitId & menusBit) > 0).ToArray();
    }
}
