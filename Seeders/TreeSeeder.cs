using TreeV2.Data;
using TreeV2.Entities;

namespace TreeV2.Seeders
{
    public class TreeSeeder
    {
        private readonly TreeDbContext _dbContext;

        public TreeSeeder(TreeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if(await _dbContext.Database.CanConnectAsync())
            {
                if(!_dbContext.Nodes.Any())
                {
                    var parent1 = new Node()
                    {
                        Name = "Parent1",
                    };

                    var parent2 = new Node()
                    {
                        Name = "Parent2",
                    };

                    var parent3 = new Node()
                    {
                        Name = "Parent3",
                    };

                    _dbContext.Nodes.AddRange(parent1, parent2, parent3);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
