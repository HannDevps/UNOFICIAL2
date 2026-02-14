using System;

namespace Celeste.Core.Platform.Audio;

public static class AudioRuntimePolicy
{
    public const string EnableFmodOnAndroidSwitch = "Celeste.Android.EnableFmodAudio";

    private static readonly object AndroidHintSync = new();
    private static bool _androidHintsConfigured;
    private static int _androidOutputSampleRate;
    private static int _androidOutputBlockSize;
    private static bool _androidSupportsLowLatency;
    private static bool _androidBluetoothOn;
    private static bool _androidJavaBridgeReady;

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

    public static void ConfigureAndroidDeviceAudioHints(int outputSampleRate, int outputBlockSize, bool supportsLowLatency, bool bluetoothOn, bool javaBridgeReady)
    {
        if (!OperatingSystem.IsAndroid())
        {
            return;
        }

        lock (AndroidHintSync)
        {
            _androidOutputSampleRate = Math.Max(0, outputSampleRate);
            _androidOutputBlockSize = Math.Max(0, outputBlockSize);
            _androidSupportsLowLatency = supportsLowLatency;
            _androidBluetoothOn = bluetoothOn;
            _androidJavaBridgeReady = javaBridgeReady;
            _androidHintsConfigured = true;
        }
    }

    public static bool TryGetAndroidDeviceAudioHints(out int outputSampleRate, out int outputBlockSize, out bool supportsLowLatency, out bool bluetoothOn, out bool javaBridgeReady)
    {
        lock (AndroidHintSync)
        {
            outputSampleRate = _androidOutputSampleRate;
            outputBlockSize = _androidOutputBlockSize;
            supportsLowLatency = _androidSupportsLowLatency;
            bluetoothOn = _androidBluetoothOn;
            javaBridgeReady = _androidJavaBridgeReady;
            return _androidHintsConfigured;
        }
    }
}
