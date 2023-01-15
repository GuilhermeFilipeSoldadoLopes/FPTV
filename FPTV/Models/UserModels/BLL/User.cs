/*
using System.Runtime;
using FPTV.Models.DAL;

namespace FPTV.Models.BLL
{
    
    public class User
    {
        
        public Profile? profile { get; set; }

        public List<Profile> getProfile(FPTVContext _context)
        {
            return _context.Profile;
        }

        public Profile getProfileAccountByProfileID(FPTVContext _context, Guid profileID)
        {
            return _context.Profile.FirstOrDefaultAsync(u => u.userId == profileID);
        }
    }
}
*/
