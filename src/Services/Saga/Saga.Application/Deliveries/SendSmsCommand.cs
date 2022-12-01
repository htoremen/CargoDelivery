
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Application.Deliveries;

public class SendSmsCommand : ISendSms
{
    public SendSmsCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
