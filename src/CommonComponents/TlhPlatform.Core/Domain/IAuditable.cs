using System;

namespace TlhPlatform.Core.Domain
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
    }
}
