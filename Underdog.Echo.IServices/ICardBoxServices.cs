using Underdog.Echo.IServices.BASE;
using Underdog.Echo.Model.Models;

namespace Underdog.Echo.IServices
{
    public interface ICardBoxServices:IBaseServices<CardBox>
    {
        Task<List<CardBox>> GetCardBoxs();
    }
}
