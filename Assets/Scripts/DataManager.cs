using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager _instance;
    public static DataManager Instance => _instance;
    //---------------------------------------------------------------------------
    string _dataFolderPath = "Data/";
    string _stageFileName = "stage";
    string _menuFileName = "menu";
    string _ingredientFileName = "ingredient";
    //---------------------------------------------------------------------------
    List<SStage> _stages = new List<SStage>();
    List<SMenu> _menus = new List<SMenu>();
    List<SIngredient> _ingredients = new List<SIngredient>();
    //---------------------------------------------------------------------------
    public Sprite[] InGameImages;
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
        TextAsset jsonText = Resources.Load(path) as TextAsset;
        return JsonUtility.FromJson<JsonWrapper<T>>(jsonText.ToString()).items;
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
    //---------------------------------------------------------------------------
    public bool isSeletedStage { get; set; }
    public SStage SeletedStage { get; set; }
    public struct SResult
    {
        public int deliveredCount;
        public int deliveredProfit;

        public int failedCount;
        public int failedProfit;

        public int totalProfit;
    }
    public SResult ResultInfo = new SResult();
    //---------------------------------------------------------------------------
    public void SetResultInfo(int deliveredCount, int deliveredProfit, int failedCount, int failedProfit, int totalProfit)
    {
        ResultInfo.deliveredCount = deliveredCount;
        ResultInfo.deliveredProfit = deliveredProfit;

        ResultInfo.failedCount = failedCount;
        ResultInfo.failedProfit = failedProfit;

        ResultInfo.totalProfit = totalProfit;
    }
}
