using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class VideoEndHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public UnityEvent onVideoEnd;

    void Start()
    {
        if (videoPlayer == null) 
            videoPlayer = GetComponent<VideoPlayer>();

        // Subscribe to the end event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished playing.");
        onVideoEnd?.Invoke();
    }

    void OnDisable()
    {
        // Clean up subscription
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
