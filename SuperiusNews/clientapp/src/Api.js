import axios from 'axios';

//const baseApiUrl = `${window.location.origin}/api`;
const baseApiUrl = `http://20.0.114.230:3000/api`;
const getAuthToken = () => localStorage.getItem('authToken'); 
const axiosWithToken = (baseURL) => {
    const instance = axios.create({ baseURL });
    instance.interceptors.request.use(
        (config) => {
            const token = getAuthToken();
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        },
        (error) => Promise.reject(error)
    );
    return instance;
};

export default {
    produto: {
        baseURL: `${baseApiUrl}/Produto`,
        axios: axiosWithToken(`${baseApiUrl}/Produto`),
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
        axios: axiosWithToken(`${baseApiUrl}/Estoque`),
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
        axios: axiosWithToken(`${baseApiUrl}/Revendedor`),
        endpoints: {
            tiposRevendedor: "/TiposRevendedor",
            consultarRevendedor: "/Revendedor",
            cadastrarRevendedor: "/CadastrarRevendedor",
            pesquisarRevendedores: "/Revendedores",
            excluirRevendedores: "/ExcluirRevendedores"
        }
    },
    autenticacao: {
        baseURL: `${baseApiUrl}/Autenticacao`,
        endpoints: {
            token: "/Token",
            login: "/Login"
        }
    },
    funcionalidade: {
        baseURL: `${baseApiUrl}/Funcionalidade`,
        axios: axiosWithToken(`${baseApiUrl}/Funcionalidade`),
        endpoints: {
            pesquisarFuncionalidades: "/Funcionalidade",
            cadastrarFuncionalidade: "/Funcionalidade",
            ativarFuncionalidades: "/AtivarFuncionalidade",
            inativarFuncionalidades: "/InativarFuncionalidade",
            editarFuncionalidade: "/Funcionalidade"
        }
    }

};
