namespace EntityFramework.Extensions.Annotations
{
    using System;

    [Flags]
    public enum TriggerEventEnum
    {
        Insert = 1,
        Delete = 2,
        Update = 4
    }
}