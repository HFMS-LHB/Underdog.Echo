using AutoMapper;

using Underdog.Echo.Common.Attributes;
using Underdog.Echo.IServices;
using Underdog.Echo.Model.Models;
using Underdog.Echo.Services.BASE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Services
{
    internal class CardBoxServices : BaseServices<CardBox>, ICardBoxServices
    {
        IMapper _mapper;
        public CardBoxServices(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task<List<CardBox>> GetCardBoxs()
        {
            var cardBoxList = await base.Query(x=>x.IsEnabled);

            return cardBoxList;
        }
    }
}
