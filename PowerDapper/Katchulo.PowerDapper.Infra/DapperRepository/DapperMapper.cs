﻿using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Katchulo.PowerDapper.Infra.DapperRepository
{
    public class DapperMapper
    {
        public DapperMapper()
        {

        }

        public void SetMapper<T>()
        {
            var oldMap = SqlMapper.GetTypeMap(typeof(T));
            var map = new CustomTypeMap(typeof(T), oldMap);
            
            typeof(T).GetProperties().ToList().ForEach(prop =>
            {
                var attrs = prop.GetCustomAttributes(false).OfType<ColumnAttribute>();
                if (attrs.Count() > 0)
                {
                    map.Map(attrs.FirstOrDefault().Name, prop.Name);
                }
            });
            SqlMapper.SetTypeMap(map.Type, map);

        }
    }

    class CustomTypeMap : SqlMapper.ITypeMap
    {
        private readonly Dictionary<string, SqlMapper.IMemberMap> members
            = new Dictionary<string, SqlMapper.IMemberMap>(StringComparer.OrdinalIgnoreCase);
        public Type Type { get { return type; } }
        private readonly Type type;
        private readonly SqlMapper.ITypeMap tail;
        public void Map(string columnName, string memberName)
        {
            members[columnName] = new MemberMap(type.GetMember(memberName).Single(), columnName);
        }
        public CustomTypeMap(Type type, SqlMapper.ITypeMap tail)
        {
            this.type = type;
            this.tail = tail;
        }
        public System.Reflection.ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return tail.FindConstructor(names, types);
        }

        public SqlMapper.IMemberMap GetConstructorParameter(
            System.Reflection.ConstructorInfo constructor, string columnName)
        {
            return tail.GetConstructorParameter(constructor, columnName);
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            SqlMapper.IMemberMap map;
            if (!members.TryGetValue(columnName, out map))
            { // you might want to return null if you prefer not to fallback to the
                // default implementation
                map = tail.GetMember(columnName);
            }
            return map;
        }
    }
    class MemberMap : SqlMapper.IMemberMap
    {
        private readonly MemberInfo member;
        private readonly string columnName;
        public MemberMap(MemberInfo member, string columnName)
        {
            this.member = member;
            this.columnName = columnName;
        }
        public string ColumnName { get { return columnName; } }
        public System.Reflection.FieldInfo Field { get { return member as FieldInfo; } }
        public Type MemberType
        {
            get
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field: return ((FieldInfo)member).FieldType;
                    case MemberTypes.Property: return ((PropertyInfo)member).PropertyType;
                    default: throw new NotSupportedException();
                }
            }
        }
        public System.Reflection.ParameterInfo Parameter { get { return null; } }
        public System.Reflection.PropertyInfo Property { get { return member as PropertyInfo; } }
    }
}
