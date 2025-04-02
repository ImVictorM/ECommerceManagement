namespace Application.Common.DTOs.Results;

/// <summary>
/// Represents a created result that contains the created resource's identifier.
/// </summary>
public class CreatedResult
{
    /// <summary>
    /// Gets the created resource identifier.
    /// </summary>
    public string Id { get; }

    private CreatedResult(string id)
    {
        Id = id;
    }

    internal static CreatedResult FromId(string id)
    {
        return new CreatedResult(id);
    }
};
