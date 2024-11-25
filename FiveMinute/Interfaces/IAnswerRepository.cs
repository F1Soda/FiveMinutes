﻿using FiveMinutes.Interfaces;
using FiveMinutes.Models;

namespace FiveMinute.Interfaces
{
    public interface IAnswerRepository : IDefaultRepository<Answer>
    {
        Task<ICollection<Answer>> GetByIdFMTAsync(int fiveMinuteId);
    }
}
