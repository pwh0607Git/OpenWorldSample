using UnityEngine;

public abstract class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance{
        get{ 
            if(_instance == null){
                _instance = FindFirstObjectByType<T>();

                if(_instance == null){
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();               //실행되면 확정
                }
            }

            return _instance;
        }
    }

    protected abstract bool IsDontDestroy();

    protected virtual void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        //한번 실행되면 씬이 바뀌어도 없어지지 않는가?
        if(IsDontDestroy()) 
            DontDestroyOnLoad(gameObject);
    }
}