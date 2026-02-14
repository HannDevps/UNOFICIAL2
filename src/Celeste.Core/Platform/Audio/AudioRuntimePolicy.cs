using System;

namespace Celeste.Core.Platform.Audio;

public static class AudioRuntimePolicy
{
    public const string EnableFmodOnAndroidSwitch = "Celeste.Android.EnableFmodAudio";

    public static bool IsFmodEnabledOnAndroid()
    {
        if (!OperatingSystem.IsAndroid())
        {
            return true;
        }

        if (AppContext.TryGetSwitch(EnableFmodOnAndroidSwitch, out var enabled))
        {
            return enabled;
        }

        return true;
    }

    public static bool ShouldForceSilentAudio()
    {
        return OperatingSystem.IsAndroid() && !IsFmodEnabledOnAndroid();
    }
}
