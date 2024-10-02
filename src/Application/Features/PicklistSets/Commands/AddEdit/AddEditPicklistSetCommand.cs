﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.PicklistSets.Caching;
using CleanArchitecture.Blazor.Application.Features.PicklistSets.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.PicklistSets.Commands.AddEdit;

public class AddEditPicklistSetCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }

    [Description("Name")] public Picklist Name { get; set; }

    [Description("Value")] public string? Value { get; set; }

    [Description("Text")] public string? Text { get; set; }

    [Description("Description")] public string? Description { get; set; }

    public TrackingState TrackingState { get; set; } = TrackingState.Unchanged;
    public string CacheKey => PicklistSetCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => PicklistSetCacheKey.GetOrCreateTokenSource();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PicklistSetDto, AddEditPicklistSetCommand>(MemberList.None);
            CreateMap<AddEditPicklistSetCommand, PicklistSet>(MemberList.None);
        }
    }
}

public class AddEditPicklistSetCommandHandler : IRequestHandler<AddEditPicklistSetCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddEditPicklistSetCommandHandler(
        IApplicationDbContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddEditPicklistSetCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var keyValue = await _context.PicklistSets.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = keyValue ?? throw new NotFoundException($"KeyValue Pair  {request.Id} Not Found.");
            keyValue = _mapper.Map(request, keyValue);
            keyValue.AddDomainEvent(new UpdatedEvent<PicklistSet>(keyValue));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(keyValue.Id);
        }
        else
        {
            var keyValue = _mapper.Map<PicklistSet>(request);
            keyValue.AddDomainEvent(new UpdatedEvent<PicklistSet>(keyValue));
            _context.PicklistSets.Add(keyValue);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(keyValue.Id);
        }
    }
}