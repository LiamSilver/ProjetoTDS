using AppTDS.Data;
using AppTDS.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTDS.Services
{
    public class UserService
    {
        private readonly MyDbContext _context;

        public UserService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }


}
