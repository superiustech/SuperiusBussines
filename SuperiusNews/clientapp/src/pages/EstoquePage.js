import React, { useState, useEffect } from 'react';
import EstoqueFiltro from '../components/estoque/EstoqueFiltro';
import EstoqueTabela from '../components/estoque/EstoqueTabela';
import { useNavigate, useParams } from 'react-router-dom';
import apiConfig from '../Api';
import axios from 'axios';

const ProductsPage = () => {
    const [estoques, setEstoques] = useState([]);
    const [loading, setLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [filters, setFilters] = useState({ sNmEstoque: '', sDsEstoque: ''});
    const pageSize = 10;
    const navigate = useNavigate();

    const carregarEstoque = async (page) => {
        setLoading(true);
        try {
            const params = new URLSearchParams({
                page: page.toString(),
                pageSize: pageSize.toString(),
                ...(filters.sNmEstoque && { 'oFiltroRequest.sNmFiltro': filters.sNmEstoque }),
                ...(filters.sDsEstoque && { 'oFiltroRequest.sDsFiltro': filters.sDsEstoque })
            });

            const response = await axios.get(`${apiConfig.baseURL}${apiConfig.endpoints.estoques}?${params}`);

            if (!response.data || !response.data.estoques) {
                throw new Error('Resposta inválida da API');
            }
            setEstoques(response.data.estoques);
            setTotalPages(response.data.totalPaginas);
        } catch (error) {
            console.error('Erro ao carregar estoque:', error);
            alert('Erro ao carregar produtos. Verifique o console para detalhes.');
        } finally { setLoading(false);}
  };

    const handleFilterChange = (newFilters) => { setFilters(newFilters);};

    const handleClearFilters = () => { setFilters({ sNmEstoque: '', sDsEstoque: '' });};

    const handleEstoqueClick = (codigoEstoque) => { navigate(`/editar-estoque/${codigoEstoque}`); };

    const handleDeleteClick = (productId) => { console.log('Excluir estoque:', productId); };

    useEffect(() => {
        if (currentPage > totalPages && totalPages > 0) { setCurrentPage(totalPages); }
        else if (totalPages === 0 && currentPage !== 1) { setCurrentPage(1); }
        carregarEstoque(currentPage);
    }, [totalPages, currentPage, filters]);

    return (
        <div className="container">
            {loading}
            <EstoqueFiltro onFilterChange={handleFilterChange} onClearFilters={handleClearFilters} />
            <EstoqueTabela estoques={estoques} onEstoqueClick={handleEstoqueClick} onDeleteClick={handleDeleteClick} currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
        </div>
    );
};

export default ProductsPage;