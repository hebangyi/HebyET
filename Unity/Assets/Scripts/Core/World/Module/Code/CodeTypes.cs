﻿using System.Collections.Generic;
using System.Reflection;
using System;

namespace ET
{
    public class CodeTypes: Singleton<CodeTypes>, ISingletonAwake<Assembly[]>
    {
        private readonly Dictionary<string, Type> allTypes = new();
        private readonly UnOrderMultiMapSet<Type, Type> attributeTypes = new();
        
        public void Awake(Assembly[] assemblies)
        {
            Dictionary<string, Type> addTypes = AssemblyHelper.GetAssemblyTypes(assemblies);
            foreach ((string fullName, Type type) in addTypes)
            {
                this.allTypes[fullName] = type;
                
                if (type.IsAbstract)
                {
                    continue;
                }
                
                // 记录所有的有BaseAttribute标记的的类型
                object[] objects = type.GetCustomAttributes(typeof(BaseAttribute), true);

                foreach (object o in objects)
                {
                    this.attributeTypes.Add(o.GetType(), type);
                }
            }
        }

        public HashSet<Type> GetAttributeTypes(Type attributeType)
        {
            if (!this.attributeTypes.ContainsKey(attributeType))
            {
                return new HashSet<Type>();
            }

            return this.attributeTypes[attributeType];
        }

        public Dictionary<string, Type> GetAllTypes()
        {
            return allTypes;
        }

        public Type GetType(string typeName)
        {
            return this.allTypes[typeName];
        }
        
        public void CreateCode()
        {
            var hashSet = this.GetAttributeTypes(typeof (CodeAttribute));
            foreach (Type type in hashSet)
            {
                object obj = Activator.CreateInstance(type);
                ((ISingletonAwake)obj).Awake();
                World.Instance.AddSingleton((ASingleton)obj);
            }
        }
    }
}