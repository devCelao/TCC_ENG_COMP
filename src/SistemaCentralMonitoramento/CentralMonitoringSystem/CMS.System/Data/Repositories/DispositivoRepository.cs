using CMS.System.Data.Context;
using CMS.System.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CMS.System.Data.Repositories
{
    public class DispositivoRepository : IDispositivoRepository
    {
        private readonly DispositivoContext context;

        private readonly MedicaoContext medicaoContext;
        public DispositivoRepository(DispositivoContext context, MedicaoContext medicaoContext)
        {
            this.context = context;
            this.medicaoContext = medicaoContext;
        }
        
        #region Dispositivo
        public async Task AddDispositivo(Dispositivo device)
        {
            context.Dispositivos.Add(device);

            await context.SaveChangesAsync();
        }

        public async Task UpdateDispositivo(Dispositivo device)
        {
            var atual = await GetDispositivosById(device.ID_DEVICE);

            if (!atual.Any())
            {
                await AddDispositivo(device);
            }
            // Enviar comando com a atualização das informações.
            context.Dispositivos.Update(device);

            await context.SaveChangesAsync();
        }

        public async Task RemoveDispositivo(string id)
        {
            var atual = await GetDispositivos(id);

            if (atual.Any())
            {
                context.Dispositivos.Remove(atual.FirstOrDefault());
            }

            await context.SaveChangesAsync();
        }


        public async Task<List<Dispositivo>> GetDispositivosById(string ID_DEVICE = null)
        {

            if(ID_DEVICE is not null)
            {
                return await GetDispositivos(ID_DEVICE);
            }
            
            return await context.Dispositivos.ToListAsync();

        }

        public async Task<Dispositivo> GetOnlyOneDevice(string ID_DEVICE)
        {
            return context.Dispositivos.Where(e => e.ID_DEVICE == ID_DEVICE).FirstOrDefault() ?? new Dispositivo();
        }

        public async Task<List<Dispositivo>> GetDispositivosByGroup(int ID_GROUP)
        {
            return await context.Dispositivos
                    .AsNoTracking()
                    .Where(d => d.ID_GROUP == ID_GROUP)
                    .ToListAsync();
        }


        private async Task<List<Dispositivo>> GetDispositivos(string ID_DEVICE)
        {
            return await context.Dispositivos
                                .AsNoTracking()
                                .Where(d => d.ID_DEVICE == ID_DEVICE)
                                .ToListAsync();
        }

        #endregion
        #region Medição

        public async Task AddMedicao(string dispositivoId, DispositivoMedicao medicao)
        {

            var dispositivo = await context.Dispositivos
                                        .FirstOrDefaultAsync(d => d.ID_DEVICE == dispositivoId);

            if (dispositivo == null)
                ManipulaNovoDispositivo(medicao);
            else
                ManipulaDispositivoExistente(medicao);
        }

        private void ManipulaNovoDispositivo(DispositivoMedicao medicao)
        {
            var dispositivo = new Dispositivo(medicao.ID_DEVICE);
            
            context.Dispositivos.Add(dispositivo);

            context.SaveChangesAsync();

            ManipulaDispositivoExistente( medicao );
        }

        private void ManipulaDispositivoExistente(DispositivoMedicao medicao)
        {

            try
            {
                medicaoContext.Medicoes.Add(medicao);

                medicaoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        public async Task LimpaMedicoes(string device_id)
        {
            var medicoes = await GetMedicoesById(device_id);

            if (medicoes.Any())
                medicaoContext.Medicoes.RemoveRange(medicoes);
        }

        public async Task LimpaMedicoesByGroup(int ID_GROUP)
        {
            var disps = await GetMedicoesByGroup(ID_GROUP);

            if (disps.Any())
            {
                foreach (var disp in disps)
                {
                    await LimpaMedicoes(disp.ID_DEVICE);
                }
            }
        }

        public async Task LimpaMedicoesByRef(DateTime init, DateTime fim)
        {
            var disps = await GetMedicoesByRef(init, fim);
            if (disps.Any())
            {
                foreach (var disp in disps)
                {
                    await LimpaMedicoes(disp.ID_DEVICE);
                }
            }
        }

        public async Task<List<DispositivoMedicao>> GetMedicoes()
        {
            return await medicaoContext.Medicoes
                   .AsNoTracking()
                   .ToListAsync();
        }

        public async Task<List<DispositivoMedicao>> GetMedicoesById(string device_id)
        {
            return await medicaoContext.Medicoes
                   .AsNoTracking()
                   .Where(d => d.ID_DEVICE == device_id)
                   .ToListAsync();
        }

        public async Task<List<DispositivoMedicao>> GetMedicoesByRef(DateTime init, DateTime fim)
        {
            return await medicaoContext.Medicoes
                   .AsNoTracking()
                   .Where(d => d.DT_PUBLICACAO >= init && d.DT_PUBLICACAO <= fim)
                   .ToListAsync();
        }

        public async Task<List<DispositivoMedicao>> GetMedicoesByGroup(int ID_GROUP)
        {
            var list = new List<DispositivoMedicao>();
            var disps = await context.Dispositivos
                                    .AsNoTracking()
                                    .Where(d => d.ID_GROUP == ID_GROUP)
                                    .ToListAsync();
            if (disps.Any())
            {
                foreach (var disp in disps)
                {
                    var sublist = medicaoContext.Medicoes.Where(e => e.ID_DEVICE == disp.ID_DEVICE).ToList();
                    if (sublist.Any())
                        list.AddRange(sublist);
                }
            }
            return list;
        }



        #endregion

    }
}
