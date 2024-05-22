using AutoMapper;

using Underdog.Common.Attributes;
using Underdog.IServices;
using Underdog.Model.Models;
using Underdog.Services.BASE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Services
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
