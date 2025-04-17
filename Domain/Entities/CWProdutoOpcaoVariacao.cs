using System.ComponentModel.DataAnnotations.Schema;

public class CWProdutoOpcaoVariacao
{
    public int? nCdProduto { get; set; } = 0;
    public int nCdVariacaoOpcao { get; set; } = 0;
    public int nCdVariacao { get; set; } = 0;

    [NotMapped]
    public string sNmVariacaoOpcao { get; set; } = string.Empty;

    [NotMapped]
    public int bFlAtrelado { get; set; } = 0;
}