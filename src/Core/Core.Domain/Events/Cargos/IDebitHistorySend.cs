using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargos;

public interface ICreateDebitHistory
{
    public string CourierId { get; set; }
    public string DebitId { get; set; }
    public string CargoId { get; set; }
    public string CargoItemId { get; set; }
    public string CommandName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}

public class CreateDebitHistory : ICreateDebitHistory
{
    public string CourierId { get; set; }
    public string DebitId { get; set; }
    public string CargoId { get; set; }
    public string CargoItemId { get; set; }
    public string CommandName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}
