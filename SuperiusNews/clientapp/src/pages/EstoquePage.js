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

            const response = await apiConfig.estoque.axios.get(
                `${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.pesquisarEstoques}`
            );

            if (!response.data || !response.data.estoques) {
                throw new Error('Resposta inválida da API');
            }
            setEstoques(response.data.estoques);
        } catch (error) {
            console.error('Erro ao carregar estoque:', error);
            alert('Erro ao carregar produtos. Verifique o console para detalhes.');
        } finally { setLoading(false);}
    };

    const handleAbrirEstoqueClick = (codigoEstoque) => { navigate(`/administrador/estoque/${codigoEstoque}`); };

    const handleEstoqueClick = (codigoEstoque) => { navigate(`/administrador/editar-estoque/${codigoEstoque}`); };

    const handleDeleteClick = async (codigosEstoques) => {
        try {

            setError(false);
            setSuccess(false);
            setMensagem('');

            const dadosEnvio = JSON.stringify(String(codigosEstoques));
            const response = await apiConfig.estoque.axios.delete(
                `${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.excluirEstoques}`,
                {
                    data: dadosEnvio,
                    headers: { 'Content-Type': 'application/json' }
                }
            );
            await carregarEstoque();

            if (response.data.status === 1) {
                setSuccess(true);
                setMensagem(`Estoque(s) '${codigosEstoques}' excluído(s) com sucesso!`);
                await carregarEstoque();
            }
            else if (response.data.status === 2) {
                setError(true);
                setMensagem("Não é possível excluir um estoque que já tenha movimentações." || response.data.mensagem);
            }
            else {
                setError(true);
                setMensagem(response.data.mensagem || "Erro desconhecido ao excluir estoque.");
            }

        } catch (error) {
            console.error(error);
            setError(true);
            setMensagem("Erro ao deletar estoques: " + error);
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
                    <EstoqueTabela estoques={estoques} loading={loading} onEstoqueClick={handleEstoqueClick} onDeleteClick={handleDeleteClick} onRefresh={handleRefreshClick} onAbrirEstoque={handleAbrirEstoqueClick} />
                </>
            )}
        </div>
    );
};

export default EstoquePage;