namespace CMS.System.Data.Models
{
    public class MedicaoMQTT
    {
        public string ID_DEVICE { get; set; }
        public DateTime DT_PUBLICACAO { get; set; }
        public float VAL_HMD_AMB { get; set; }
        public float VAL_TMP_AMB { get; set; }
        public float VAL_HMD_SOL { get; set; }
    }
}
