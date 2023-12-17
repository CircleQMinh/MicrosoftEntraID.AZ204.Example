using System.Security.Claims;

namespace CircleCat.MicrosoftEntraID.AZ204.Helpers
{
    public class RoleBasePolicyHepler
    {
        public const string UserRoles = "Roles";
        public static List<string> GetUserRolesFromClaim(ClaimsPrincipal principal)
        {
            try
            {
                var rolesValue = principal.Claims.Where(q => q.Type == RoleBasePolicyHepler.UserRoles).Select(q => q.Value).FirstOrDefault();
                return rolesValue.Split(";").ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
      
        }
    }

    public static class AppRole
    {
        public const string Admin = "admin";
        public const string Manager = "manager";
        //base role
        public const string User = "user";
    }

    public static class AppPolicy
    {
        public const string Admin = "admin";
        public const string Manager = "manager";
        public const string User = "user";
        public const string HighLevelUserOnly = "highleveluseronly";
    }
}
