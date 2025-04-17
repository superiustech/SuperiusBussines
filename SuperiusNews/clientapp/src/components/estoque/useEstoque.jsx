import { useState, useEffect, useCallback } from 'react';
import axios from 'axios';
import FormatadorValores from '../common/FormatadorValores';
import apiConfig from '../../Api';

export const useEstoque = (codigoEstoque) => {

    const [state, setState] = useState({ produtos: [], estoqueProdutos: [], loading: false, error: null, success: false, mensagem: '' });
    const obterEstoqueCompleto = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.baseURL}${apiConfig.endpoints.estoqueProduto}/${codigoEstoque}`);
            setState(prev => ({
                ...prev,
                produtos: response.data.todosProdutos,
                estoqueProdutos: response.data.produtos,
                loading: false
            }));
        } catch (err) {
            setState(prev => ({
                ...prev,
                error: true,
                mensagem: "Erro ao carregar os dados",
                loading: false
            }));
        }
    }, [codigoEstoque]);

    const adicionarProduto = async (produtoData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const produtoFormatado = {
                nCdEstoque: FormatadorValores.converterParaInteiro(codigoEstoque),
                nCdProduto: FormatadorValores.converterParaInteiro(produtoData.produtoId),
                dQtMinima: FormatadorValores.converterParaInteiro(produtoData.quantidadeMinima),
                dQtEstoque: FormatadorValores.converterParaInteiro(produtoData.quantidadeEstoque),
                dVlCusto: FormatadorValores.converterParaDecimal(produtoData.valorCusto),
                dVlVenda: FormatadorValores.converterParaDecimal(produtoData.valorVenda)
            };

            const response = await axios.post(`${apiConfig.baseURL}${apiConfig.endpoints.adicionarEstoqueProduto}`, produtoFormatado, { headers: { 'Content-Type': 'application/json' } });

            if (response.data.success) {
                setState(prev => ({
                    ...prev,
                    success: true,
                    mensagem: "Dados de estoque salvos com sucesso!",
                    loading: false
                }));
                await obterEstoqueCompleto();
                return true;
            } else { throw new Error(response.data.message || "Erro ao salvar dados"); }
        } catch (err) {
            setState(prev => ({
                ...prev,
                error: true,
                mensagem: err.message || "Erro na comunicação com o servidor",
                loading: false
            }));
            return false;
        }
    };

    const removerProdutoEstoque = async (nCdProduto) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const estoqueProduto = {
                nCdEstoque: FormatadorValores.converterParaInteiro(codigoEstoque),
                nCdProduto: FormatadorValores.converterParaInteiro(nCdProduto)
            };

            const response = await axios.delete(
                `${apiConfig.baseURL}${apiConfig.endpoints.removerEstoqueProduto}`,
                {
                    data: estoqueProduto,  
                    headers: {'Content-Type': 'application/json'}
                }
            )
            if (response.data.success) {
                setState(prev => ({ ...prev, success: true, mensagem: "Dados de estoque removidos com sucesso!", loading: false }));
                await obterEstoqueCompleto();
                return true;
            } else { throw new Error(response.data.message || "Erro ao salvar dados"); }
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    useEffect(() => { obterEstoqueCompleto(); }, [obterEstoqueCompleto]);

    return { ...state, obterEstoqueCompleto, adicionarProduto, removerProdutoEstoque, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))
    };
};