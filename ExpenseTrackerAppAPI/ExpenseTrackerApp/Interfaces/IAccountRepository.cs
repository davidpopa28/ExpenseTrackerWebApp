using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Interfaces
{
    public interface IAccountRepository
    {
        ICollection<Account> GetAccounts();
        Account GetAccount(int id);
        ICollection<Account> GetAccountsByUserId(int userId);
        UserRole GetUserRoleByAccountIdAndUserId(int accountId, int userId);
        bool CreateAccount(int userId, Account account);
        bool Save();
        bool AccountExists(int id);
        bool UpdateAccount(Account account);
        bool DeleteAccount(Account account);
        bool AddUserToAccount(int userId, int accountId, UserRole userRole);
        bool RemoveUserFromAccount(int userId, int accountId);
    }
}
