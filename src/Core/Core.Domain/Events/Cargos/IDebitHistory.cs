using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargos;

public interface IDebitHistory
{
    public string UserId { get; set; }
    public string DebitId { get; set; }
    public string CargoId { get; set; }
    public string CargoItemId { get; set; }
    public string CommandName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}

public class DebitHistory : IDebitHistory
{
    public string UserId { get; set; }
    public string DebitId { get; set; }
    public string CargoId { get; set; }
    public string CargoItemId { get; set; }
    public string CommandName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}
