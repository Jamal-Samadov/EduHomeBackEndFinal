﻿using Microsoft.AspNetCore.Identity;

namespace EduHome.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
