using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSocialLink : MonoBehaviour
{
    public void OpenSteamPage()
    {
        Application.OpenURL("https://store.steampowered.com/app/1949310/Eternals_Path/");
    }
    public void OpenDiscordPage()
    {
        Application.OpenURL("https://discord.gg/YvBYf74Paj");
    }
    public void OpenLinktreePage()
    {
        Application.OpenURL("https://linktr.ee/sleeplessparadise");
    }
    public void OpenKickstarterPage()
    {
        Application.OpenURL("https://www.kickstarter.com/projects/sleeplessparadise/eternals-path");
    }
}
