using UnityEngine;
using UnityEngine.Video;
public class Epilogue : MonoBehaviour
{
    private VideoPlayer video;
    bool isFinished = false;
    bool isLoading = false;
    private void Awake()
    {
        if (video == null) video = GetComponentInChildren<VideoPlayer>();
        video.loopPointReached += EnableRestart;
        video.Play();
    }

    private void EnableRestart(VideoPlayer vp)
    {
        Debug.Log("Video Finished");
        isFinished = true;
    }

    private void Update()
    {
        if (isFinished && InputManager.anyWasPressedThisFrame && !isLoading)
        {
            isLoading = true;
            SceneManager.Instance.LoadScene("StartScene");
        }
    }
}
