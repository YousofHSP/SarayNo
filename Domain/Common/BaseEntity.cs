﻿using Microsoft.EntityFrameworkCore.Metadata;

namespace Domain.Common;

public interface IEntity
{
    
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
    DateTimeOffset CreatedAt { get; set; }
}

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}

public abstract class BaseEntity : BaseEntity<int>
{
    
}