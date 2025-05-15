namespace Common;

public class SiteSettings
{
    public string ElmahPath { get; set; } = null!;
    public IdentitySettings IdentitySettings { get; set; } = null!;
}

public class IdentitySettings
{
    public bool PasswordRequireDigit { get; set; }
    public int PasswordRequiredLength { get; set; }
    public bool PasswordRequireNonAlphanumeric { get; set; }
    public bool PasswordRequireUppercase { get; set; }
    public bool PasswordRequireLowercase { get; set; }
    public bool RequireUniqueEmail { get; set; }
}