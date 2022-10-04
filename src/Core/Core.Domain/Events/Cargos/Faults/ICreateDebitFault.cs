using Cargos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargos;

public interface ICreateDebitFault
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}

public class CreateDebitFault : ICreateDebitFault
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}
