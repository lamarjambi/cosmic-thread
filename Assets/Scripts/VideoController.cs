using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.source = VideoSource.Url;
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "CT_background.mp4");
        vp.Play();
    }
}