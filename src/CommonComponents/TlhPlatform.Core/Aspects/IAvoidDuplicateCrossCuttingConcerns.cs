using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Aspects
{
   public interface IAvoidDuplicateCrossCuttingConcerns
    {
        List<string> AppliedCrossCuttingConcerns { get; }
    }
}
