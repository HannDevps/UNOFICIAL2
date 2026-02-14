using System;
using System.Diagnostics;

namespace Celeste.Core.Platform.Interop;

public static class CelesteExternalLinkBridge
{
    private static Func<string, bool>? _openLinkHandler;

    public static void Configure(Func<string, bool> openLinkHandler)
    {
        _openLinkHandler = openLinkHandler;
    }

    public static void Clear()
    {
        _openLinkHandler = null;
    }

    public static bool TryOpen(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return false;
        }

        string normalizedUrl = uri.ToString();
        if (_openLinkHandler != null)
        {
            try
            {
                if (_openLinkHandler(normalizedUrl))
                {
                    return true;
                }
            }
            catch
            {
            }
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = normalizedUrl,
                UseShellExecute = true
            });
            return true;
        }
        catch
        {
            return false;
        }
    }
}
