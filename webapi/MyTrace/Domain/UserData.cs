using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MyTrace.Models;
using System.Diagnostics;

namespace MyTrace.Domain
{
    public class UserData
    {
        private readonly MyTraceContext _context;

        public UserData(MyTraceContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(string wallet)
        {
            User user = new User(); 

            try
            {
                user = await _context.Users
                        .Where(u => u.WalletAddress == wallet).SingleAsync();
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return user;
        }
    }
}
