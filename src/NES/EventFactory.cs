// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   Based on the MessageMapper implementation in NServiceBus
//   https://github.com/NServiceBus/NServiceBus/blob/master/src/impl/messageInterfaces/NServiceBus.MessageInterfaces.MessageMapper.Reflection/MessageMapper.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    /// <summary>
    ///     Based on the MessageMapper implementation in NServiceBus
    ///     https://github.com/NServiceBus/NServiceBus/blob/master/src/impl/messageInterfaces/NServiceBus.MessageInterfaces.MessageMapper.Reflection/MessageMapper.cs
    /// </summary>
    public class EventFactory : IEventFactory
    {
        #region Constants

        private const string _suffix = ".__Concrete";

        #endregion

        #region Static Fields

        private static readonly Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();

        private static readonly object _cacheLock = new object();

        #endregion

        #region Fields

        private readonly ModuleBuilder _moduleBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventFactory" /> class.
        /// </summary>
        public EventFactory()
        {
            var @namespace = this.GetType().Namespace + _suffix;
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(@namespace), AssemblyBuilderAccess.Run);

            this._moduleBuilder = assemblyBuilder.DefineDynamicModule(@namespace);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The action which will be applied to event instance
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Create<T>(Action<T> action)
        {
            var @event = (T)this.Create(typeof(T));

            action(@event);

            return @event;
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Create(Type type)
        {
            return FormatterServices.GetUninitializedObject(this.GetConcreteType(type));
        }

        #endregion

        #region Methods

        private Type CreateType(Type type)
        {
            var typeBuilder = this._moduleBuilder.DefineType(
                type.Namespace + _suffix + "." + type.Name, 
                TypeAttributes.Serializable | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, 
                typeof(object));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            typeBuilder.AddInterfaceImplementation(type);

            foreach (var propertyInfo in this.GetPropertyInfo(type))
            {
                var propertyType = propertyInfo.PropertyType;
                var fieldBuilder = typeBuilder.DefineField(string.Format("_{0}", propertyInfo.Name), propertyType, FieldAttributes.Private);
                var propertyBuilder = typeBuilder.DefineProperty(
                    propertyInfo.Name, 
                    propertyInfo.Attributes | PropertyAttributes.HasDefault, 
                    propertyType, 
                    null);
                var getMethodBuilder = typeBuilder.DefineMethod(
                    string.Format("get_{0}", propertyInfo.Name), 
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Final
                    | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask, 
                    propertyType, 
                    Type.EmptyTypes);
                var setMethodBuilder = typeBuilder.DefineMethod(
                    string.Format("set_{0}", propertyInfo.Name), 
                    getMethodBuilder.Attributes, 
                    null, 
                    new[] { propertyType });
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

        private Type GetConcreteType(Type type)
        {
            lock (_cacheLock)
            {
                Type concreteType;

                if (!_cache.TryGetValue(type, out concreteType))
                {
                    _cache[type] = concreteType = this.CreateType(type);
                }

                return concreteType;
            }
        }

        private IEnumerable<PropertyInfo> GetPropertyInfo(Type type)
        {
            var propertyInfo = new List<PropertyInfo>(type.GetProperties());

            foreach (var subType in type.GetInterfaces())
            {
                propertyInfo.AddRange(this.GetPropertyInfo(subType));
            }

            return propertyInfo;
        }

        #endregion
    }
}