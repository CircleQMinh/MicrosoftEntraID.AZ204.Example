using CircleCat.MicrosoftEntraID.AZ204.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CircleCat.MicrosoftEntraID.AZ204.Helpers
{
    public class ClaimTranformer : IClaimsTransformation
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _configuration;
        public ClaimTranformer(IUserServices userServices, IConfiguration configuration)
        {
            _configuration = configuration;
            _userServices = userServices;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity == null)
            {
                return await Task.FromResult(principal);
            }
            if (!principal.Identity.IsAuthenticated)
            {
                return await Task.FromResult(principal);
            }
            
            var identity = (ClaimsIdentity)principal.Identity; //e.g abcd@mgi.com
            var userName = identity.Name.Split('@')[0]; // abcd

            var userRoles = await _userServices.GetUserRolesFromUserName(userName); //get role assigned for user from database
            userRoles.Add(AppRole.User); //add default role

            foreach (var role in userRoles)
            {
                AddClaim(identity, role, role);
            }
            AddClaim(identity, RoleBasePolicyHepler.UserRoles, string.Join(";",userRoles)); 

            return await Task.FromResult(principal);
        }

        private static void AddClaim(ClaimsIdentity identity, string type, string value)
        {
            if (!identity.HasClaim(type, value) && !string.IsNullOrEmpty(value))
            {
                identity.AddClaim(new Claim(type, value));
            }
        }
    }
}
