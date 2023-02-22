using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private const float k_timeBetweenRotations = 30.0f;

    [SerializeField]
    private MainConfig m_Config;

    private int m_CurrentSceneIndex = 0;

    private float m_timeLastPrint = 0f;
    private const float k_updateInterval = 2f;

    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        StartCoroutine(RotateScenes());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime - m_timeLastPrint > k_updateInterval)
        {
            m_timeLastPrint = Time.unscaledTime;
            PrintStats();
        }
    }

    public void PrintStats()
    {
        Debug.Log(" FPS: " + (int)(1.0f / Time.smoothDeltaTime));
    }

    IEnumerator RotateScenes()
    {
        while (true)
        {
            var sceneName = m_Config.scenesToLoad[m_CurrentSceneIndex++];
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            Debug.Log($"Chargement de la scene: {sceneName}");
            if (m_CurrentSceneIndex == m_Config.scenesToLoad.Count)
            {
                m_CurrentSceneIndex = 0;
            }
            yield return new WaitForSeconds(k_timeBetweenRotations);
        }
    }
}
