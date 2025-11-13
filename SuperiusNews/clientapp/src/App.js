import React, { useEffect, useCallback, useMemo, memo } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import ProductsPage from './pages/ProductsPage';
import CadastrarProduto from './pages/CadastrarProduto';
import CadastrarEstoquePage from './pages/CadastrarEstoquePage';
import ImagemProdutoPage from './pages/ImagemProdutoPage';
import VariacaoProdutoPage from './pages/VariacaoProdutoPage';
import ControleEstoquePage from './pages/ControleEstoquePage';
import EstoqueProdutoPage from './pages/EstoqueProdutoPage';
import CadastrarRevendedorPage from './pages/CadastrarRevendedorPage';
import RevendedorPage from './pages/RevendedorPage';
import EstoquePage from './pages/EstoquePage';
import LoginPage from './pages/LoginPage';
import ConfiguracaoPage from './pages/ConfiguracaoPage';
import DashboardPage from './pages/DashboardPage';
import FuncionalidadesPage from './pages/FuncionalidadesPage';
import PermissoesPage from './pages/PermissoesPage';
import UsuariosPage from './pages/UsuariosPage';
import SemAcessoPage from './pages/SemAcessoPage';
import PerfisPage from './pages/PerfisPage';
import Layout from './components/layout/Layout';
import { AuthProvider, useAuth } from './components/common/AuthContext';
import ProtectedRoute from './components/common/ProtectedRoute';
import funcionalidades from './components/common/Funcionalidades';
import './styles/css/site.css';
import './styles/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootswatch/dist/lux/bootstrap.min.css';

const AuthenticatedApp = memo(() => (
    <Layout>
        <Routes>
            {/* Rotas protegidas - só aparecem com layout quando logado */}
            {/*Dashboard*/}
            <Route path="/administrador/dashboard" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_DASHBOARD}> <DashboardPage /> </ProtectedRoute>} />

            {/*Produtos*/}
            <Route path="/administrador/produtos" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_PRODUTOS}> <ProductsPage /> </ProtectedRoute>} />
            <Route path="/administrador/cadastrar-produto" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_PRODUTOS}><CadastrarProduto /></ProtectedRoute>} />
            <Route path="/administrador/editar-produto/:codigoProduto" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_PRODUTOS}><CadastrarProduto /></ProtectedRoute>} />
            <Route path="/administrador/produto-imagem/:codigoProduto" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_PRODUTOS}><ImagemProdutoPage /></ProtectedRoute>} />
            <Route path="/administrador/produto-variacao/:codigoProduto" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_PRODUTOS}><VariacaoProdutoPage /></ProtectedRoute>} />

            {/* Estoque */}
            <Route path="/administrador/estoque/:codigoEstoque" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_MOVIMENTACOES}><ControleEstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/estoques" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_ESTOQUES}><EstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/cadastrar-estoque" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_ESTOQUES}><CadastrarEstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/editar-estoque/:codigoEstoque" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_ESTOQUES}><CadastrarEstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/estoque-produto/:codigoEstoque" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_ESTOQUES}><EstoqueProdutoPage /></ProtectedRoute>} />

            {/* Revendedores */}
            <Route path="/administrador/revendedores" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_REVENDEDORES}><RevendedorPage /></ProtectedRoute>} />
            <Route path="/administrador/cadastrar-revendedor" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_REVENDEDORES}><CadastrarRevendedorPage /></ProtectedRoute>} />
            <Route path="/administrador/editar-revendedor/:codigoRevendedor" element={<ProtectedRoute funcionalidade={funcionalidades.EDITAR_REVENDEDORES}><CadastrarRevendedorPage /></ProtectedRoute>} />

            {/* Configuração */}
            <Route path="/administrador/configuracoes" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_CONFIGURACOES}><ConfiguracaoPage /></ProtectedRoute>} />
            <Route path="/administrador/funcionalidades" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_FUNCIONALIDADES}><FuncionalidadesPage /></ProtectedRoute>} />
            <Route path="/administrador/permissoes" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_PERMISSOES}><PermissoesPage /></ProtectedRoute>} />
            <Route path="/administrador/perfis" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_PERFIS}><PerfisPage /></ProtectedRoute>} />
            <Route path="/administrador/usuarios" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_USUARIOS}><UsuariosPage /></ProtectedRoute>} />

            {/* Redirecionamento para login se acessar qualquer rota sem estar logado */}
            <Route path="/administrador/sem-acesso" element={<ProtectedRoute><SemAcessoPage /></ProtectedRoute>} />
            <Route path="*" element={<Navigate to="/administrador/login" replace />} />
        </Routes>
    </Layout>
));

const PublicRoutes = memo(() => (
    <Routes>
        <Route path="/administrador/login" element={<LoginPage />} />
        <Route path="*" element={<Navigate to="/administrador/login" replace />} />
    </Routes>
));

const AppWrapper = memo(() => {
    const { isAuthenticated } = useAuth();

    return (
        <div className="app-container">
            {isAuthenticated() ? <AuthenticatedApp /> : <PublicRoutes />}
        </div>
    );
});
function App() {
    useEffect(() => {
        document.documentElement.lang = 'pt-BR';
        document.documentElement.charset = 'UTF-8';
    }, []);

    return (
        <Router>
            <AuthProvider>
                <AppWrapper />
            </AuthProvider>
        </Router>
    );
}

export default App;