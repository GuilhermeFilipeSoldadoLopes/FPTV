using FPTV.Data;
using FPTV.Models.Authentication.DAL;
using FPTV.Models.ToReview;

namespace FPTV.Models.Authentication.BLL
{
    public class Authentication
    {
        //gets
        public List<UserAccount> getUserAccounts(FPTVContext _context)
        {

            return _context.UserAccount.Include(u => u.User, u => u.AuthenticationLog); // toList...
        }

        public UserAccount getUserAccountsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.UserAccount.Include(u => u.User, u => u.AuthenticationLog).FirstOrDefaultAsync(u => u.id == userID);
        }
    }
}
