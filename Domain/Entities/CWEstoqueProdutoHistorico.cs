using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum nTipoMovimentacao : int
{
    Entrada = 1,
    Saida = 2,
    Devolucao = 3,
    Transferencia = 4
}

public class CWEstoqueProdutoHistorico
{
    [Key, Column(Order = 0)]
    public int nCdEstoqueProdutoHistorico { get; set; }
    [Column(Order = 1)]
    public int nCdEstoque { get; set; }
    [Column(Order = 2)]
    public int nCdProduto { get; set; }
    public DateTime? tDtMovimentacao { get; set; }
    public int dQtMovimentada { get; set; }
    public nTipoMovimentacao nTipoMovimentacao { get; set; }
    public string? sDsObservacao { get; set; }
    public int nCdEstoqueDestino { get; set; }
    public CWProduto? Produto { get; set; }
    public CWEstoque? EstoqueOrigem { get; set; }
    public CWEstoque? EstoqueDestino { get; set; }

}