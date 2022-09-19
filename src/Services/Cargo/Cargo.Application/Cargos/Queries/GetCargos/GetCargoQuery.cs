using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.Queries.GetCargos;

public class GetCargoQuery : IRequest<GenericResponse<List<GetCargosResponse>>>
{
    public string CorrelationId { get; set; }
}


public class GetCargoQueryHandler : IRequestHandler<GetCargoQuery, GenericResponse<List<GetCargosResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCargoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<GetCargosResponse>>> Handle(GetCargoQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Cargos
            .Include(x => x.CargoItems)
            .Where(x => x.Debit.CorrelationId == request.CorrelationId)
            .Select(x=> new GetCargosResponse
            {
                CargoId = x.CargoId,
                Address = x.Address,
                DebitId = x.DebitId,
                CargoItems = x.CargoItems.Select(y => new GetCargoItem
                {
                    Barcode = y.Barcode,
                    CargoItemId = y.CargoItemId,
                    Description = y.Description,
                    Desi = y.Desi,
                    Kg = y.Kg,
                    WaybillNumber = y.Barcode
                }).ToList()
            })
            .ToListAsync();

        // var data = _mapper.Map<List<GetCargosResponse>>(response);
        return GenericResponse<List<GetCargosResponse>>.Success(response, 200);
    }
}