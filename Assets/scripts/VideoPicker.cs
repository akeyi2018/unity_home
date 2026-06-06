using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;  // ← これを追加

public class VideoFilePicker : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public TMP_InputField pathInput;  // ← InputField → TMP_InputField に変更

    public void PlayVideo()
    {
        string path = pathInput.text.Trim();
        if (string.IsNullOrEmpty(path)) return;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;
        videoPlayer.Play();
    }
}