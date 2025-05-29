namespace Domain.Uteis
{
    public static class FuncionalidadeMapper
    {
        public static readonly Dictionary<string, HashSet<int>> EndpointParaFuncionalidade = new()
        {
            // Estoque
            ["/api/Estoque/Estoques"] = new HashSet<int> { 1, 34 },
            ["/api/Estoque/PesquisarEstoquesSemRevendedor"] = new HashSet<int> { 1 },
            ["/api/Estoque/Estoque/{codigoEstoque}"] = new HashSet<int> { 1, 2, 4, 34 },
            ["/api/Estoque/EstoqueProduto/{codigoEstoque}"] = new HashSet<int> { 1, 2, 4 },
            ["/api/Estoque/HistoricoMovimentacao/{codigoEstoque}"] = new HashSet<int> { 4 },
            ["/api/Estoque/CadastrarEstoque"] = new HashSet<int> { 2 },
            ["/api/Estoque/MovimentarEntradaSaida"] = new HashSet<int> { 5, 6 },
            ["/api/Estoque/EditarProdutoEstoque"] = new HashSet<int> { 2 },
            ["/api/Estoque/RemoverEstoqueProduto"] = new HashSet<int> { 7 },
            ["/api/Estoque/ExcluirEstoques"] = new HashSet<int> { 3 },

            // Funcionalidade
            ["/api/Funcionalidade/Funcionalidades"] = new HashSet<int> { 15 },
            ["/api/Funcionalidade/Funcionalidade"] = new HashSet<int> { 16 },
            ["/api/Funcionalidade/AtivarFuncionalidade"] = new HashSet<int> { 17 },
            ["/api/Funcionalidade/InativarFuncionalidade"] = new HashSet<int> { 18 },

            // Perfil
            ["/api/Perfil/Perfis"] = new HashSet<int> { 24 },
            ["/api/Perfil/Perfil"] = new HashSet<int> { 20 },
            ["/api/Perfil/PermissoesAssociadasCompleto/{codigoPermissao}"] = new HashSet<int> { 27 },
            ["/api/Perfil/PermissoesAssociadas/{codigoPerfil}"] = new HashSet<int> { 27 },
            ["/api/Perfil/AssociarDesassociarPermissoes"] = new HashSet<int> { 27 },
            ["/api/Perfil/AtivarPerfis"] = new HashSet<int> { 25 },
            ["/api/Perfil/InativarPerfis"] = new HashSet<int> { 26 },
            ["/api/Perfil/AssociarPermissoes"] = new HashSet<int> { 27 },
            ["/api/Perfil/DesassociarPermissoes"] = new HashSet<int> { 27 },

            // Permissao
            ["/api/Permissao/Permissoes"] = new HashSet<int> { 19 }, 
            ["/api/Permissao/Permissao"] = new HashSet<int> { 20 },
            ["/api/Permissao/FuncionalidadesAssociadas/{codigoPermissao}"] = new HashSet<int> { 23 },
            ["/api/Permissao/FuncionalidadesAssociadasCompleto/{codigoPermissao}"] = new HashSet<int> { 23 },
            ["/api/Permissao/AssociarFuncionalidades"] =  new HashSet<int> { 23 },
            ["/api/Permissao/AssociarDesassociarFuncionalidades"] =  new HashSet<int> { 23 },
            ["/api/Permissao/DesassociarFuncionalidades"] =  new HashSet<int> { 23 },
            ["/api/Permissao/AtivarPermissoes"] =  new HashSet<int> { 21 },
            ["/api/Permissao/InativarPermissoes"] =  new HashSet<int> { 22 },

            // Produto
            ["/api/Produto/ConsultarProduto/{nCdProduto}"] =  new HashSet<int> { 8 },
            ["/api/Produto/Produtos"] =  new HashSet<int> { 8 },
            ["/api/Produto/ConsultarVariacoesProduto/{nCdProduto}"] =  new HashSet<int> { 9 },
            ["/api/Produto/TipoVariacao"] =  new HashSet<int> { 9 },
            ["/api/Produto/UnidadeDeMedida"] =  new HashSet<int> { 9 },
            ["/api/Produto/OpcoesVariacao/{tipo}"] =  new HashSet<int> { 9 },
            ["/api/Produto/ImagensProduto/{codigoProduto}"] =  new HashSet<int> { 9 },
            ["/api/Produto/CadastrarProduto"] =  new HashSet<int> { 9 },
            ["/api/Produto/AdicionarImagem"] =  new HashSet<int> { 9 },
            ["/api/Produto/EditarVariacaoProduto"] =  new HashSet<int> { 9 },
            ["/api/Produto/AtualizarProduto"] =  new HashSet<int> { 9 },
            ["/api/Produto/ExcluirImagem/{codigoImagem}"] =  new HashSet<int> { 9 },
            ["/api/Produto/ExcluirProdutos"] =  new HashSet<int> { 10 },

            // Revendedor
            ["/api/Revendedor/TiposRevendedor"] =  new HashSet<int> { 12 },
            ["/api/Revendedor/Revendedor/{nCdRevendedor}"] =  new HashSet<int> { 11, 12 },
            ["/api/Revendedor/Revendedores"] =  new HashSet<int> { 11 },
            ["/api/Revendedor/CadastrarRevendedor"] =  new HashSet<int> { 12 },
            ["/api/Revendedor/ExcluirRevendedores"] =  new HashSet<int> { 13 },
            ["/api/Revendedor/AssociarDesassociarUsuarios"] = new HashSet<int> { 33 },
            ["/api/Revendedor/UsuariosAssociadosCompleto"] = new HashSet<int> { 12 , 33 },

            // Usuario
            ["/api/Usuario/Usuarios"] =  new HashSet<int> { 28 }, 
            ["/api/Usuario/Usuario"] =  new HashSet<int> { 29 },
            ["/api/Usuario/FuncionalidadesUsuario/{codigoUsuario}"] =  new HashSet<int> { 28 },
            ["/api/Usuario/PerfisAssociadosCompleto/{codigoUsuario}"] =  new HashSet<int> { 28 },
            ["/api/Usuario/PerfisAssociados/{codigoUsuario}"] =  new HashSet<int> { 28 },
            ["/api/Usuario/AssociarDesassociarPerfis"] =  new HashSet<int> { 30 },
            ["/api/Usuario/AssociarPerfis"] =  new HashSet<int> { 30 },
            ["/api/Usuario/DesassociarPerfis"] = new HashSet<int> { 30 }
        };
    }

}
