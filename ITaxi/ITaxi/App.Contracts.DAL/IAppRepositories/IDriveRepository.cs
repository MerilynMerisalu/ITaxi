﻿using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriveRepository: IEntityRepository<Drive>
{
    Task<IEnumerable<Drive>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Drive> GetAllDrivesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Drive>> GettingAllOrderedDrivesWithIncludesAsync(bool noTracking = true);
    IEnumerable<Drive> GettingAllOrderedDrivesWithIncludes(bool noTracking = true);
    
    Task<Drive?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true);
    Drive? GetDriveWithoutIncludes(Guid id, bool noTracking = true);
    Task<IEnumerable<Drive?>> SearchByDateAsync(DateTime search);
    IEnumerable<Drive?> SearchByDate(DateTime search);

    Task<IEnumerable<Drive?>> PrintAsync(Guid id);
    IEnumerable<Drive?>Print(Guid id);
    string PickUpDateAndTimeStr(Drive drive);
    Task<IEnumerable<Drive?>> GettingDrivesWithoutCommentAsync(bool noTracking = true);
    IEnumerable<Drive?> GettingDrivesWithoutComment(bool noTracking = true);
    

}