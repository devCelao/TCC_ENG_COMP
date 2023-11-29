using CMS.System.Data.Models;

namespace CMS.System.Data.Repositories
{
    public interface IDispositivoRepository
    {
        /// <summary>
        /// Métodos referente ao cadastro dos dispositivos
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        
        Task AddDispositivo(Dispositivo device);
        Task UpdateDispositivo(Dispositivo device);
        Task RemoveDispositivo(string id);
        Task<Dispositivo> GetOnlyOneDevice(string ID_DEVICE);
        Task<List<Dispositivo>> GetDispositivosById(string ID_DEVICE = null);
        Task<List<Dispositivo>> GetDispositivosByGroup(int ID_GROUP);

        /// <summary>
        /// Métodos referente as medições dos dispositivos
        /// </summary>
        /// <param name="medicao"></param>
        /// <returns></returns>
        Task AddMedicao(string dispositivoId, DispositivoMedicao medicao);
        Task LimpaMedicoes(string device_id);
        Task LimpaMedicoesByRef(DateTime init, DateTime fim);
        Task LimpaMedicoesByGroup(int ID_GROUP);
        Task<List<DispositivoMedicao>> GetMedicoes();
        Task<List<DispositivoMedicao>> GetMedicoesById(string device_id);
        Task<List<DispositivoMedicao>> GetMedicoesByRef(DateTime init, DateTime fim);
        Task<List<DispositivoMedicao>> GetMedicoesByGroup(int ID_GROUP);
    }
}
