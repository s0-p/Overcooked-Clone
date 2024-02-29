using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager _instance;
    public static DataManager Instance => _instance;
    //---------------------------------------------------------------------------
    string _dataFolderPath = Application.dataPath + "/Data/";
    string _stageFileName = "stage.json";
    string _menuFileName = "menu.json";
    string _ingredientFileName = "ingredient.json";
    //---------------------------------------------------------------------------
    List<SStage> _stages = new List<SStage>();
    List<SMenu> _menus = new List<SMenu>();
    List<SIngredient> _ingredients = new List<SIngredient>();
    //---------------------------------------------------------------------------
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _stages.AddRange(LoadData<SStage>(_stageFileName));
        _menus.AddRange(LoadData<SMenu>(_menuFileName));
        _ingredients.AddRange(LoadData<SIngredient>(_ingredientFileName));
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
        return _stages.Find(s => s.chapter == chapterValue && s.stage == stageValue); 
    }
    public SMenu[] GetMenus(int menusBit) 
    {
        return _menus.FindAll(m => (m.bitId & menusBit) > 0).ToArray();
    }
    public SIngredient GetIngredient(int bitId)
    {
        return _ingredients.Find(i => i.bitId == bitId);
    }
}
