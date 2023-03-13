using System.Threading.Tasks;

namespace MonitorPet.Functions.Repository;

internal interface IPesoRepository
{
    Task Create(Model.WeightDosador model);
}