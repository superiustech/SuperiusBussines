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
import Layout from './components/layout/Layout';
import { AuthProvider, useAuth } from './components/common/AuthContext';
import ProtectedRoute from './components/common/ProtectedRoute';
import './styles/css/site.css';
import './styles/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const AuthenticatedApp = memo(() => (
    <Layout>
        <Routes>
            {/* Rotas protegidas - só aparecem com layout quando logado */}
            <Route path="/administrador/produtos" element={<ProtectedRoute><ProductsPage /></ProtectedRoute>} />
            <Route path="/administrador/cadastrar-produto" element={<ProtectedRoute><CadastrarProduto /></ProtectedRoute>} />
            <Route path="/administrador/editar-produto/:codigoProduto" element={<ProtectedRoute><CadastrarProduto /></ProtectedRoute>} />
            <Route path="/administrador/produto-imagem/:codigoProduto" element={<ProtectedRoute><ImagemProdutoPage /></ProtectedRoute>} />
            <Route path="/administrador/produto-variacao/:codigoProduto" element={<ProtectedRoute><VariacaoProdutoPage /></ProtectedRoute>} />

            {/* Estoque */}
            <Route path="/administrador/estoque/:codigoEstoque" element={<ProtectedRoute><ControleEstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/estoques" element={<ProtectedRoute><EstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/cadastrar-estoque" element={<ProtectedRoute><CadastrarEstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/editar-estoque/:codigoEstoque" element={<ProtectedRoute><CadastrarEstoquePage /></ProtectedRoute>} />
            <Route path="/administrador/estoque-produto/:codigoEstoque" element={<ProtectedRoute><EstoqueProdutoPage /></ProtectedRoute>} />

            {/* Revendedores */}
            <Route path="/administrador/revendedores" element={<ProtectedRoute><RevendedorPage /></ProtectedRoute>} />
            <Route path="/administrador/cadastrar-revendedor" element={<ProtectedRoute><CadastrarRevendedorPage /></ProtectedRoute>} />
            <Route path="/administrador/editar-revendedor/:codigoRevendedor" element={<ProtectedRoute><CadastrarRevendedorPage /></ProtectedRoute>} />

            {/* Redirecionamento para login se acessar qualquer rota sem estar logado */}
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