﻿using Microsoft.AspNetCore.Identity;

namespace LMS.WebAPI.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }

}
