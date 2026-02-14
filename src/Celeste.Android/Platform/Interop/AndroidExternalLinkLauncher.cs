using System;
using Android.App;
using Android.Content;
using AndroidUri = Android.Net.Uri;
using Celeste.Android.Platform.Logging;
using Celeste.Core.Platform.Logging;

namespace Celeste.Android.Platform.Interop;

public sealed class AndroidExternalLinkLauncher
{
    private const string DiscordPackageName = "com.discord";

    private readonly Activity _activity;
    private readonly AndroidDualLogger _logger;

    public AndroidExternalLinkLauncher(Activity activity, AndroidDualLogger logger)
    {
        _activity = activity;
        _logger = logger;
    }

    public bool TryOpenUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out Uri? parsedUrl))
        {
            _logger.Log(LogLevel.Warn, "LINK", "Rejected invalid URL", context: url);
            return false;
        }

        string normalizedUrl = parsedUrl.ToString();
        if (IsDiscordInvite(parsedUrl) && TryOpenWithPackage(normalizedUrl, DiscordPackageName, "discord_app"))
        {
            return true;
        }

        return TryOpenWithPackage(normalizedUrl, packageName: null, "browser");
    }

    private bool TryOpenWithPackage(string url, string? packageName, string target)
    {
        try
        {
            Intent intent = new(Intent.ActionView, AndroidUri.Parse(url));
            intent.AddFlags(ActivityFlags.NewTask);

            if (!string.IsNullOrWhiteSpace(packageName))
            {
                intent.SetPackage(packageName);
            }

            if (intent.ResolveActivity(_activity.PackageManager) == null)
            {
                _logger.Log(LogLevel.Warn, "LINK", "No activity available for URL", context: $"target={target}; url={url}");
                return false;
            }

            _activity.StartActivity(intent);
            _logger.Log(LogLevel.Info, "LINK", "Opened external URL", context: $"target={target}; url={url}");
            return true;
        }
        catch (Exception exception)
        {
            _logger.Log(LogLevel.Warn, "LINK", "Failed to open external URL", exception, $"target={target}; url={url}");
            return false;
        }
    }

    private static bool IsDiscordInvite(Uri uri)
    {
        string host = uri.Host ?? string.Empty;
        if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
        {
            host = host.Substring(4);
        }

        return host.Equals("discord.gg", StringComparison.OrdinalIgnoreCase)
            || host.Equals("discord.com", StringComparison.OrdinalIgnoreCase)
            || host.Equals("discordapp.com", StringComparison.OrdinalIgnoreCase);
    }
}
