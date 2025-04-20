import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ProductsPage from './pages/ProductsPage';
import CadastrarProduto from './pages/CadastrarProduto';
import CadastrarEstoquePage from './pages/CadastrarEstoquePage';
import ImagemProdutoPage from './pages/ImagemProdutoPage';
import VariacaoProdutoPage from './pages/VariacaoProdutoPage';
import EstoqueProdutoPage from './pages/EstoqueProdutoPage';
import CadastrarRevendedorPage from './pages/CadastrarRevendedorPage';
import EstoquePage from './pages/EstoquePage';
import Layout from './components/layout/Layout';
import './styles/css/site.css';
import './App.css';

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
                    <Route path="/produtos" element={<ProductsPage />} />
                    <Route path="/cadastrar-produto" element={<CadastrarProduto />} />
                    <Route path="/editar-produto/:codigoProduto" element={<CadastrarProduto />} />
                    <Route path="/produto-imagem/:codigoProduto" element={<ImagemProdutoPage />}/>
                    <Route path="/produto-variacao/:codigoProduto" element={<VariacaoProdutoPage />} />
                    {/*Estoque*/}
                    <Route path="/estoques" element={<EstoquePage />} />
                    <Route path="/cadastrar-estoque" element={<CadastrarEstoquePage />} />
                    <Route path="/editar-estoque/:codigoEstoque" element={<CadastrarEstoquePage />} />
                    <Route path="/estoque-produto/:codigoEstoque" element={<EstoqueProdutoPage />} />
                    {/*Revendedores*/}
                    <Route path="/revendedores" element={<EstoquePage />} />z
                    <Route path="/cadastrar-revendedores" element={<CadastrarRevendedorPage />} />
                    <Route path="/editar-revendedor/:codigoRevendedor" element={<CadastrarRevendedorPage />} />

                </Routes>
            </Layout>
        </Router>
    );
}

export default App;