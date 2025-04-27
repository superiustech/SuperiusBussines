import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ProductsPage from './pages/ProductsPage';
import CadastrarProduto from './pages/CadastrarProduto';
import CadastrarEstoquePage from './pages/CadastrarEstoquePage';
import ImagemProdutoPage from './pages/ImagemProdutoPage';
import VariacaoProdutoPage from './pages/VariacaoProdutoPage';

import EstoqueProdutoPage from './pages/EstoqueProdutoPage';
import CadastrarRevendedorPage from './pages/CadastrarRevendedorPage';
import RevendedorPage from './pages/RevendedorPage';
import EstoquePage from './pages/EstoquePage';
import Layout from './components/layout/Layout';
import './styles/css/site.css';
import './styles/App.css';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

function App() {
    useEffect(() => {
        // Força UTF-8 para todas as requisições
        document.documentElement.lang = 'pt-BR';
        document.documentElement.charset = 'UTF-8';
    }, []);

    return (
        <Router>
            <Layout>
                <Routes>
                    {/*Produto*/}
                    <Route path="/administrador/produtos" element={<ProductsPage />} />
                    <Route path="/administrador/cadastrar-produto" element={<CadastrarProduto />} />
                    <Route path="/administrador/editar-produto/:codigoProduto" element={<CadastrarProduto />} />
                    <Route path="/administrador/produto-imagem/:codigoProduto" element={<ImagemProdutoPage />}/>
                    <Route path="/administrador/produto-variacao/:codigoProduto" element={<VariacaoProdutoPage />} />
                    {/*Estoque*/}
                    <Route path="/administrador/estoques" element={<EstoquePage />} />
                    <Route path="/administrador/cadastrar-estoque" element={<CadastrarEstoquePage />} />
                    <Route path="/administrador/editar-estoque/:codigoEstoque" element={<CadastrarEstoquePage />} />
                    <Route path="/administrador/estoque-produto/:codigoEstoque" element={<EstoqueProdutoPage />} />
                    {/*Revendedores*/}
                    <Route path="/administrador/revendedores" element={<RevendedorPage />} />z
                    <Route path="/administrador/cadastrar-revendedor" element={<CadastrarRevendedorPage />} />
                    <Route path="/administrador/editar-revendedor/:codigoRevendedor" element={<CadastrarRevendedorPage />} />
                </Routes>
            </Layout>
        </Router>
    );
}

export default App;