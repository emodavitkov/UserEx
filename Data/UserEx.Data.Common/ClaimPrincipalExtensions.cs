namespace UserEx.Data.Common
{
    using System.Security.Claims;

    public static class ClaimPrincipalExtensions
    {
        public const string AdministratorRoleName = "Administrator";

        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
    }
}
