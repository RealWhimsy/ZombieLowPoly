using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WebGlPlayVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = System.IO.Path.Combine(Const.VideoAssetsPath, "pexels.mp4");
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }
}
