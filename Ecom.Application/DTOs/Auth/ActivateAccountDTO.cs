﻿
namespace Ecom.Application.DTOs.Auth
{
    public record ActivateAccountDTO 
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
