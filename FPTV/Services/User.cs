using FPTV.Data;
using FPTV.Models.UserModels;

namespace FPTV.Services
{
    /// <summary>
    /// This class contains all the methods to get any information about an user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Returns all Profiles
        /// </summary>
        /// <param name="_context"></param>
        /// <returns> List<Profile> </returns>
        public List<Profile> getProfiles(FPTVContext _context)
        {
            return _context.Profiles.ToList();
        }

        /// <summary>
        /// Returns true if the profile exists in the database
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>bool</returns>
        public bool existProfile(FPTVContext _context, Guid userID)
        {
            return getProfiles(_context).Any(u => u.UserId == userID);
        }

        /// <summary>
        /// Returns a user's profile by its UserId
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>Profile</returns>
        /// <exception cref="ArgumentException"></exception>
        public Profile getProfileByUserID(FPTVContext _context, Guid userID)
        {
            if (existProfile(_context, userID))
            {
                return _context.Profiles.Single(p => p.UserId == userID);
            }
            else
            {
                throw new ArgumentException(message: "Profile doesn't exist.", paramName: nameof(userID));
            }
        }

        /// <summary>
        /// Returns the Profile Picture via the User ID
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>ProfilePicture</returns>
        /// <exception cref="ArgumentException"></exception>
        public byte[] getProfilePictureByUserID(FPTVContext _context, Guid userID)
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

        /// <summary>
        /// Returns the comments of a user via the User ID
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>List<Comment></returns>
        public List<Comment> getCommentsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Comments.ToList().FindAll(u => u.ProfileId == userID);
        }

        /// <summary>
        /// Returns a user's topics by User ID
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>List<Topic></returns>
        public List<Topic> getTopicsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Topics.ToList().FindAll(u => u.ProfileId == userID);
        }

        /// <summary>
        /// Returns the reactions of a user via the User ID
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>List<Reaction></returns>
        public List<Reaction> getReactionsByUserID(FPTVContext _context, Guid userID)
        {
            return _context.Reactions.ToList().FindAll(u => u.UserId == userID);
        }

        /// <summary>
        /// Returns a user's favorite players list using the User ID
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>List<FavPlayerList></returns>
        public List<FavPlayerList> getFavPlayersByUserID(FPTVContext _context, Guid userID)
        {
            var profile = getProfileByUserID(_context, userID);
            return _context.FavPlayerList.ToList().FindAll(u => u.ProfileId == profile.Id);
        }

        /// <summary>
        /// Returns a user's favorite teams list using the User ID
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userID"></param>
        /// <returns>List<FavTeamsList></returns>
        public List<FavTeamsList> getFavTeamsByUserID(FPTVContext _context, Guid userID)
        {
            var profile = getProfileByUserID(_context, userID);
            return _context.FavTeamsList.ToList().FindAll(u => u.ProfileId == profile.Id);
        }

        /*
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
        }*/
    }
}
