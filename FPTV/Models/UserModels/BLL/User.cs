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
        public List<Profile> getProfiles(FPTVContext _context)
        { 
            return _context.Profile.ToList();
        }

        public bool existProfileAccount(FPTVContext _context, Guid userID)
        {
            return getProfiles(_context).Any(u => u.UserId == userID);
        }

        public Profile getProfileByProfileID(FPTVContext _context, Guid profileID)
        {
            if (existProfileAccount(_context, profileID))
            {
                return _context.Profile.Find(profileID);
            }
            else
            {
                throw new ArgumentException(message: "User doesn't exist.", paramName: nameof(profileID));
            }
        }

        public ProfilePicture getProfilePictureByProfileID(FPTVContext _context, Guid profileID)
        {
            if (existProfileAccount(_context, profileID))
            {
                return getProfileByProfileID(_context, profileID).Picture;
            }
            else
            {
                throw new ArgumentException(message: "Profile doesn't exist.", paramName: nameof(profileID));
            }
        }
        
        public List<Comment> getCommentByProfileID(FPTVContext _context, Guid profileID)
        {
            return _context.Comments.ToList().FindAll(u => u.UserId == profileID);
        }

        /*
        public Guid getUserIDByUserAccountID(FPTVContext _context, Guid userAccountID)
        {
            if (existProfileAccount(_context, userAccountID))
            {
                return getUserAccountByUserID(_context, userID).userAccountId;
            }
            else
            {
                throw new ArgumentException(message: "Profile doesn't exist.", paramName: nameof(profileID));
            }
        }
        */

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
    }

