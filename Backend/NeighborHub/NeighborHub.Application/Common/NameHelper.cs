namespace NeighborHub.Application.Common;

public static class NameHelper
{
    public static string BuildFullName(string? firstName, string? lastName)
    {
        string first = firstName?.Trim() ?? string.Empty;
        string last = lastName?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(first))
        {
            return last;
        }

        if (string.IsNullOrEmpty(last))
        {
            return first;
        }

        if (string.Equals(first, last, StringComparison.OrdinalIgnoreCase))
        {
            return first;
        }

        return $"{first} {last}";
    }

    public static string Normalize(string? fullName, string fallback = "Unknown User")
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return fallback;
        }

        string trimmed = fullName.Trim();
        string[] parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2 &&
            string.Equals(parts[0], parts[1], StringComparison.OrdinalIgnoreCase))
        {
            return parts[0];
        }

        return trimmed;
    }
}
