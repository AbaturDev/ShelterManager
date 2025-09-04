using System.Text;
using ShelterManager.Common.Constants;

namespace ShelterManager.Common.Utils;

public static class ApiRouteBuilder
{
    private const char RouteSeparator = '/';

    public static string BuildBaseGroupRoute(string groupName, int version)
    {
        var builder = new StringBuilder();
        builder.AppendJoin(RouteSeparator, ApiRoutes.BaseRoot, $"v{version}", groupName);
        return builder.ToString();        
    }
}