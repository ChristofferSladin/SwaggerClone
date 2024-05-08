using System;

namespace SwaggerCloneLibrary.Utility;

public static class Validation
{
    public static bool IsNotValidUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        bool output = !(Uri.TryCreate(url, UriKind.Absolute, out Uri uriOutput) && (uriOutput.Scheme == Uri.UriSchemeHttps));

        return output;
    }

    public static bool IsNotWellFormedUrl(string url)
    {
        if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return true;
        }
        return false;
    }
}
