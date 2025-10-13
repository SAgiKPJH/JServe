namespace ProcessService.VolueObject;

/// <summary>
/// Process status enumeration.
/// Acodding to this site https://en.wikipedia.org/wiki/Process_state
/// </summary>
internal enum StatusEnum
{
    New,
    Waiting,
    Running,
    Blocked,
    Terminated,
}