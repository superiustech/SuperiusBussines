export default {
    baseURL: '/api/Administrador',
    endpoints: {
        addImage: '/AdicionarImagem',
        getImages: '/ObterImagensProduto',
        deletarImagens: '/ExcluirImagem',
        getVariacoesProduto: '/ConsultarVariacoesProduto',
        getVariacoes: '/GetTipoVariacao',
        getVariacaoPorId: '/GetOpcoesVariacao',
        adicionarEditarVariacoes: '/EditarVariacaoProduto',
        cadastrarEstoque: '/CadastrarEstoque',
        estoqueProduto: '/EstoqueProduto',
        adicionarEstoqueProduto: '/AdicionarEstoqueProduto',
        removerEstoqueProduto: '/RemoverEstoqueProduto',
        consultarProduto: '/ConsultarProduto',
        unidadeMedida: '/UnidadeDeMedida',
        salvarDados: '/SalvarDados',
        estoques: '/PesquisarEstoquesComPaginacao',
        consultarEstoque: '/Estoque'
    }
};