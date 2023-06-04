using Microsoft.EntityFrameworkCore;
using TreeV2.Entities;
using TreeV2.Data;
using TreeV2.NodeDto;
using TreeV2.Interfaces;

namespace TreeV2.Services
{
    public class TreeService : ITreeService
    {
        private readonly TreeDbContext _dbcontext;

        public TreeService(TreeDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<List<Node>> GetAllNodesOrderedById()
        {
            return await _dbcontext.Nodes.OrderBy(n => n.Id).ToListAsync();
        }

        public async Task<TreeV2.Utilities.OperationResult> DeleteNodeById(int id)
        {
            var node = await _dbcontext.Nodes.FirstOrDefaultAsync(n => n.Id == id);

            if (node == null)
            {
                return TreeV2.Utilities.OperationResult.Failure("Node not found");
            }

            _dbcontext.Nodes.RemoveRange(node);
            await _dbcontext.SaveChangesAsync();

            return TreeV2.Utilities.OperationResult.CreateSuccess();
        }

        public async Task<TreeV2.Utilities.OperationResult> CreateNode(CreateNodeDto dto)
        {
            var nodes = await _dbcontext.Nodes.ToListAsync();

            var node = new Node()
            {
                Name = dto.Name
            };

            if (dto.ParentNode is null)
            {
                if (nodes.Any())
                {
                    return TreeV2.Utilities.OperationResult.Failure("You must select parent node");
                }

                await _dbcontext.Nodes.AddAsync(node);
                await _dbcontext.SaveChangesAsync();
                return TreeV2.Utilities.OperationResult.CreateSuccess();
            }

            var parentNode = await _dbcontext.Nodes.FirstOrDefaultAsync(n => n.Id == int.Parse(dto.ParentNode));

            if (parentNode == null)
            {
                return TreeV2.Utilities.OperationResult.Failure("Parent node not found");
            }

            parentNode.Children.Add(node);
            await _dbcontext.Nodes.AddAsync(node);
            await _dbcontext.SaveChangesAsync();

            return TreeV2.Utilities.OperationResult.CreateSuccess();
        }

        private async Task<bool> CheckIfPossible(Node node, Node targetNode)
        {
            var nodeWithChildren = await _dbcontext.Nodes.Include(n => n.Children).FirstOrDefaultAsync(n => n == node);

            if (nodeWithChildren?.Children.FirstOrDefault() is null)
            {
                return true;
            }

            if (nodeWithChildren.Children.Contains(targetNode))
            {
                return false;
            }

            foreach (var child in nodeWithChildren.Children)
            {
                if (!await CheckIfPossible(child, targetNode))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<TreeV2.Utilities.OperationResult> EditNode(EditNodeDto dto)
        {
            if (dto.Name is not null)
            {
                var node = await _dbcontext.Nodes.FirstOrDefaultAsync(n => n.Id == dto.SelectedNodeId);

                if (node == null)
                {
                    return TreeV2.Utilities.OperationResult.Failure("Selected node not found");
                }

                node.Name = dto.Name;

                await _dbcontext.SaveChangesAsync();
            }

            if (dto.ParentNode is not null)
            {
                var node = await _dbcontext.Nodes.FirstOrDefaultAsync(n => n.Id == dto.SelectedNodeId);

                if (node == null)
                {
                    return TreeV2.Utilities.OperationResult.Failure("Selected node not found");
                }

                var targetParentId = int.Parse(dto.ParentNode.Split(".")[0]);

                var targetParentNode = await _dbcontext.Nodes.FirstOrDefaultAsync(n => n.Id == targetParentId);

                if (targetParentNode == null)
                {
                    return TreeV2.Utilities.OperationResult.Failure("Parent node not found");
                }

                if (node == targetParentNode)
                {
                    return TreeV2.Utilities.OperationResult.Failure("You can't move node to this same node");
                }

                if (!await CheckIfPossible(node, targetParentNode))
                {
                    return TreeV2.Utilities.OperationResult.Failure("You can't move node to sub node of this node");
                }

                targetParentNode.Children.Add(node);

                await _dbcontext.SaveChangesAsync();
            }

            return TreeV2.Utilities.OperationResult.CreateSuccess();
        }
    }
}