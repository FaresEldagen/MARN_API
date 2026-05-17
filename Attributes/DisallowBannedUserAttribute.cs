namespace MARN_API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DisallowBannedUserAttribute : Attribute
    {
    }
}
