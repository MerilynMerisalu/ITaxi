﻿
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

public class BaseUser: BaseUser<Guid>, IDomainEntityId
{
    public BaseUser() 
    {
        
    }

    public BaseUser(string username) : base(username)
    {
        
    }
}

public class BaseUser<TKey> : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
{
    public BaseUser()
    {
        
    }

    public BaseUser(string username) : base(username)
    {
        
    }

    public string? ProfilePhotoName { get; set; } 
    public byte[]? ProfilePhoto { get;set; }
}