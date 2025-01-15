using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Components")]
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;
    public GameObject agroBot;
    public TextMeshProUGUI speechBubbleText;
    
    [Header("Video Settings")]
    public string videoPath;
    
    private bool isPlaying = false;
    private bool isWaitingForTap = false;
    
    void Start()
    {
        SetupVideo();
        InitializeUI();
    }
    
    private void SetupVideo()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.VideoClip;
        VideoClip clip = Resources.Load<VideoClip>(videoPath);
        if (clip != null)
        {
            videoPlayer.clip = clip;
        }
        else
        {
            Debug.LogError("Failed to load video clip from path: " + videoPath);
        }
        
        videoPlayer.loopPointReached += OnVideoEnd;

        if (videoDisplay.gameObject.GetComponent<Button>() == null)
        {
            Button button = videoDisplay.gameObject.AddComponent<Button>();
            button.onClick.AddListener(OnScreenTap);
        }
    }

    private void InitializeUI()
    {
        speechBubbleText.text = "This tutorial will show how to load fertilizer.\nTap to play";
    }
    
    private void Update()
    {
        if (isPlaying && !isWaitingForTap)
        {
            CheckVideoProgress();
        }
    }
    
    private void CheckVideoProgress()
    {
        if (!videoPlayer.isPaused)
        {
            if (videoPlayer.time >= 6f && videoPlayer.time < 6.1f)
            {
                PauseVideoWithMessage("Tap to open the Fertilizer Tray");
                isWaitingForTap = true;
            }
            else if (videoPlayer.time >= 10f && videoPlayer.time < 10.1f)
            {
                PauseVideoWithMessage("Tap to add the fertilizer");
                isWaitingForTap = true;
            }
        }
    }

    private void PauseVideoWithMessage(string message)
    {
        videoPlayer.Pause();
        speechBubbleText.text = message;
    }
    
    private void OnVideoEnd(VideoPlayer source)
    {
        speechBubbleText.text = "Great Job! \nReward: 200 Points";
        isPlaying = false;
        isWaitingForTap = false;
        StartCoroutine(LoadSceneAfterDelay(3f));
    }
    
    public void OnScreenTap()
    {
        if (!isPlaying)
        {
            videoPlayer.Play();
            isPlaying = true;
            speechBubbleText.text = "";
        }
        else if (videoPlayer.isPaused && isWaitingForTap)
        {
            videoPlayer.Play();
            speechBubbleText.text = "";
            isWaitingForTap = false;
            StartCoroutine(ResumeDelay());
        }
    }

    private IEnumerator ResumeDelay()
    {
        yield return null; // Wait for the next frame

        if (videoPlayer.time >= 6f && videoPlayer.time < 6.2f)
        {
            videoPlayer.time = 6.2f;
        }
        else if (videoPlayer.time >= 10f && videoPlayer.time < 10.2f)
        {
            videoPlayer.time = 10.2f;
        }
    }


    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.GiveUserPointsAfterTutorial.Invoke(200);
        EventManager.CloseVideoTutorial.Invoke();
        Destroy(gameObject);
    }
}
