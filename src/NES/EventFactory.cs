using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using NES.Contracts;

namespace NES
{
    /// <summary>
    /// Based on the MessageMapper implementation in NServiceBus
    /// https://github.com/NServiceBus/NServiceBus/blob/master/src/impl/messageInterfaces/NServiceBus.MessageInterfaces.MessageMapper.Reflection/MessageMapper.cs
    /// </summary>
    public class EventFactory : IEventFactory
    {
        private const string _suffix = ".__Concrete";
        private static readonly Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();
        private static readonly object _cacheLock = new object();
        private readonly ModuleBuilder _moduleBuilder;

        public EventFactory()
        {
            var @namespace = GetType().Namespace + _suffix;
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(@namespace), AssemblyBuilderAccess.Run);

            _moduleBuilder = assemblyBuilder.DefineDynamicModule(@namespace);
        }

        public T Create<T>(Action<T> action)
        {
            var @event = (T)Create(typeof(T));

            action(@event);

            return @event;
        }

        public object Create(Type type)
        {
            return FormatterServices.GetUninitializedObject(GetConcreteType(type));
        }

        private Type GetConcreteType(Type type)
        {
            lock (_cacheLock)
            {
                Type concreteType;

                if (!_cache.TryGetValue(type, out concreteType))
                {
                    _cache[type] = concreteType = CreateType(type);
                }

                return concreteType;
            }
        }

        private Type CreateType(Type type)
        {
            var typeBuilder = _moduleBuilder.DefineType(type.Namespace + _suffix + "." + type.Name, TypeAttributes.Serializable | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, typeof(object));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            typeBuilder.AddInterfaceImplementation(type);

            foreach (var propertyInfo in GetPropertyInfo(type))
            {
                var propertyType = propertyInfo.PropertyType;
                var fieldBuilder = typeBuilder.DefineField("_" + propertyInfo.Name, propertyType, FieldAttributes.Private);
                var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, propertyInfo.Attributes | PropertyAttributes.HasDefault, propertyType, null);
                var getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask, propertyType, Type.EmptyTypes);
                var setMethodBuilder = typeBuilder.DefineMethod("set_" + propertyInfo.Name, getMethodBuilder.Attributes, null, new[] { propertyType });
                var getILGenerator = getMethodBuilder.GetILGenerator();
                var setILGenerator = setMethodBuilder.GetILGenerator();

                getILGenerator.Emit(OpCodes.Ldarg_0);
                getILGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                getILGenerator.Emit(OpCodes.Ret);

                setILGenerator.Emit(OpCodes.Ldarg_0);
                setILGenerator.Emit(OpCodes.Ldarg_1);
                setILGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                setILGenerator.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethodBuilder);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            return typeBuilder.CreateType();
        }

        private IEnumerable<PropertyInfo> GetPropertyInfo(Type type)
        {
            var propertyInfo = new List<PropertyInfo>(type.GetProperties());

            foreach (var subType in type.GetInterfaces())
            {
                propertyInfo.AddRange(GetPropertyInfo(subType));
            }

            return propertyInfo;
        }
    }
}