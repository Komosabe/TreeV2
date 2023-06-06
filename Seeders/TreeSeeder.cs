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
                    var child1 = new Node()
                    {
                        Name = "AChild1",
                    };

                    var child2 = new Node()
                    {
                        Name = "EChild2",
                    };

                    var child3 = new Node()
                    {
                        Name = "BChild3",
                    };
                    var child4 = new Node()
                    {
                        Name = "AAChild3",
                    };
                    var child5 = new Node()
                    {
                        Name = "CChild3",
                    };
                    var parent1 = new Node()
                    {
                        Name = "Parent1",
                        Children = new List<Node>()
                        {
                            child1,
                            child2,
                            child3,
                            child4,
                            child5
                        }
                    };
                    _dbContext.Nodes.AddRange(child1, child2, child3,child4, child5, parent1);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
