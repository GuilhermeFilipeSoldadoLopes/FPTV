using FPTV.Data;
using FPTV.Models.UserModels.DAL;
using FPTV.Models.AuthenticationModels.DAL;

namespace FPTV.Models.UserModels.BLL
{

    public class User
    {
        //Retorna todos os Profiles
        public List<Profile> getProfiles(FPTVContext _context)
        {
            return _context.Profile.ToList();
        }

        //Retorna true caso o profile exista na base de dados
        public bool existProfile(FPTVContext _context, Guid userID)
        {
            return getProfiles(_context).Any(u => u.UserId == userID);
        }

        //Retorna o Profile de um utilizador através do seu UserId
        public Profile getProfileByUserID(FPTVContext _context, Guid userID)
        {
            if (existProfile(_context, userID))
            {
                return _context.Profile.Find(userID);
            }
            else
            {
                throw new ArgumentException(message: "Profile doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna a Profile Picture através do User ID
        public ProfilePicture getProfilePictureByUserID(FPTVContext _context, Guid userID)
        {
            if (existProfile(_context, userID))
            {
                return getProfileByUserID(_context, userID).Picture;
            }
            else
            {
                throw new ArgumentException(message: "Profile doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna os comentários de um utilizador através do User ID
        public List<Comment> getCommentsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Comments.ToList().FindAll(u => u.UserId == userID);
        }

        //Retorna os tópicos de um utilizador através do User ID
        public List<Topic> getTopicsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Topics.ToList().FindAll(u => u.UserId == userID);
        }

        //Retorna as reações de um utilizador através do User ID
        public List<Reaction> getReactionsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Reactions.ToList().FindAll(u => u.UserId == userID);
        }

        //Retorna a lista de jogadores favoritos de um utilizador através do User ID
        public List<FavPlayerList> getFavPlayersByUserID(FPTVContext _context, Guid userID)
        {
            return _context.FavPlayerList.ToList().FindAll(u => u.UserId == userID);
        }

        //Retorna a lista de equipas favoritas de um utilizador através do User ID
        public List<FavTeamsList> getFavTeamsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.FavTeamsList.ToList().FindAll(u => u.UserId == userID);
        }

        //Retorna o tipo de utilizador a partir do User ID
        public UserType getUserTypeByUserID(FPTVContext _context, Guid userID)
        {
            if (existProfile(_context, userID))
            {
                return getProfileByUserID(_context, userID).UserType;
            }
            else
            {
                throw new ArgumentException(message: "Profile doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna a UserAccount de um utilizador atraves do seu ID (UserId)
        public UserAccount getUserAccountByUserID(FPTVContext _context, Guid userID)
        {
            if (existUserAccount(_context, userID))
            {
                return _context.UserAccount.FirstOrDefault(u => u.UserId == userID);
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(userID));
            }
        }

        //Retorna todos as UserAccounts
        public List<UserAccount> getUserAccounts(FPTVContext _context)
        {
            return _context.UserAccount.ToList();
        }

        //Retorna true caso o otilizador exista na base de dados
        public bool existUserAccount(FPTVContext _context, Guid userID)
        {
            return getUserAccounts(_context).Any(u => u.UserId == userID);
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
    }
}
