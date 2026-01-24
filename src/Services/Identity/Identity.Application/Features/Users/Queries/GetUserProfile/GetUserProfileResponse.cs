namespace Identity.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileResponse
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public bool Status { get; set; }
    public string RegistrationIp { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public List<string> Roles { get; set; } = new();

    public int ActiveSessionCount { get; set; }
    public DateTime? LastLoginDate { get; set; }
}
