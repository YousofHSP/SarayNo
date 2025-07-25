﻿using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain;

[Display(Name = "نقش")]
public class Role: IdentityRole<int>, IEntity<int>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public List<User> Users { get; set; }
}