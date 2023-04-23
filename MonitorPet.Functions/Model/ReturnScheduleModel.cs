using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitorPet.Functions.Model;

internal class ReturnScheduleModel
{
    public DateTime? LastRelease { get; set; } = null;
    public IEnumerable<ScheduleModel> Schedules { get; set; }
        = Enumerable.Empty<ScheduleModel>();
}
