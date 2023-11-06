using ShopForProducts.Entities;

namespace ShopForProducts.IServices
{
    public interface IDecentralizationsService
    {
        Task<Decentralization> Create(string name);
        Task<Decentralization> Delete(string name);
        Task<List<Decentralization>> GetDecentralization();
    }
}
