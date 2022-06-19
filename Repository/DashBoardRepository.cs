using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashBoardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
           _httpContextAccessor = httpContextAccessor;
        }      

        public async Task<List<Club>> GetAllUserClubs()
        {
            var CurUser= _httpContextAccessor.HttpContext.User.GetUserId();
            var UserClubs =  _context.Clubs.Where(r => r.AppUser.Id == CurUser);
            return UserClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var CurUser= _httpContextAccessor.HttpContext.User.GetUserId();
            var UserRaces =  _context.Races.Where(r => r.AppUser.Id == CurUser);
            return UserRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id) 
        
        {

            return await _context.Users.FindAsync(id);
        
        
        }

        public async Task<AppUser> GetByIdNoTracking(string id) 

        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        
        
        }
        public bool Update(AppUser user)
        { 

            _context.Users.Update(user);
            return Save();
        
        
        }

        public bool Save() 
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        
        
        }
    }
}
