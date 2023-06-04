using TreeV2.NodeDto;
using TreeV2.Entities;
using TreeV2.Entities;

namespace TreeV2.Interfaces
{
    public interface ITreeService
    {
        Task<List<Node>> GetAllNodesOrderedById();
        Task<TreeV2.Utilities.OperationResult> DeleteNodeById(int id);
        Task<TreeV2.Utilities.OperationResult> CreateNode(CreateNodeDto dto);
        Task<TreeV2.Utilities.OperationResult> EditNode(EditNodeDto dto);
    }
}