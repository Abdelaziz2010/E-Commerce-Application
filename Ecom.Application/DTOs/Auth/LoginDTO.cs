﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.DTOs.Auth
{
    public record LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
