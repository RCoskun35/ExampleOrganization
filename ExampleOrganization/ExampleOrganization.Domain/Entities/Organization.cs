﻿using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Services;

namespace ExampleOrganization.Domain.Entities
{
    public class Organization: IBaseHierarchyEntity
    {
        public int Id { get; set; }
        public string Name{ get; set; }=string.Empty;
        public int? ParentId { get; set; }
        public Organization? Parent { get; set; }

    }
}
