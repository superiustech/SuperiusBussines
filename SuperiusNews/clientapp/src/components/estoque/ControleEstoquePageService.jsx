import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import apiConfig from '../../Api';
import FormatadorValores from '../common/FormatadorValores';

export const ControleEstoquePageService = (codigoEstoque) => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', historico: [], produtos: [], estoqueProdutos: [], modal: { open: false, tipo: null, data: null } });

    const carregarEstoque = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.estoqueProduto}/${codigoEstoque}`);
            setState(prev => ({ ...prev, estoqueProdutos: response.data.estoqueProduto, produtos: response.data.produtos, historico: response.data.historico, loading: false }));
            setState(prev => ({ ...prev, loading: false, error: false, success: false }));

        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, [codigoEstoque]);

    const abrirModal = (tipo, data = null) => {
        setState(prev => ({ ...prev, modal: { open: true, tipo, data } }));
    };

    const fecharModal = () => {
        setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } }));
    };

    const confirmar = async (formData) => {
        try {
            setState(prev => ({ ...prev, success: false, error: false , mensagem: '' }));

            if (formData.tipo === "entrada") // substituir por enums
            {
                await confirmarEntrada(formData)
            }
            else if (formData.tipo === "saida")  // substituir por enums
            {
                await confirmarSaida(formData)
            }

        } catch (error) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao registrar entrada." }));
        }
    };

    const confirmarEntrada = async (formData) => {
        try {
            setState(prev => ({ ...prev, loading: true }));
            const historicoFormatado = {
                Codigo: 0,
                CodigoProduto: FormatadorValores.converterParaInteiro(formData.produtoId),
                CodigoEstoqueOrigem: FormatadorValores.converterParaInteiro(codigoEstoque),
                CodigoEstoqueDestino: FormatadorValores.converterParaInteiro(codigoEstoque),
                QuantidadeMovimentada: FormatadorValores.converterParaInteiro(formData.quantidade),
                TipoMovimentacao: "1"
            };

            const response = await axios.post(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.movimentarEntradaSaida}`, historicoFormatado, { headers: { 'Content-Type': 'application/json' } });

            if (response.data.status === 1) {
                fecharModal();
                await carregarEstoque();
                setState(prev => ({ ...prev, success: true, mensagem: "Entrada de produto efetuada com sucesso!"}));
            } else {
                fecharModal();
                setState(prev => ({ ...prev, error: true, mensagem: response.data.mensagem }));
            }
            setState(prev => ({ ...prev, loading: false }));
        } catch (error) {
            fecharModal();
            setState(prev => ({ ...prev, error: true, mensagem: error.mensagem || "Ocorreu um erro ao processar sua solicitação."}));
        }
    };

    const confirmarSaida = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {

            const historicoFormatado = {
                Codigo: 0,
                CodigoProduto: FormatadorValores.converterParaInteiro(formData.produtoId),
                CodigoEstoqueOrigem: FormatadorValores.converterParaInteiro(codigoEstoque),
                CodigoEstoqueDestino: FormatadorValores.converterParaInteiro(codigoEstoque),
                QuantidadeMovimentada: FormatadorValores.converterParaInteiro(formData.quantidade),
                TipoMovimentacao: "2"
            };

            const response = await axios.post(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.movimentarEntradaSaida}`, historicoFormatado, { headers: { 'Content-Type': 'application/json' } });

            if (response.data.status === 1) {
                fecharModal();
                await carregarEstoque();
                setState(prev => ({ ...prev, success: true, mensagem: "Saída de produto efetuada com sucesso!" }));
            } else {
                fecharModal();
                setState(prev => ({ ...prev, error: true, mensagem: response.data.mensagem || "Ocorreu um erro ao inserir o produto." }));
            }
            setState(prev => ({ ...prev, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const deletarProduto = async (arrCodigosProdutos) => {
        setState(prev => ({ ...prev, loading: true }));
        try {

            const estoqueProduto = {
                codigoEstoque: FormatadorValores.converterParaInteiro(codigoEstoque),
                arrCodigosProdutos: String(arrCodigosProdutos)
            };

            const response = await axios.delete(
                `${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.removerEstoqueProduto}`,
                {
                    data: estoqueProduto,
                    headers: { 'Content-Type': 'application/json' }
                }
            );
            await carregarEstoque();
            if (response.data.status === 1) {
                fecharModal();
                await carregarEstoque();
                setState(prev => ({ ...prev, success: true, mensagem: "Inativação de produto efetuada com sucesso!" }));
            } else {
                fecharModal();
                setState(prev => ({ ...prev, error: true, mensagem: response.data.mensagem || "Ocorreu um erro ao inserir o produto." }));
            }
            setState(prev => ({ ...prev, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    useEffect(() => { carregarEstoque(); }, [carregarEstoque]);

    return { ...state, abrirModal, fecharModal, confirmar, carregarEstoque, deletarProduto, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};