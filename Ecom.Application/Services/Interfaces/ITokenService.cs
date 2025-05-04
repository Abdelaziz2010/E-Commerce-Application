﻿using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser user);
    }
}
