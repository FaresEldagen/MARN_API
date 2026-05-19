using MARN_API.Enums.Account;

namespace MARN_API.DTOs.Admin
{
    public class AdminRoleManagementQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? Role { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public bool IncludeDeleted { get; set; } = true;
    }

    public class AdminRoleDefinitionDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string RoleNameDisplayName { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public int UsersCount { get; set; }
        public bool IsProtected { get; set; }
        public bool IsAssignable { get; set; }
    }

    public class AdminRoleUserListItemDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = [];
        public List<string> RolesDisplayNames { get; set; } = [];
    }

    public class AdminRoleUserDetailsDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImage { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = [];
        public List<string> RolesDisplayNames { get; set; } = [];
        public List<AdminRoleDefinitionDto> AvailableRoles { get; set; } = [];
    }

    public class AdminUpdateUserRolesDto
    {
        public List<string> Roles { get; set; } = [];
    }
}
