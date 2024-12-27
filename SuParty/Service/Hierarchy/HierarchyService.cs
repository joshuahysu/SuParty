using SuParty.Data;

namespace SuParty.Service.Hierarchy
{
    public class HierarchyService
    {
        private readonly ApplicationDbContext _dbContext;

        public HierarchyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void t(string id)
        {
            //誰可以分
        }
    }
}
