namespace Domain.Entities
{
    public class CWProdutoOpcaoVariacaoBase
    {
        public int? nCdProduto { get; set; } = 0; 
        public int nCdVariacaoOpcao { get; set; } = 0; 
        public int nCdVariacao { get; set; } = 0;
    }

    public class CWProdutoOpcaoVariacao : CWProdutoOpcaoVariacaoBase
    {
        public string sNmVariacaoOpcao { get; set; }
        public int bFlAtrelado { get; set; }

        public CWProdutoOpcaoVariacao()
        {
            this.nCdVariacaoOpcao = 0;
            this.nCdProduto = 0;
            this.nCdVariacao = 0;
            this.sNmVariacaoOpcao = string.Empty; ;
            this.bFlAtrelado = 0;
        }
    }
}