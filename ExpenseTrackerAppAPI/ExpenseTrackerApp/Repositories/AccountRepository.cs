using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public bool AccountExists(int id)
        {
            return _context.Accounts.Any(a => a.Id == id);
        }

        public bool CreateAccount(int userId, Account account)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            var accountUser = new UserAccount()
            {
                User = user,
                Account = account,
                UserRole = 0
            };

            _context.Add(accountUser);
            _context.Add(account);

            return Save();
        }

        public bool DeleteAccount(Account account)
        {
            _context.Remove(account);
            return Save();
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.Where(a => a.Id == id).FirstOrDefault();
        }

        public ICollection<Account> GetAccounts()
        {
            return _context.Accounts.OrderBy(a => a.Id).ToList();
        }

        public ICollection<Account> GetAccountsByUserId(int userId)
        {
            return _context.UserAccounts.Where(u => u.UserId == userId).Select(a => a.Account).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAccount(Account account)
        {
            _context.Update(account);
            return Save();
        }

        public bool AddUserToAccount(int userId, int accountId, UserRole userRole)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            var account = _context.Accounts.Where(a => a.Id == accountId).FirstOrDefault();
            var accountUser = new UserAccount()
            {
                User = user,
                Account = account,
                UserRole = userRole,
            };

            _context.Add(accountUser);
            return Save();
        }

        public bool RemoveUserFromAccount(int userId, int accountId)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            var account = _context.Accounts.Where(a => a.Id == accountId).FirstOrDefault();
            var accountUser = new UserAccount()
            {
                User = user,
                Account = account,
            };
            _context.Remove(accountUser);
            return Save();
        }

        public UserRole GetUserRoleByAccountIdAndUserId(int accountId, int userId)
        {
            var userAccount = _context.UserAccounts.Where(ua => ua.AccountId ==  accountId).Where(ua => ua.UserId == userId).Select(ua => ua.UserRole).FirstOrDefault();
            return userAccount;
        }
    }
}
