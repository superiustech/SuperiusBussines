using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace Infra.Repositories.Persistencia
{
    public class ProdutoRepositorySQL : IProdutoRepositorySQL
    {
        public ProdutoRepositorySQL(){}
        
        public string AtualizarProdutoSQL()
        {
            var sSql = @"UPDATE PRODUTO SET
                sNmProduto = @sNmProduto, sDsProduto = @sDsProduto, sCdProduto = @sCdProduto, sUrlVideo = @sUrlVideo,
                sLargura = @sLargura, sComprimento = @sComprimento, sAltura = @sAltura, sPeso = @sPeso, dVlVenda = @dVlVenda,
                dVlUnitario = @dVlUnitario,nCdUnidadeMedida = @nCdUnidadeMedida WHERE nCdProduto = @nCdProduto;";
            return sSql;
        }
        public string ConsultarProdutoVariacaoSQL()
        {
            var sSql = @"WITH PRODUTO_VARIACAO AS (
                SELECT VO.nCdVariacaoOpcao, VO.sNmVariacaoOpcao, V.nCdVariacao,CASE WHEN POV.nCdProduto IS NOT NULL THEN 1 ELSE 0 END AS bFlAtrelado FROM VARIACAO V
                INNER JOIN VARIACAO_OPCAO_VARIACAO VOV ON VOV.nCdVariacao = V.nCdVariacao
                INNER JOIN VARIACAO_OPCAO VO ON VO.nCdVariacaoOpcao = VOV.nCdVariacaoOpcao
                LEFT JOIN PRODUTO_OPCAO_VARIACAO POV ON VO.nCdVariacaoOpcao = POV.nCdVariacaoOpcao AND POV.nCdProduto = @nCdProduto
            ) SELECT * FROM PRODUTO_VARIACAO PV WHERE EXISTS ( SELECT 1 FROM PRODUTO_VARIACAO PV2 WHERE PV2.nCdVariacao = PV.nCdVariacao AND PV2.bFlAtrelado = 1 );";

            return sSql;
        }
    }
}
