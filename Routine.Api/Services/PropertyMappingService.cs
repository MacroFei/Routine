﻿using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
                { "Id",new PropertyMappingValue(new List<string>{ "id"})},
                { "CompanyId",new PropertyMappingValue(new List<string>{ "CompanyId"})},
                { "EmployeeNo",new PropertyMappingValue(new List<string>{ "EmployeeNo"})},
                { "Name",new PropertyMappingValue(new List<string>{ "FirstName","LastName"})},
                { "GenderDisplay",new PropertyMappingValue(new List<string>{ "Gender"})},
                { "Age",new PropertyMappingValue(new List<string>{ "DateOfBirth"},true)}
        };
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }
        /// <summary>
        /// 取出映射关系
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count == 1)
            {
                return propertyMappings.First().MappingDictionary;
            }
            throw new Exception($"无法找到唯一的映射关系:{typeof(TSource)},{typeof(TDestination)}");
        }
    }

}