namespace ABSolutions.ImageToBase64.Models;

/// <summary>
///     Base64 cached string with optional expiry date.
/// </summary>
public record Base64CachedObject
{
    public required string Filename { get; init; }
    public required string Base64String { get; init; }
    public DateTime? Expiry { get; init; }

    /// <summary>
    ///     Override ToString() to provide relevant debugging output.
    /// </summary>
    /// <returns>String representation of this object.</returns>
    public override string ToString()
    {
        return
            $"Filename: {Filename} | Base64String: {(string.IsNullOrWhiteSpace(Base64String) ? "<null>" : AbbreviatedBase64String())} | Expiry: {(Expiry.HasValue ? Expiry.Value.ToString("O") : "infinite")}";
    }

    /// <summary>
    ///     Get an abbreviated version of the Base64 string. Useful for log output.
    /// </summary>
    /// <returns>Base64String if 20 characters or less, otherwise return the first 5 characters and last 10 characters.</returns>
    private string AbbreviatedBase64String()
    {
        if (Base64String.Length <= 20) return Base64String;

        var base64Span = Base64String.AsSpan();
        return $"{base64Span[..5].ToString()}...{base64Span.Slice(Base64String.Length - 10, 10).ToString()}";
    }
}