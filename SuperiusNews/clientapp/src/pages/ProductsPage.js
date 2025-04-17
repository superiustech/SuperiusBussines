import React, { useState, useEffect } from 'react';
import ProductFilter from '../components/products/ProductFilter';
import ProductTable from '../components/products/ProductTable';
import { getProducts } from '../services/productService';
import { useNavigate, useParams } from 'react-router-dom';

const ProductsPage = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [filters, setFilters] = useState({ sNmProduto: '', sDsProduto: ''
    });
    const pageSize = 10;
    const navigate = useNavigate();
    useEffect(() => {
        if (currentPage > totalPages && totalPages > 0) { setCurrentPage(totalPages); }
        else if (totalPages === 0 && currentPage !== 1) { setCurrentPage(1); }
        loadProducts(currentPage);
    }, [totalPages, currentPage, filters]);

    const loadProducts = async (page) => {
        setLoading(true);
        try {
            const response = await getProducts(page, pageSize, filters);
            if (!response || !response.produtos) {
                throw new Error('Resposta inválida da API');
            }
            setProducts(response.produtos);
            setTotalPages(response.totalPaginas);
        } catch (error) {
            console.error('Erro ao carregar produtos:', error);
            // Adicione tratamento de erro visível para o usuário
            alert('Erro ao carregar produtos. Verifique o console para detalhes.');
        } finally {
            setLoading(false);
        }
  };

    const handleFilterChange = (newFilters) => {
        setFilters(newFilters);
    };

    const handleClearFilters = () => {
        setFilters({ sNmProduto: '', sDsProduto: '' });
    };

    const handleProductClick = (codigoProduto) => {
        navigate(`/editar-produto/${codigoProduto}`);
    };

    const handleDeleteClick = (productId) => {
        // Implemente a lógica de exclusão
        console.log('Excluir produto:', productId);
    };

    return (
        <div className="container">
            {loading}
            <ProductFilter onFilterChange={handleFilterChange} onClearFilters={handleClearFilters} />
            <ProductTable products={products} onProductClick={handleProductClick} onDeleteClick={handleDeleteClick} currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage}/>
        </div>
    );
};

export default ProductsPage;