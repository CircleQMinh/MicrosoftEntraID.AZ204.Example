
using CircleCat.MicrosoftEntraID.AZ204.Helpers;

namespace CircleCat.MicrosoftEntraID.AZ204.Services
{
    public class UserService : IUserServices
    {
        public Task<List<string>> GetUserRolesFromUserName(string userName)
        {
            //get from database using EF Core -> need implement
            // temp result for testing 
            //var result = new List<string>() {
            //    //no role, will default to User 
            //};
            //var result = new List<string>() { 
            //    AppRole.Admin,
            //};
            var result = new List<string>() {
                AppRole.Manager,
            };
            //var result = new List<string>() {
            //    AppRole.Admin, AppRole.Manager,
            //};

            return Task.FromResult(result);
        }
    }
}
