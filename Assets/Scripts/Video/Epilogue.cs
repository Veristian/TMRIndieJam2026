using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Epilogue : MonoBehaviour
{
    private VideoPlayer video;
    [SerializeField] private float delay = 1f;
    bool isFinished = false;
    bool isLoading = false;
    private void Awake()
    {
        if (video == null) video = GetComponentInChildren<VideoPlayer>();
        video.loopPointReached += EnableRestart;
    }
    private void Start()
    {
        StartCoroutine(PlayAfterDelay());
    }

    IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(delay);
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
