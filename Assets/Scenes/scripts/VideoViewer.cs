using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class VideoViewer : MonoBehaviour
{

    public GameObject mainViewer;
    public GameObject prevViewer;
    public GameObject nextViewer;

    private VideoPlayer mainPlayer;
    private VideoPlayer prevPlayer;
    private VideoPlayer nextPlayer;

    private List<string> urls = new List<string>();
    public int offset = 0;
    private int currentVideo = 0;
    private int l = 0;


    // Start is called before the first frame update
    void Start()
    {
        loadVideos();
        l = urls.Count;
        currentVideo = clamp(offset);

        var tex = new RenderTexture(2048, 2048, 32);
        var video = new GameObject("videoPlayer_main");
        mainPlayer = video.AddComponent<VideoPlayer>();
        mainPlayer.renderMode = VideoRenderMode.RenderTexture;
        mainPlayer.targetTexture = tex;
        mainPlayer.source = VideoSource.Url;
        mainPlayer.isLooping = true;
        mainPlayer.SetDirectAudioMute(0,true);
        var rawImage = mainViewer.GetComponent<RawImage>();
        rawImage.texture = tex;


        tex = new RenderTexture(2048, 2048, 32);
        video = new GameObject("videoPlayer_prev");
        prevPlayer = video.AddComponent<VideoPlayer>();
        prevPlayer.renderMode = VideoRenderMode.RenderTexture;
        prevPlayer.targetTexture = tex;
        prevPlayer.source = VideoSource.Url;
        prevPlayer.isLooping = true;
        prevPlayer.SetDirectAudioMute(0,true);
        rawImage = prevViewer.GetComponent<RawImage>();
        rawImage.texture = tex;

        tex = new RenderTexture(2048, 2048, 32);
        video = new GameObject("videoPlayer_next");
        nextPlayer = video.AddComponent<VideoPlayer>();
        nextPlayer.renderMode = VideoRenderMode.RenderTexture;
        nextPlayer.targetTexture = tex;
        nextPlayer.source = VideoSource.Url;
        nextPlayer.isLooping = true;
        nextPlayer.SetDirectAudioMute(0,true);
        rawImage = nextViewer.GetComponent<RawImage>();
        rawImage.texture = tex;


        setVideo(mainPlayer, currentVideo);       
        setVideo(prevPlayer, currentVideo-1);     
        setVideo(nextPlayer, currentVideo+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void loadVideos(string extension = "mp4")
    {
        var path = Application.persistentDataPath + Path.DirectorySeparatorChar;
        var paths = Directory.EnumerateFiles(path, $"*.{extension}");

        foreach (var p in paths)
        {
            urls.Add(p);
            // var tex = new RenderTexture(2048, 2048, 32);
            // var video = new GameObject($"videoPlayer_{i}");
            // var player = video.AddComponent<VideoPlayer>();
            // player.renderMode = VideoRenderMode.RenderTexture;
            // player.targetTexture = tex;
            // player.url = p;
            // player.source = VideoSource.Url;
            // player.isLooping = true;
            // player.SetDirectAudioMute(0,true);
            // players.Add(player);

            // images.Add(tex);
            // ++i;
        }
    }


    private int clamp(int id)
    {
        int newId = id;
         if (newId < 0)
            newId = l - 1;
         if (newId > l - 1)
            newId = 0;
        return newId;
    }

    void setVideo(VideoPlayer player, int id)
    {
        id = clamp(id);
        player.url = urls[id];
    }

    public void nextVideo()
    {
        ++currentVideo;
        currentVideo = clamp(currentVideo);
        setVideo(mainPlayer, currentVideo);
        setVideo(prevPlayer, currentVideo-1);     
        setVideo(nextPlayer, currentVideo+1);
    }

    public void previousVideo()
    {
        --currentVideo;
        currentVideo = clamp(currentVideo);
        setVideo(mainPlayer, currentVideo);
        setVideo(prevPlayer, currentVideo-1);     
        setVideo(nextPlayer, currentVideo+1);
    }


}
