using System.Collections.Generic;
using UnityEngine;

public class WeaponSaveManager : MonoBehaviour
{
    private const string WEAPONS_SAVE_KEY = "WeaponsData";
    
    [System.Serializable]
    public class WeaponSaveData
    {
        public string weaponId;
        public int level;
    }
    
    [System.Serializable]
    public class WeaponsSaveData
    {
        public List<WeaponSaveData> weapons = new List<WeaponSaveData>();
        public bool isSessionActive = false;
    }

    public static WeaponSaveManager Instance { get; private set; }
    
    private WeaponsSaveData currentSessionData = new WeaponsSaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWeaponsState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveWeaponsState(List<Weapon> weapons)
    {
        currentSessionData.weapons.Clear();
        currentSessionData.isSessionActive = true;
        
        foreach (var weapon in weapons)
        {
            currentSessionData.weapons.Add(new WeaponSaveData
            {
                weaponId = weapon.Data.name,
                level = weapon.CurrentLevel
            });
        }
        
        string json = JsonUtility.ToJson(currentSessionData);
        PlayerPrefs.SetString(WEAPONS_SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadWeaponsState()
    {
        if (PlayerPrefs.HasKey(WEAPONS_SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(WEAPONS_SAVE_KEY);
            currentSessionData = JsonUtility.FromJson<WeaponsSaveData>(json);
        }
    }

    public void ApplySavedStateToWeapon(Weapon weapon)
    {
        if (currentSessionData.isSessionActive)
        {
            var savedWeapon = currentSessionData.weapons.Find(w => w.weaponId == weapon.Data.name);
            if (savedWeapon != null)
            {
                weapon.AddLevel(savedWeapon.level - weapon.CurrentLevel);
            }
        }
    }

    public void ResetSession()
    {
        currentSessionData.isSessionActive = false;
        currentSessionData.weapons.Clear();
        PlayerPrefs.DeleteKey(WEAPONS_SAVE_KEY);
        PlayerPrefs.Save();
    }

    // Вызывать при выходе в главное меню
    public void OnReturnToMainMenu()
    {
        ResetSession();
    }

    // Вызывать при завершении игры
    public void OnGameOver()
    {
        ResetSession();
    }
}