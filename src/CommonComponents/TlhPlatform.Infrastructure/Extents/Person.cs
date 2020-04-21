using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Infrastructure.AOP;

namespace TlhPlatform.Infrastructure.Extents
{
    public class Person
    {
        [CustomInterceptor]
        public virtual void Say(string message)
        {
            Console.WriteLine("service calling ..."+ message);
        }
    }
}
