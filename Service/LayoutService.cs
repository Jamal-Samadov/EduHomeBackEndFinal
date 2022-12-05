using EduHome.DAL;
using EduHome.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _dbContext;

        public LayoutService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Setting>> GetSettings()
        {
            List<Setting> settings =await _dbContext.Settings.ToListAsync();
            return settings;
        }
    }
}
