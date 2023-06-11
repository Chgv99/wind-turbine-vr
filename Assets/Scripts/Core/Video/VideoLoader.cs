using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    VideoPlayer videoPlayer;

    [SerializeField] float ratio = 16 / 9f;
    //[SerializeField] string url = "";

    //public string Url { get => url; set => url = value; }

    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        //videoPlayer.url = url;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.playOnAwake = false;

        rt = GetComponent<Canvas>().GetComponent<RectTransform>();
    }

    public void Play(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Prepare();
        videoPlayer.Play();
    }

    //void OnEnable() => videoPlayer.Play();

    //void On??() => videoPlayer.Pause(); //Issue #25: Consider this option when closing modal

    void OnDisable() => videoPlayer.Stop();

    void Update()
    {
        rt.sizeDelta = new Vector2(rt.rect.width, rt.rect.width / ratio);
    }
}
