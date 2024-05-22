using Underdog.IServices.BASE;
using Underdog.Model.Models;

namespace Underdog.IServices
{
    public interface ICardBoxServices:IBaseServices<CardBox>
    {
        Task<List<CardBox>> GetCardBoxs();
    }
}
