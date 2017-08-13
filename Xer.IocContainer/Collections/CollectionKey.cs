//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Xer.IocContainer.Collections
//{
//    internal struct CollectionKey : IEquatable<CollectionKey>
//    {
//        public string Name;
//        public Type Type;

//        public CollectionKey(Type type)
//        {
//            Type = type;
//            Name = type.Name;
//        }

//        public bool Equals(CollectionKey other)
//        {
//            return string.Equals(Name, other.Name, StringComparison.Ordinal);
//        }

//        public override bool Equals(object obj)
//        {
//            if(!ReferenceEquals(this, obj))
//            {
//                return false;
//            }

//            if(ReferenceEquals(this, null))
//            {
//                return false;
//            }

//            return Equals((CollectionKey)obj);            
//        }

//        public static bool operator ==(CollectionKey key1, CollectionKey key2)
//        {
//            return key1.Equals(key2);
//        }

//        public static bool operator !=(CollectionKey key1, CollectionKey key2)
//        {
//            return !(key1 == key2);
//        }
//    }
//}
