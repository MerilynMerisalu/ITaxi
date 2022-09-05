﻿using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICommentRepository : IEntityRepository<Comment>
{
   Task<IEnumerable<Comment>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true);
   IEnumerable<Comment> GetAllCommentsWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithIncludesAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    //IEnumerable<Comment> GettingAllOrderedCommentsWithIncludes(bool noTracking = true);
    Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Comment> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true);
    Task<Comment?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true);
    //Comment? GettingCommentWithoutIncludes(Guid id, bool noTracking = true);
    string PickUpDateAndTimeStr(Comment comment);
    Task<Comment?> GettingTheFirstCommentAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    Comment? GettingTheFirstComment(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
}