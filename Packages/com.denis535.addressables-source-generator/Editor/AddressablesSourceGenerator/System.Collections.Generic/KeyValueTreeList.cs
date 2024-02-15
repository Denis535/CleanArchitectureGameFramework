#nullable enable
namespace System.Collections.Generic {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class KeyValueTreeList {

        // Create
        public static KeyValueTreeList<T> Create<T>(IEnumerable<(string[] Keys, T Value)> items) {
            var treeList = new KeyValueTreeList<T>();
            foreach (var (keys, value) in items) {
                treeList.AddValue( keys.SkipLast( 1 ).ToArray(), keys.Last(), value );
            }
            return treeList;
        }

        // Create
        public static KeyValueTreeList<T> Create<T>(IEnumerable<(string[] Keys, string Key, T Value)> items) {
            var treeList = new KeyValueTreeList<T>();
            foreach (var (keys, key, value) in items) {
                treeList.AddValue( keys, key, value );
            }
            return treeList;
        }

    }
    internal interface IKeyValueTreeList<T> {
        List<KeyValueTreeList<T>.Item> Items { get; }
    }
    public class KeyValueTreeList<T> : IKeyValueTreeList<T> {
        // Item
        public abstract class Item {
            public string Key { get; }
            public Item(string key) {
                Key = key;
            }
        }
        // ValueItem
        public class ValueItem : Item {
            public T Value { get; }
            public ValueItem(string key, T value) : base( key ) {
                Value = value;
            }
        }
        // ListItem
        public class ListItem : Item, IKeyValueTreeList<T> {
            public List<KeyValueTreeList<T>.Item> Items { get; } = new List<KeyValueTreeList<T>.Item>( 0 );
            public ListItem(string key) : base( key ) {
            }
        }

        public List<KeyValueTreeList<T>.Item> Items { get; } = new List<Item>();

        // Constructor
        public KeyValueTreeList() {
        }

        // GetValue
        public T? GetValue(IEnumerable<string> keys, string key) {
            var list = this.GetList( keys );
            if (list != null) {
                var result = list.Items.OfType<ValueItem>().Where( i => i.Key == key ).Select( i => i.Value ).FirstOrDefault();
                return result;
            }
            return default;
        }
        public T[]? GetValues(IEnumerable<string> keys, string key) {
            var list = this.GetList( keys );
            if (list != null) {
                var result = list.Items.OfType<ValueItem>().Where( i => i.Key == key ).Select( i => i.Value ).ToArray();
                return result.Any() ? result : null;
            }
            return default;
        }
        // AddValue
        public void AddValue(IEnumerable<string> keys, string key, T value) {
            var list = this.GetOrAddList( keys );
            list.Items.Add( new ValueItem( key, value ) );
        }
        public void AddValues(IEnumerable<string> keys, string key, params T[] values) {
            var list = this.GetOrAddList( keys );
            foreach (var value in values) {
                list.Items.Add( new ValueItem( key, value ) );
            }
        }
        // RemoveAll
        public int RemoveAll(IEnumerable<string> keys, string key) {
            var list = this.GetList( keys );
            if (list != null) {
                return list.Items.RemoveAll( i => i.Key == key );
            }
            return 0;
        }

        // Sort
        public void Sort(Comparison<Item> comparison) {
            this.Sort_( comparison );
        }

        // Utils
        public override string ToString() {
            var builder = new StringBuilder();
            builder.AppendObject( this );
            return builder.ToString();
        }

    }
    internal static class KeyValueTreeListHelper {

        // GetList
        public static IKeyValueTreeList<T>? GetList<T>(this IKeyValueTreeList<T> list, IEnumerable<string> keys) {
            var result = list;
            foreach (var key in keys) {
                result = result.GetList( key );
                if (result == null) return null;
            }
            return result;
        }
        public static IKeyValueTreeList<T> GetOrAddList<T>(this IKeyValueTreeList<T> list, IEnumerable<string> keys) {
            var result = list;
            foreach (var key in keys) {
                result = result.GetOrAddList( key );
            }
            return result;
        }

        // GetList
        private static IKeyValueTreeList<T>? GetList<T>(this IKeyValueTreeList<T> list, string key) {
            var result = list.Items.OfType<KeyValueTreeList<T>.ListItem>().Where( i => i.Key == key ).FirstOrDefault();
            return result;
        }
        private static IKeyValueTreeList<T> GetOrAddList<T>(this IKeyValueTreeList<T> list, string key) {
            var result = list.Items.OfType<KeyValueTreeList<T>.ListItem>().Where( i => i.Key == key ).FirstOrDefault();
            if (result == null) {
                list.Items.Add( result = new KeyValueTreeList<T>.ListItem( key ) );
            }
            return result;
        }

        // Sort
        public static void Sort_<T>(this IKeyValueTreeList<T> list, Comparison<KeyValueTreeList<T>.Item> comparison) {
            list.Items.Sort( comparison );
            foreach (var item in list.Items.OfType<IKeyValueTreeList<T>>()) {
                item.Sort_( comparison );
            }
        }

        // AppendObject
        public static void AppendObject<T>(this StringBuilder builder, KeyValueTreeList<T> treeList) {
            builder.Append( "KeyValueTreeList:" );
            foreach (var item in treeList.Items) {
                if (item is KeyValueTreeList<T>.ValueItem value) {
                    builder.AppendObject( value.Key, value );
                } else
                if (item is KeyValueTreeList<T>.ListItem list) {
                    builder.AppendObject( list.Key, list );
                }
            }
        }
        private static void AppendObject<T>(this StringBuilder builder, string path, KeyValueTreeList<T>.ValueItem value) {
            builder.AppendLine();
            builder.Append( path ).Append( ": " ).Append( value.Value );
        }
        private static void AppendObject<T>(this StringBuilder builder, string path, KeyValueTreeList<T>.ListItem list) {
            foreach (var item in list.Items) {
                if (item is KeyValueTreeList<T>.ValueItem value) {
                    builder.AppendObject( $"{path}/{value.Key}", value );
                } else
                if (item is KeyValueTreeList<T>.ListItem list_) {
                    builder.AppendObject( $"{path}/{list_.Key}", list_ );
                }
            }
        }

    }
}
