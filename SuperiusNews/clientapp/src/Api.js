const baseApiUrl = `${window.location.origin}/api`;
export default {
    produto: {
        baseURL: `${baseApiUrl}/Produto`,
        endpoints: {
            consultarProduto: '/ConsultarProduto', 
            pesquisarProdutosComPaginacao: '/PesquisarProdutosComPaginacao', 
            cadastrarProduto: '/CadastrarProduto',
            editarVariacaoProduto: '/EditarVariacaoProduto',
            atualizarProduto: '/AtualizarProduto', 
            consultarVariacoesProduto: '/ConsultarVariacoesProduto', 
            tipoVariacao: '/TipoVariacao', 
            unidadeDeMedida: '/UnidadeDeMedida',
            opcoesVariacao: '/OpcoesVariacao', 
            obterImagensProduto: '/ImagensProduto', 
            adicionarImagem: '/AdicionarImagem', 
            excluirImagem: '/ExcluirImagem' 
        }
    },
    estoque: {
        baseURL: `${baseApiUrl}/Estoque`,
        endpoints: {
            pesquisarEstoquesComPaginacao: "/PesquisarEstoquesComPaginacao",
            consultarEstoque: "/Estoque",
            cadastrarEstoque: "/CadastrarEstoque", 
            estoqueProduto: "/EstoqueProduto",
            adicionarEstoqueProduto: "/AdicionarEstoqueProduto", 
            removerEstoqueProduto: "/RemoverEstoqueProduto",
            editarProdutoEstoque: "/EditarProdutoEstoque"
        }
    },
    revendedor: {
        baseURL: `${baseApiUrl}/Revendedor`,
        endpoints: {
            tiposRevendedor: "/TiposRevendedor",
            consultarRevendedor: "/Revendedor",  
            cadastrarRevendedor: "/CadastrarRevendedor"
        }
    }
};
