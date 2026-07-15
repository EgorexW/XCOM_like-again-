using UnityEngine;

public static class SaveProfile{
    const string PlayerPrefsKey = "SaveProfileIndex";
    public const int DefaultProfile = 1;

    public static int CurrentProfile{
        get => PlayerPrefs.GetInt(PlayerPrefsKey, DefaultProfile);
        set => PlayerPrefs.SetInt(PlayerPrefsKey, value);
    }
}
