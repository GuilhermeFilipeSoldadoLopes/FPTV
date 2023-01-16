using System.Runtime;
using System.Xml.Linq;
using FPTV.Data;
using FPTV.Models.Authentication.DAL;
using FPTV.Models.DAL;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FPTV.Models.BLL
{

    public class User
    {
        //Retorna todos os Profiles
        public List<Profile> getProfiles(FPTVContext _context)
        {
            return _context.Profile.ToList();
        }

        //Retorna true caso o profile exista na base de dados
        public bool existProfileAccount(FPTVContext _context, Guid userID)
        {
            return getProfiles(_context).Any(u => u.UserId == userID);
        }

        //Retorna o Profile de um utilizador através do seu UserId
        public Profile getProfileByUserID(FPTVContext _context, Guid userID)
        {
            if (existProfileAccount(_context, userID))
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
            if (existProfileAccount(_context, userID))
            {
                return getProfileByUserID(_context, userID).Picture;
            }
            else
            {
                throw new ArgumentException(message: "Profile Picture doesn't exist.", paramName: nameof(userID));
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

    }
}

