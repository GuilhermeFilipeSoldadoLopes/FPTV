using FPTV.Data;
using FPTV.Models.Authentication.DAL;

namespace FPTV.Models.Authentication.BLL
{
    public class Authentication
    {
        private FPTVContext _context;
        //gets
        public UserAccount getUserAccounts(FPTVContext _context)
        {
            return _context.UserAccount;
        }

        public FPTV_BD.UserAccountDataTable getUserAccountsByUserID(Guid userID)
        {
            return _productsAdapter.GetUserAccountsByUserID(userID);
        }

        public FPTV_BD.UserAccountDataTable getAuthenticationtType()
        {

        }
    }
}
