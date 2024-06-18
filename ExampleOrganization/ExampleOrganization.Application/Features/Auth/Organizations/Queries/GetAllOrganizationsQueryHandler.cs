using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using TS.Result;

namespace ExampleOrganization.Application.Features.Auth.Organizations.Queries
{
    internal sealed class GetAllOrganizationsQueryHandler(IOrganizationRepository organizationRepository) : IRequestHandler<GetAllOrganizationsQuery, Result<List<Organization>>>
    {
        public async Task<Result<List<Organization>>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var result = await organizationRepository.GetAllOrganizationWithParent().ToListAsync();
            foreach (var organization in result)
            {
                PopulateAllChildren(organization);
            }
          
            return result;
        }

        private void PopulateAllChildren(Organization organization)
        {
            // Eğer organizasyonun çocuk birimleri varsa
            if (organization.Children.Any())
            {
                // Yeni bir liste oluştur ve tüm çocuk birimleri bu listeye ekle
                var allChildren = new List<Organization>();
                PopulateAllChildrenRecursive(organization, allChildren);

                //// Organizasyonun AllChildren property'sine oluşturduğumuz listeyi ata
                organization.AllChildren = allChildren;
            }
        }

        private void PopulateAllChildrenRecursive(Organization organization, List<Organization> allChildren)
        {
            // Önce mevcut organizasyonu listeye ekle
            allChildren.Add(organization);

            // Ardından, organizasyonun çocuk birimlerini dolaş
            foreach (var child in organization.Children)
            {
                // Her bir çocuk birim için rekursif olarak bu metodu çağır
                // ve çocuk birimleri de listeye ekle
                PopulateAllChildrenRecursive(child, allChildren);
            }
        }

        
    }

}