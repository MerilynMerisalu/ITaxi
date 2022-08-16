using System.ComponentModel;
using System.Security.Claims;

namespace Base.Extensions;

public static class IdentityExtension
{
    public static Guid GettingUserId(this ClaimsPrincipal user) => GettingUserId<Guid>(user);
    public static TKeyType GettingUserId<TKeyType>(this ClaimsPrincipal user)
    {
        /*if (typeof(TKeyType) != typeof(Guid) 
            || typeof(TKeyType) != typeof(string)
            || typeof(TKeyType) != typeof(int))
        {
            throw new ApplicationException($"This type of user id {} is not supported!");
        }
        */

        var claimId = user.Claims.FirstOrDefault(u => u.Type.Equals(ClaimTypes.NameIdentifier));
        if (claimId == null)
        {
            throw new NullReferenceException("Name identifier claim not found!");
        }

        var res = (TKeyType) TypeDescriptor.GetConverter(typeof(TKeyType))
            .ConvertFromInvariantString(claimId.Value)!;
        return res;
    }
}