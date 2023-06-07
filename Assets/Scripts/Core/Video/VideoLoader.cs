using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    VideoPlayer videoPlayer;

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
    }
}
