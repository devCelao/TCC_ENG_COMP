namespace CMS.System.Data.Models
{
    public class DispositivoMedicao
    {
        public Guid Id { get; set; }
        public string ID_DEVICE { get; set; }
        public DateTime DT_PUBLICACAO { get; set; }
        public float VAL_HMD_AMB { get; set; }
        public float VAL_TMP_AMB { get; set; }
        public float VAL_HMD_SOL { get; set; }

        public DispositivoMedicao() {}
        public DispositivoMedicao(MedicaoMQTT medicao)
        {
            ID_DEVICE = medicao.ID_DEVICE;
            DT_PUBLICACAO = medicao.DT_PUBLICACAO;
            VAL_HMD_AMB = medicao.VAL_HMD_AMB;
            VAL_TMP_AMB = medicao.VAL_TMP_AMB;
            VAL_HMD_SOL = medicao.VAL_HMD_SOL;
        }
    }
}
