namespace CMS.System.Data.Models
{
    public class MedicaoViewModel
    {
        public string dataSelecionada { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public List<int> Dispositivos { get; set; } = new List<int>();
        public List<int> DispositivosGrupos { get; set; } = new List<int>();
        public int DispositivoSelecionado { get; set; }
        public int GrupoSelecionado { get; set; }
        public int filtro { get; set; } = 2;
        public List<string> categoria { get; set; } = new List<string>();
        public List<MedicaoItensViewModel> Medicoes { get; set; } = new List<MedicaoItensViewModel>();

        public MedicaoViewModel()
        {
            
        }
        public MedicaoViewModel(List<DispositivoMedicao> list)
        {
            ajustaClasse(list, new MedicaoViewModel());
        }

        public MedicaoViewModel(List<DispositivoMedicao> list, MedicaoViewModel model)
        {
            ajustaClasse(list, model);
        }

        #region Ajuste da Classe
        private void ajustaClasse(List<DispositivoMedicao> list, MedicaoViewModel model)
        {
            List<string> dataSet = new List<string>();
            List<MedicaoItensViewModel> ListItens = new List<MedicaoItensViewModel>();
            this.filtro = model.filtro;
            this.dataSelecionada = model.dataSelecionada;
            this.DispositivoSelecionado = model.DispositivoSelecionado;


            var mesesDoAno = Enumerable.Range(1, 12);
            var diasDoMes = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var horasDoDia = Enumerable.Range(0, 24);
            #region FILTRO
            switch (filtro)
            {
                //Filtro de Ano
                case 0:
                    DateTime dataAtual = new DateTime(DateTime.Now.Year, 1, 1);

                    for (int i = 1; i <= 12; i++)
                    {
                        dataSet.Add(dataAtual.ToString("MM"));
                        dataAtual = dataAtual.AddMonths(1);
                    }

                    categoria = dataSet;

                    var medicoesAgrupadasPorMes = list.GroupBy( m => new { m.DT_PUBLICACAO.Year, m.DT_PUBLICACAO.Month });

                    var medicoesComMedia = mesesDoAno
                                                .GroupJoin(
                                                            medicoesAgrupadasPorMes,
                                                            mes => mes,
                                                            grupo => grupo.Key.Month,
                                                            (mes, grupo) => new
                                                            {
                                                                Mes = mes,
                                                                Grupo = grupo.FirstOrDefault(),
                                                            })
                                                .Select(item =>
                                                {
                                                    // Se não houver medições para o mês, retorne valores padrão (0)
                                                    if (item.Grupo == null)
                                                    {
                                                        return new MedicaoItensViewModel
                                                        {
                                                            DT_PUBLICACAO = new DateTime(DateTime.Now.Year, item.Mes, 1),
                                                            VAL_HMD_AMB = "0",
                                                            VAL_TMP_AMB = "0",
                                                            VAL_HMD_SOL = "0",
                                                        };
                                                    }

                                                    float VAL_HMD_AMB = (float)Math.Round(item.Grupo.Average(m => m.VAL_HMD_AMB),2);
                                                    float VAL_TMP_AMB = (float)Math.Round(item.Grupo.Average(m => m.VAL_TMP_AMB), 2);
                                                    float VAL_HMD_SOL = (float)Math.Round(item.Grupo.Average(m => m.VAL_HMD_SOL), 2);

                                                    return new MedicaoItensViewModel
                                                    {
                                                        DT_PUBLICACAO = new DateTime(item.Grupo.Key.Year, item.Grupo.Key.Month, 1),
                                                        VAL_HMD_AMB = VAL_HMD_AMB.ToString(),
                                                        VAL_TMP_AMB = VAL_TMP_AMB.ToString(),
                                                        VAL_HMD_SOL = VAL_HMD_SOL.ToString()
                                                    };
                                                })
                                                .ToList();
                    Medicoes = medicoesComMedia;
                    break;
                //Filtro de Mês
                case 1:
                    DateTime primeiroDiaDoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

                    for (DateTime i = primeiroDiaDoMes; i <= ultimoDiaDoMes; i = i.AddDays(1))
                    {
                        dataSet.Add(i.Day.ToString());
                    }

                    categoria = dataSet;

                    var medicoesAgrupadasPorMes2 = list.GroupBy(m => new { m.DT_PUBLICACAO.Year, m.DT_PUBLICACAO.Month, m.DT_PUBLICACAO.Day });

                    var medicoesComMedia2 = diasDoMes
                                                    .GroupJoin(
                                                                medicoesAgrupadasPorMes2,
                                                                dia => dia,
                                                                grupo => grupo.Key.Day,
                                                                (dia, grupo) => new
                                                                {
                                                                    Dia = dia,
                                                                    Grupo = grupo.FirstOrDefault(),
                                                                }
                                                              )
                                                    .Select(item =>
                                                    {
                                                        // Se não houver medições para o dia, retorne valores padrão (0)
                                                        if (item.Grupo == null)
                                                        {
                                                            return new MedicaoItensViewModel
                                                            {
                                                                DT_PUBLICACAO = new DateTime(DateTime.Now.Year, DateTime.Now.Month, item.Dia),
                                                                VAL_HMD_AMB = "0",
                                                                VAL_TMP_AMB = "0",
                                                                VAL_HMD_SOL = "0",
                                                            };
                                                        }

                                                        float VAL_HMD_AMB = (float)Math.Round(item.Grupo.Average(m => m.VAL_HMD_AMB), 2);
                                                        float VAL_TMP_AMB = (float)Math.Round(item.Grupo.Average(m => m.VAL_TMP_AMB), 2);
                                                        float VAL_HMD_SOL = (float)Math.Round(item.Grupo.Average(m => m.VAL_HMD_SOL), 2);

                                                        return new MedicaoItensViewModel
                                                        {
                                                            DT_PUBLICACAO = new DateTime(item.Grupo.Key.Year, item.Grupo.Key.Month, item.Dia),
                                                            VAL_HMD_AMB = VAL_HMD_AMB.ToString(),
                                                            VAL_TMP_AMB = VAL_TMP_AMB.ToString(),
                                                            VAL_HMD_SOL = VAL_HMD_SOL.ToString()
                                                        };
                                                    })
                                                    .ToList();

                    Medicoes = medicoesComMedia2;
                    break;
                //Filtro de Dia
                case 2:
                    for(int i = 0; i < 24; i++)
                    {
                        dataSet.Add(i.ToString());
                    }
                    categoria = dataSet;

                    var medicoesAgrupadasPorMes3 = list.GroupBy(m => new { m.DT_PUBLICACAO.Year, m.DT_PUBLICACAO.Month, m.DT_PUBLICACAO.Day, m.DT_PUBLICACAO.Hour });

                    var medicoesComMedia3 = horasDoDia
                                                    .GroupJoin(
                                                                medicoesAgrupadasPorMes3,
                                                                hora => hora,
                                                                grupo => grupo.Key.Hour,
                                                                (hora, grupo) => new
                                                                {
                                                                    Hora = hora,
                                                                    Grupo = grupo.FirstOrDefault(),
                                                                })
                                                    .Select(item =>
                                                    {
                                                        // Se não houver medições para a hora, retorne valores padrão (0)
                                                        if (item.Grupo == null)
                                                        {
                                                            return new MedicaoItensViewModel
                                                            {
                                                                DT_PUBLICACAO = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Hora, 0, 0),
                                                                VAL_HMD_AMB = "0",
                                                                VAL_TMP_AMB = "0",
                                                                VAL_HMD_SOL = "0",
                                                            };
                                                        }

                                                        float VAL_HMD_AMB = (float)Math.Round(item.Grupo.Average(m => m.VAL_HMD_AMB), 2);
                                                        float VAL_TMP_AMB = (float)Math.Round(item.Grupo.Average(m => m.VAL_TMP_AMB), 2);
                                                        float VAL_HMD_SOL = (float)Math.Round(item.Grupo.Average(m => m.VAL_HMD_SOL), 2);

                                                        return new MedicaoItensViewModel
                                                        {
                                                            DT_PUBLICACAO = new DateTime(item.Grupo.Key.Year, item.Grupo.Key.Month, item.Grupo.Key.Day, item.Hora, 0, 0),
                                                            VAL_HMD_AMB = VAL_HMD_AMB.ToString(),
                                                            VAL_TMP_AMB = VAL_TMP_AMB.ToString(),
                                                            VAL_HMD_SOL = VAL_HMD_SOL.ToString()
                                                        };
                                                    })
                                                    .ToList();

                    Medicoes = medicoesComMedia3;

                    break;
            }
            #endregion

            Dispositivos = model.Dispositivos;
        }

        #endregion
    }

    public class MedicaoItensViewModel
    {
        public DateTime DT_PUBLICACAO { get; set; }
        public string VAL_HMD_AMB { get; set; }
        public string VAL_TMP_AMB { get; set; }
        public string VAL_HMD_SOL { get; set; }
    }
}
