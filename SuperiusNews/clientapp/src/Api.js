const baseApiUrl = `${window.location.origin}/api`;
export default {
    produto: {
        baseURL: `${baseApiUrl}/Produto`,
        endpoints: {
            consultarProduto: '/ConsultarProduto', 
            pesquisarProdutos: '/Produtos', 
            cadastrarProduto: '/CadastrarProduto',
            editarVariacaoProduto: '/EditarVariacaoProduto',
            atualizarProduto: '/AtualizarProduto', 
            consultarVariacoesProduto: '/ConsultarVariacoesProduto', 
            tipoVariacao: '/TipoVariacao', 
            unidadeDeMedida: '/UnidadeDeMedida',
            opcoesVariacao: '/OpcoesVariacao', 
            obterImagensProduto: '/ImagensProduto', 
            adicionarImagem: '/AdicionarImagem', 
            excluirImagem: '/ExcluirImagem',
            excluirProdutos: '/ExcluirProdutos'
        }
    },
    estoque: {
        baseURL: `${baseApiUrl}/Estoque`,
        endpoints: {
            pesquisarEstoquesComPaginacao: "/PesquisarEstoquesComPaginacao",
            pesquisarEstoques: "/Estoques",
            pesquisarEstoquesSemRevendedor: '/PesquisarEstoquesSemRevendedor', 
            consultarEstoque: "/Estoque",
            cadastrarEstoque: "/CadastrarEstoque", 
            estoqueProduto: "/EstoqueProduto",
            adicionarEstoqueProduto: "/AdicionarEstoqueProduto", 
            removerEstoqueProduto: "/RemoverEstoqueProduto",
            editarProdutoEstoque: "/EditarProdutoEstoque",
            excluirEstoques: "/ExcluirEstoques",
            movimentarEntradaSaida: "/MovimentarEntradaSaida"
        }
    },
    revendedor: {
        baseURL: `${baseApiUrl}/Revendedor`,
        endpoints: {
            tiposRevendedor: "/TiposRevendedor",
            consultarRevendedor: "/Revendedor",  
            cadastrarRevendedor: "/CadastrarRevendedor",
            pesquisarRevendedores: "/Revendedores",
            excluirRevendedores: "/ExcluirRevendedores"
        }
    }
};
