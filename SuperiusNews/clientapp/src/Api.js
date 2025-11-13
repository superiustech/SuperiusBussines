import axios from 'axios';

//const baseApiUrl = `${window.location.origin}/api`;
const baseApiUrl = process.env.REACT_APP_API_URL || `${window.location.origin}/api`;

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
            movimentarEntradaSaida: "/MovimentarEntradaSaida",
            movimentacoesRecentes: "/MovimentacoesRecentesHistorico"
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
            excluirRevendedores: "/ExcluirRevendedores",
            associarDesassiarUsuarios: "/AssociarDesassociarUsuarios",
            usuariosAssociadosCompleto: "/UsuariosAssociadosCompleto"
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
            pesquisarFuncionalidades: "/Funcionalidades",
            cadastrarFuncionalidade: "/Funcionalidade",
            ativarFuncionalidades: "/AtivarFuncionalidade",
            inativarFuncionalidades: "/InativarFuncionalidade",
            editarFuncionalidade: "/Funcionalidade"
        }
    },
    permissao: {
        baseURL: `${baseApiUrl}/Permissao`,
        axios: axiosWithToken(`${baseApiUrl}/Permissao`),
        endpoints: {
            pesquisarPermissoes: "/Permissoes",
            cadastrarPermissao: "/Permissao",
            ativarPermissoes: "/AtivarPermissoes",
            inativarPermissoes: "/InativarPermissoes",
            editarPermissao: "/Permissao",
            associarFuncionalidades: "/AssociarFuncionalidades",
            desassociarFuncionalidades: "/DesassociarFuncionalidades",
            funcionalidadesAtreladas: "/FuncionalidadesAtreladas",
            funcionalidadesAssociadasCompleto: "/FuncionalidadesAssociadasCompleto",
            associarDesassociarFuncionalidades: "/AssociarDesassociarFuncionalidades"
        }
    },
    perfil: {
        baseURL: `${baseApiUrl}/Perfil`,
        axios: axiosWithToken(`${baseApiUrl}/Perfil`),
        endpoints: {
            pesquisarPerfis: "/Perfis",
            cadastrarPerfil: "/Perfil",
            ativarPerfis: "/AtivarPerfis",
            inativarPerfis: "/InativarPerfis",
            editarPerfil: "/Perfil",
            associarPermissoes: "/AssociarPermissoes",
            desassociarPermissoes: "/DesassociarPermissoes",
            permissoesAtreladas: "/PermissoesAtreladas",
            permissoesAssociadasCompleto: "/PermissoesAssociadasCompleto",
            associarDesassociarPermissoes: "/AssociarDesassociarPermissoes"
        }
    },
    usuario: {
        baseURL: `${baseApiUrl}/Usuario`,
        axios: axiosWithToken(`${baseApiUrl}/Usuario`),
        endpoints: {
            pesquisarUsuarios: "/Usuarios",
            cadastrarUsuario: "/Usuario",
            ativarUsuarios: "/AtivarUsuarios",
            inativarUsuarios: "/InativarUsuarios",
            editarUsuario: "/Usuario",
            associarPerfis: "/AssociarPerfis",
            desassociarPerfis: "/DesassociarPerfis",
            perfisAtrelados: "/PerfisAtrelados",
            perfisAssociadosCompleto: "/PerfisAssociadosCompleto",
            associarDesassociarPerfis: "/AssociarDesassociarPerfis"
        }
    },
    dashboard: {
        baseURL: `${baseApiUrl}/Dashboard`,
        axios: axiosWithToken(`${baseApiUrl}/Dashboard`),
        endpoints: {
            dashboardResumo: "/ResumoDashboard",
            produtosPorEstoques: "/ProdutosPorEstoques"
        }
    }
};
