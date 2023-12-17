namespace CircleCat.MicrosoftEntraID.AZ204.Services
{
    public interface IUserServices
    {
        public Task<List<string>> GetUserRolesFromUserName(string userName);
    }
}
