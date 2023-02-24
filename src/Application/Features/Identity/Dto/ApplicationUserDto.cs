﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Blazor.Application.Features.Documents.DTOs;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Application.Features.Identity.Dto;
public class ApplicationUserDto:IMapFrom<ApplicationUser>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationUser, ApplicationUserDto>(MemberList.None)
           .ForMember(x => x.SuperiorName, s => s.MapFrom(y => y.Superior.UserName));
         
    }
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Provider { get; set; } = "Local";
    public string? TenantId { get; set; }
    public string? TenantName { get; set; }
    public string? ProfilePictureDataUrl { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? SuperiorId { get; set; }
    public string? SuperiorName { get; set; }
}
