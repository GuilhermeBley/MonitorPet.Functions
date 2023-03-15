using System;

namespace MonitorPet.Functions.Model;

internal class ScheduleModel
{
    public int IdSchedule { get; set; }
    public string IdDosador { get; set; }
    public int DayOfWeek { get; set; }
    public DateTime ScheduledDate { get; set; }
    public double Quantity { get; set; }
}

