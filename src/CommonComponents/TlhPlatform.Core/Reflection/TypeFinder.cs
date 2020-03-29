using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TlhPlatform.Core.Extensions;

namespace TlhPlatform.Core.Reflection
{
    public class TypeFinder : ITypeFinder
    {
 
        private readonly IAssemblyFinder _assemblyFinder;
        private readonly object _syncObj = new object();
        private Type[] _types;

        public TypeFinder(IAssemblyFinder assemblyFinder)
        {
            _assemblyFinder = assemblyFinder;
         
        }

        public Type[] Find(Func<Type, bool> predicate, bool fromCache = false)
        {
            return GetAllTypes().Where(predicate).ToArray();
        }

        public Type[] FindAll(bool formCache = false)
        {
            return GetAllTypes().ToArray();
        }

        private Type[] GetAllTypes()
        {
            if (_types == null)
            {
                lock (_syncObj)
                {
                    if (_types == null)
                    {
                        _types = CreateTypeList().ToArray();
                    }
                }
            }

            return _types;
        }

        private List<Type> CreateTypeList()
        {
            var allTypes = new List<Type>();

            var assemblies = _assemblyFinder.GetAllAssemblies().Distinct();

            foreach (var assembly in assemblies)
            {
                try
                {
                    Type[] typesInThisAssembly;

                    try
                    {
                        typesInThisAssembly = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        typesInThisAssembly = ex.Types;
                    }

                    if (typesInThisAssembly.IsNullOrEmpty())
                    {
                        continue;
                    }

                    allTypes.AddRange(typesInThisAssembly.Where(type => type != null));
                }
                catch (Exception ex)
                { 
                   
                }
            }

            return allTypes;
        }

 
    }
}
