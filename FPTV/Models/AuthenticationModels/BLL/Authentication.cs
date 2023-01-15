/*
using FPTV.Data;
using FPTV.Models.Authentication.DAL;

namespace FPTV.Models.Authentication.BLL
{
    public class Authentication
    {
        public List<UserAccount> getUserAccounts(FPTVContext _context)
        {
            return _context.UserAccount; // toList...
        }

        public UserAccount getUserAccountByUserID(FPTVContext _context, Guid userID)
        {
            return _context.UserAccount.FirstOrDefaultAsync(u => u.id == userID);
        }
    }
}
*/