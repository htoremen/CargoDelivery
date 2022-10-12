using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargo.Application.Telemetry;

public static class ConsumerActivitySource
{
    public static readonly ActivitySource Source = OpenTelemetryExtensions.CreateActivitySource();
}

