import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiConfig from '../Api';

export const DashboardService = () => {
    const navigate = useNavigate();

    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', dashboardResumo: [], historico: [], produtosEstoques: []});

    const carregarDashboardResumo = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.dashboard.axios.get(`${apiConfig.dashboard.baseURL}${apiConfig.dashboard.endpoints.dashboardResumo}`);
            setState(prev => ({ ...prev, dashboardResumo: response.data, loading: false }));
        }
        catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const carregarHistoricos = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.estoque.axios.get(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.movimentacoesRecentes}`);
            setState(prev => ({ ...prev, historico: response.data, loading: false }));
        }
        catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const carregarProdutosEstoques = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.dashboard.axios.get(`${apiConfig.dashboard.baseURL}${apiConfig.dashboard.endpoints.produtosPorEstoques}`);

            const todosEstoques = new Set();
            response.data.forEach(produto => { produto.estoques.forEach(estoque => { todosEstoques.add(estoque.nome); });});

            const dadosParaGrafico = response.data.map(produto => {
                const novoFormato = { nomeProduto: produto.nomeProduto };
                todosEstoques.forEach(nomeEstoque => { novoFormato[nomeEstoque] = 0;});
                produto.estoques.forEach(estoque => { novoFormato[estoque.nome] = estoque.quantidade;});
                return novoFormato;
            });

            setState(prev => ({ ...prev, produtosEstoques: dadosParaGrafico, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    useEffect(() => { carregarDashboardResumo(); carregarHistoricos(); carregarProdutosEstoques(); }, [carregarDashboardResumo, carregarHistoricos, carregarProdutosEstoques]);

    return { ...state, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' })) };
};
