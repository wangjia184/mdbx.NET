using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public static class SerializerRegistry
    {
        private static readonly Dictionary<Type, object> _dic = new Dictionary<Type, object>()
        {
            {  typeof(string), new StringSerializer() },
            {  typeof(int), new IntSerializer() },
            {  typeof(long), new LongSerializer() },
            { typeof(byte[]), new ByteArraySerializer() },
        };

        /// <summary>
        /// Register a serializer of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        public static void Register<T>(ISerializer<T> serializer)
        {
            _dic[typeof(T)] = serializer;
        }

        internal static ISerializer<T> Get<T>()
        {
            object obj;
            _dic.TryGetValue(typeof(T), out obj);
            ISerializer<T> serializer = obj as ISerializer<T>;
            if( serializer == null)
            {
                throw new KeyNotFoundException($"Unable to find serializer of {typeof(T).Name}, please use `SerializerRegistry.Register` to register.");
            }
            return serializer;
        }


    }
}
