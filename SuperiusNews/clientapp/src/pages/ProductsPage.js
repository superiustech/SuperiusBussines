import React, { useState, useEffect } from 'react';
import ProductTable from '../components/products/ProductTable';
import { getProducts, deletarProdutos } from '../services/productService';
import { useNavigate } from 'react-router-dom';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components//ui/FlashMessage';

const ProductsPage = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);
    const [mensagem, setMensagem] = useState('');
    const navigate = useNavigate();

    const loadProducts = async () => {
        setLoading(true);
        try {
            const response = await getProducts();
            if (!response || !response.produtos) {
                setError(true);
                setMensagem("Nenhum produto existente.");
            }
            setProducts(response.produtos);
        } catch (error) {
            setError(true);
            setMensagem("Erro ao carregar produtos.");
        } finally {

            setLoading(false);
        }
  };

    const handleProductClick = (codigoProduto) => { navigate(`/administrador/editar-produto/${codigoProduto}`); };

    const handleDeleteClick = async (arrCodigoProdutos) => {
        try {
            await deletarProdutos(arrCodigoProdutos);
            await loadProducts();
            setMensagem("Produto(s) '" + arrCodigoProdutos + "' excluídos com sucesso! ")
            setSuccess(true);
        } catch (error) {
            console.error();
            setMensagem("Erro ao deletar produtos:" + error);
            setError(true);
            
        }
    };

    const handleRefreshClick = () => { loadProducts() };

    useEffect(() => {
        loadProducts();
    }, []);

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
            <>
                <h1 className="fw-bold display-5 text-primary m-0 mb-5"> <i className="bi bi-people-fill me-2"></i> Produtos </h1>
                <ProductTable products={products} loading={loading} onProductClick={handleProductClick} onDeleteClick={handleDeleteClick} onRefresh={handleRefreshClick} />
                </>
            )}
        </div>
    );
};

export default ProductsPage;