using AutoMapper;

namespace Cargo.Application.Cargos.Queries.GetCargoLists;

public class GetCargoListQuery : IRequest<GenericResponse<List<GetCargoListResponse>>>
{
    public string CorrelationId { get; set; }
}

public class GetCargoListQueryHandler : IRequestHandler<GetCargoListQuery, GenericResponse<List<GetCargoListResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCargoListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<GetCargoListResponse>>> Handle(GetCargoListQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Cargos
            .Where(x => x.Debit.CorrelationId == request.CorrelationId)
            .Select(x => new GetCargoListResponse
            {
                CargoId = x.CargoId,
                Address = x.Address,
                DebitId = x.DebitId
            })
            .ToListAsync();

        return GenericResponse<List<GetCargoListResponse>>.Success(response, 200);
    }
}