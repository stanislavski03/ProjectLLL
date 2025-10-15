using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // Если экземпляр ещё не создан
            if (_instance == null)
            {
                // Находим все ассеты данного типа в проекте
                T[] assets = Resources.LoadAll<T>("");

                // Если найдено несколько ассетов, выдаём предупреждение
                if (assets.Length > 1)
                {
                    Debug.LogError("Найдено несколько ассетов типа " + typeof(T).Name + ". Используется первый.");
                }

                // Устанавливаем первый найденный ассет в качестве единственного экземпляра
                _instance = assets.Length > 0 ? assets[0] : null;

                // Если не найдено ни одного ассета, выдаём предупреждение
                if (_instance == null)
                {
                    Debug.LogError("Не удалось найти ассет типа " + typeof(T).Name + " в папке Resources.");
                }
            }
            return _instance;
        }
    }
}
