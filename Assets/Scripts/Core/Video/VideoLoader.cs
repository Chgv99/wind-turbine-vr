using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    VideoPlayer videoPlayer;

    [SerializeField] float ratio = 16 / 9f;
    [SerializeField] string url = "";

    public string Url { get => url; set => url = value; }

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.url = url;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Prepare();
        
        RectTransform rt = GetComponent<Canvas>().GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.rect.width, rt.rect.width / ratio);
    }
}
