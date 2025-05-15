using Domain;

namespace Data.Contracts
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByUserAsync(string username, CancellationToken cancellationToken);
        Task<bool> CheckUserName(string userName, CancellationToken cancellationToken, params int[] ignoreIds);
        Task<bool> CheckNationalCode(string code, CancellationToken cancellationToken, params int[] ignoreIds);
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
        Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}