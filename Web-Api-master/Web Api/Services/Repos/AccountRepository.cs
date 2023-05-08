using Microsoft.EntityFrameworkCore;
using System.Linq;
using Web_Api.Models;
using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Services;

public class AccountRepository : BaseRepository<Account>
{
    public AccountRepository(WebApiContext context)
        : base(context) { }
    public async Task<Account> GetAccountByUserNameAsync(string name)
    {
        Account account = await _dbSet.FirstOrDefaultAsync(a => a.Name == name);
        if (account is null)
        {
            return null;
        }
        _db.Set<RefreshToken>().Where(rt => rt.IsActive && rt.AccountId == account.Id).Load();
        return account;
    }
    public async Task<bool> IsUsernameFreeAsync(string Name)
    {
        if (!await _dbSet.AnyAsync(u => u.Name == Name))
        {
            return true;
        }
        return false;
    }
    public async Task<RefreshToken> GetRefreshToken(string refreshTokenValue)
    {
        return await _db.Set<RefreshToken>()
            .Include(rt => rt.Account)
            .FirstOrDefaultAsync(rt => rt.Value == refreshTokenValue && rt.IsActive);
    }
    public async Task<bool> IsRT_UniqueAsync(string refreshToken)
    {
        return !await _db.Set<RefreshToken>().AnyAsync(rt => rt.Value == refreshToken);
    }
}
