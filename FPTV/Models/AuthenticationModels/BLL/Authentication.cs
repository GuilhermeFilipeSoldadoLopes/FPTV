using FPTV.Data;
using FPTV.Models.AuthenticationModels.DAL;
using FPTV.Models.UserModels.DAL;

namespace FPTV.Models.AuthenticationModels.BLL
{
    public class Authentication
    {
        //FPTVContext _context;

        //Retorna todos as UserAccounts
        public List<UserAccount> getUserAccounts(FPTVContext _context)
        {
            return _context.UserAccount.ToList();
        }

        //Retorna true caso o otilizador exista na base de dados atraves do ID do utilizador (UserId)
        public bool existUserAccountByUserID(FPTVContext _context, Guid userID)
        {
            return getUserAccounts(_context).Any(u => u.UserId == userID);
        }

        //Retorna true caso o otilizador exista na base de dados atraves do ID da UserAccount (UserAccountId)
        public bool existUserAccountByUserAccountID(FPTVContext _context, Guid userAccountID)
        {
            return getUserAccounts(_context).Any(u => u.UserAccountId == userAccountID);
        }

        //Retorna a UserAccount de um utilizador atraves do seu ID (UserId)
        public UserAccount getUserAccountByUserID(FPTVContext _context, Guid userID)
        {
            if (existUserAccountByUserID(_context, userID))
            {
                return _context.UserAccount.FirstOrDefault(u => u.UserId == userID);
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna ID da UserAccount (UserAccountId) atraves do ID do utilizador (UserId)
        public Guid getUserAccounIDtByUserID(FPTVContext _context, Guid userID)
        {
            if (existUserAccountByUserID(_context, userID))
            {
                return getUserAccountByUserID(_context, userID).UserAccountId;
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna o utilizador (Profile) atraves do ID da sua UserAccount (UserAccountId)
        public Profile getProfileByUserAccountID(FPTVContext _context, Guid userAccounID)
        {
            if (getUserAccounts(_context).Any(u => u.UserAccountId == userAccounID))
            {
                return _context.Profile.FirstOrDefault(p => p.UserId == getProfileIDByUserAccounID(_context, userAccounID));
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userAccounID));
            }
        }

        //Retorna o utilizador (Profile) atraves do ID do user (userId)
        public Profile getProfileByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Profile.FirstOrDefault(p => p.UserId == userID);
            /*if ()
            {
                return _context.Profile.FirstOrDefault(p => p.UserId == userID);
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userID));
            }*/
        }

        //Retorna o ID do utilizador (UserId) atraves do ID da sua UserAccount (UserAccountId)
        public Guid getProfileIDByUserAccounID(FPTVContext _context, Guid userAccounID)
        {
            if (getUserAccounts(_context).Any(u => u.UserAccountId == userAccounID))
            {
                return _context.UserAccount.FirstOrDefault(u => u.UserAccountId == userAccounID).UserId;
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userAccounID));
            }
        }

        //Retorna o tipo de autenticao (Acccount, Steam, Google) do utilizador atraves do seu ID (UserId)
        public AuthenticationType getAuthenticationTypeByUserID(FPTVContext _context, Guid userID)
        {
            if (existUserAccountByUserID(_context, userID))
            {
                return getUserAccountByUserID(_context, userID).AuthenticationType;
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna true caso o otilizador ja se encontre verificado
        public bool getIsValidatedByUserID(FPTVContext _context, Guid userID)
        {
            if (existUserAccountByUserID(_context, userID))
            {
                return getUserAccountByUserID(_context, userID).Validated;
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna a AuthenticationChange da UserAccount do utilizador
        public AuthenticationChange getAuthenticationChangeByUserID(FPTVContext _context, Guid userID)
        {
            var aux = getUserAccounIDtByUserID(_context, userID);
            return _context.AuthenticationChanges.FirstOrDefault(a => a.UserAccountId == aux);
        }

        //Retorna a AuthenticationLog da UserAccount do utilizador
        public AuthenticationLog getAuthenticationLogByUserID(FPTVContext _context, Guid userID)
        {
            var aux = getUserAccounIDtByUserID(_context, userID);
            return _context.AuthenticationLog.FirstOrDefault(a => a.UserAccountId == aux);
        }

        //Retorna a AuthenticationRecovery da UserAccount do utilizador
        public AuthenticationRecovery getAuthenticationRecoveryByUserID(FPTVContext _context, Guid userID)
        {
            var aux = getUserAccounIDtByUserID(_context, userID);
            return _context.AuthenticationRecovery.FirstOrDefault(a => a.UserAccountId == aux);
        }
    }
}
