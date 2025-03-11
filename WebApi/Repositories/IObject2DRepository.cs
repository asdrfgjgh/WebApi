﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiObject2D;

namespace WebApi.Repositories2D
{
    public interface IObject2DRepository
    {
        Task<IEnumerable<Object2D>> GetByEnvironmentIdAsync(Guid environmentId);
        Task<Object2D> AddObject2DAsync(Object2D object2D);
        Task<Object2D> UpdateObject2DAsync(Object2D object2D);
        Task DeleteObjectAsync(Guid id);
        Task DeleteAllByEnvironmentIdAsync(Guid environmentId);
    }
}
