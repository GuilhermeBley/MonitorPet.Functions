using System;

namespace MonitorPet.Functions.Model;

internal class ScheduleModel
{
    public int IdSchedule { get; set; }
    public object IdDosador { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan ScheduledDate { get; set; }
    public double Quantity { get; set; }
    public bool Activated { get; set; }
}

