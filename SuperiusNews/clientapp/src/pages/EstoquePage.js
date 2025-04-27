import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import EstoqueTabela from '../components/estoque/EstoqueTabela';
import apiConfig from '../Api';
import axios from 'axios';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components//ui/FlashMessage';

const EstoquePage = () => {
    const navigate = useNavigate();
    const [estoques, setEstoques] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);
    const [mensagem, setMensagem] = useState('');

    const carregarEstoque = async () => {
        setLoading(true);
        try {
            const response = await axios.get(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.pesquisarEstoques}`);

            if (!response.data || !response.data.estoques) {
                throw new Error('Resposta inválida da API');
            }
            setEstoques(response.data.estoques);
        } catch (error) {
            console.error('Erro ao carregar estoque:', error);
            alert('Erro ao carregar produtos. Verifique o console para detalhes.');
        } finally { setLoading(false);}
  };

    const handleEstoqueClick = (codigoEstoque) => { navigate(`/administrador/editar-estoque/${codigoEstoque}`); };

    const handleDeleteClick = async (codigosEstoques) => {
        try {
            const dadosEnvio = JSON.stringify(String(codigosEstoques));
            const response = await axios.delete(
                `${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.excluirEstoques}`,
                {
                    data: dadosEnvio,
                    headers: { 'Content-Type': 'application/json' }
                }
            );
            await carregarEstoque();
            setMensagem("Estoques(s) '" + codigosEstoques + "' excluídos com sucesso!");
            setSuccess(true);
        } catch (error) {
            console.error(error);
            setMensagem("Erro ao deletar estoques: " + error);
            setError(true);
        }
    };

    const handleRefreshClick = () => { carregarEstoque() };

    useEffect(() => { carregarEstoque(); }, []);

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5"> <i className="bi bi-people-fill me-2"></i> Estoques </h1>
                    <EstoqueTabela estoques={estoques} loading={loading} onEstoqueClick={handleEstoqueClick} onDeleteClick={handleDeleteClick} onRefresh={handleRefreshClick} />
                </>
            )}
        </div>
    );
};

export default EstoquePage;