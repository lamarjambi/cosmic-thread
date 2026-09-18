using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VolumeSettings : MonoBehaviour
{
    public const string VolumeKey = "MasterVolume";
    public const float MinDb = -80f;
    public const float MaxDb = 0f;

    public static VolumeSettings Instance { get; private set; }

    public AudioMixer mainMixer;

    [Tooltip("Leave empty to use the mixer's 'Master' group.")]
    public AudioMixerGroup targetGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (targetGroup == null && mainMixer != null)
        {
            AudioMixerGroup[] groups = mainMixer.FindMatchingGroups("Master");
            if (groups.Length > 0)
                targetGroup = groups[0];
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        Apply(SavedVolume);
        RouteAll();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RouteAll();
    }

    public static float SavedVolume => PlayerPrefs.GetFloat(VolumeKey, MaxDb);
    public static void Route(AudioSource source)
    {
        if (source == null || Instance == null || Instance.targetGroup == null)
            return;

        if (source.outputAudioMixerGroup == null)
            source.outputAudioMixerGroup = Instance.targetGroup;
    }

    public void RouteAll()
    {
        if (targetGroup == null)
            return;

        AudioSource[] sources = FindObjectsByType<AudioSource>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (AudioSource source in sources)
        {
            if (source.outputAudioMixerGroup == null)
                source.outputAudioMixerGroup = targetGroup;
        }
    }

    public void Apply(float volumeDb)
    {
        if (mainMixer != null)
            mainMixer.SetFloat("volume", Mathf.Clamp(volumeDb, MinDb, MaxDb));
    }

    public void Save(float volumeDb)
    {
        Apply(volumeDb);
        PlayerPrefs.SetFloat(VolumeKey, volumeDb);
        PlayerPrefs.Save();
    }
}
