namespace Domain.Entities
{
    public class CWVariacaoOpcao
    {
        public int nCdVariacaoOpcao { get; set; }
        public string sNmVariacaoOpcao { get; set; }
        public string sDsVariacaoOpcao { get; set; }
        public bool bFlAtiva { get; set; }
        public ICollection<CWVariacao> Variacoes { get; set; }
    }
}
