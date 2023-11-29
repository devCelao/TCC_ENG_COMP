using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace CMS.System.Data.Models
{
    public class Dispositivo
    {
        [Key]
        public string ID_DEVICE { get; set; }
        public int ID_GROUP { get; set; }
        public int FREQUENCY { get; set; }
        public DateTime DT_CADASTRO { get; set; }
        public DateTime DT_ALTERACAO { get; set; }
        public string COD_USUARIO_ALTERACAO { get; set; }
        public int IND_ATIVO_HMD_AMB { get; set; }
        public int IND_ATIVO_TMP_AMB { get; set; }
        public int IND_ATIVO_HMD_SOL { get; set; }
        public int IND_ATIVO { get; set; }

        public Dispositivo(){}


        public Dispositivo(string ID_DEVICE) 
        {
            this.ID_DEVICE = ID_DEVICE;
        }
    }
}
