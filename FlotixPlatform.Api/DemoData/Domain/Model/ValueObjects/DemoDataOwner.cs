using System.Security.Cryptography;
using System.Text;

namespace FlotixPlatform.Api.DemoData.Domain.Model.ValueObjects;

/// <summary>
///     Owner identifier of an IAM user.
/// </summary>
/// <remarks>
///     IAM users have integer identifiers while the business tables reference owners by UUID. The owner UUID is
///     derived deterministically from the user id so the API and the database agree on it without storing it:
///     <c>md5('flotix:owner:' || users.id)::uuid</c>, the same expression used by the
///     <c>public.flotix_demo_owner_id</c> PL/pgSQL function.
/// </remarks>
public static class DemoDataOwner
{
    private const string Prefix = "flotix:owner:";

    /// <summary>
    ///     Gets the owner UUID of an IAM user.
    /// </summary>
    /// <param name="userId">The IAM user id.</param>
    /// <returns>The owner UUID that the demo data and the business tables use for that user.</returns>
    public static Guid ForUser(int userId)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(Prefix + userId));
        // PostgreSQL casts the 32 hexadecimal characters of md5() to uuid in textual order, so parse the hex string
        // instead of using new Guid(byte[]), which reorders the first three groups (little-endian).
        return Guid.ParseExact(Convert.ToHexString(hash), "N");
    }
}