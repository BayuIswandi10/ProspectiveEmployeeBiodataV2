namespace Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public DateTime CreatedAt { get; set; }

    // Navigation
    public BiodataEntity? Biodata { get; set; }

    public bool IsAdmin() => Role == "admin";
}
