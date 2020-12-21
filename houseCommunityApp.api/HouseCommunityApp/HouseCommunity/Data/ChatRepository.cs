using AutoMapper;
using HouseCommunity.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext dataContext;

        public ChatRepository(DataContext _dataContext)
        {
            this.dataContext = _dataContext;
        }
    }
}
