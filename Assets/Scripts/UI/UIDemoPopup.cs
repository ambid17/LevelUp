using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDemoPopup : MonoBehaviour
{
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button discordButton;
    [SerializeField]
    private Button youtubeButton;
    [SerializeField]
    private Button twitchButton;
    private void Awake()
    {
        backButton.onClick.AddListener(Close);
        discordButton.onClick.AddListener(Discord);
        youtubeButton.onClick.AddListener(YouTube);
        twitchButton.onClick.AddListener(Twitch);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void Discord()
    {
        Application.OpenURL("https://discord.gg/ygzuXGMj9B");
    }

    private void YouTube()
    {
        Application.OpenURL("https://www.youtube.com/@StudioDreadLLC");
    }

    private void Twitch()
    {
        Application.OpenURL("https://www.twitch.tv/studiodread");
    }
}
