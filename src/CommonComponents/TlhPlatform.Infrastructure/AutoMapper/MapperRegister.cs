using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TlhPlatform.Infrastructure.AutoMapper
{
    public static class MapperRegister
    {
        public static Type[] MapType()
        {

            var allIem = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().Select(Assembly.Load)
                .SelectMany(y => y.DefinedTypes)
                .Where(type => typeof(IProfile).GetTypeInfo().IsAssignableFrom(type.AsType()));
            List<Type> allList = new List<Type>();
            if (allIem != null)
            {
                foreach (var typeinfo in allIem)
                {
                    var type = typeinfo.AsType();
                    allList.Add(type);
                }
            } 
            Type[] alltypes = allList.ToArray();
            return alltypes;

        }
    }
}
