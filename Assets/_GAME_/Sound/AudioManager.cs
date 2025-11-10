using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;

    [Header("Background Music List")]
    [SerializeField] private AudioClip[] backgroundTracks; // danh sách nhạc nền

    [Header("UI Slider (optional)")]
    [SerializeField] private Slider volumeSlider;

    private int currentTrackIndex = 0;
    private bool isMusicLooping = true;

    private void Awake()
    {
        // Đảm bảo chỉ có 1 instance tồn tại
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tự tạo AudioSource nếu chưa có
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = false;       // để tự quản lý chuyển bài
        musicSource.playOnAwake = false;
    }

    private void Start()
    {
        // Phát bài đầu tiên nếu có danh sách
        if (backgroundTracks.Length > 0)
        {
            PlayTrack(currentTrackIndex);
            StartCoroutine(TrackWatcher());
        }

        // Nếu có slider volume
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(volumeSlider.value);
        }
    }

    private IEnumerator TrackWatcher()
    {
        // Theo dõi trạng thái bài hát, tự chuyển khi hết
        while (isMusicLooping)
        {
            if (!musicSource.isPlaying && backgroundTracks.Length > 0)
            {
                NextTrack();
            }
            yield return null;
        }
    }

    public void PlayTrack(int index)
    {
        if (backgroundTracks.Length == 0) return;
        if (index < 0 || index >= backgroundTracks.Length) index = 0;

        currentTrackIndex = index;
        musicSource.clip = backgroundTracks[index];
        musicSource.Play();
    }

    public void NextTrack()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= backgroundTracks.Length)
            currentTrackIndex = 0;

        PlayTrack(currentTrackIndex);
    }

    public void PreviousTrack()
    {
        currentTrackIndex--;
        if (currentTrackIndex < 0)
            currentTrackIndex = backgroundTracks.Length - 1;

        PlayTrack(currentTrackIndex);
    }

    public void StopMusic() => musicSource.Stop();
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }
}

