namespace BetterEntryPoint;

public class NodeDto {
    [Required]
    public string CurrentCity { get; set; } = null!;
    [Required]
    public string EndCity { get; set; } = null!;
}