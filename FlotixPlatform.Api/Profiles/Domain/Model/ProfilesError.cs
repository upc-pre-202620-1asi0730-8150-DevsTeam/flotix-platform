namespace FlotixPlatform.Api.Profiles.Domain.Model;

public enum ProfilesError
{
    None,
    ProfileNotFound,
    EmailAlreadyRegistered,
    InvalidProfileData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
