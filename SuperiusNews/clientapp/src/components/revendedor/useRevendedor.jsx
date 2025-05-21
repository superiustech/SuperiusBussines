import { useState, useEffect, useCallback } from 'react';
import axios from 'axios';
import FormatadorValores from '../common/FormatadorValores';
import apiConfig from '../../Api';
import { useNavigate } from 'react-router-dom';

export const useRevendedor = (codigoRevendedor) => {
    const navigate = useNavigate();

    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', estoques: [], tipos: [], revendedor: [] });

    const carregarEstoque = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const url = `${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.pesquisarEstoquesSemRevendedor}`;
            const finalUrl = codigoRevendedor ? `${url}?nCdRevendedor=${codigoRevendedor}` : url;

            const response = await apiConfig.revendedor.axios.get(finalUrl);
            const data = response.data.estoques;
            setState(prev => ({ ...prev, estoques: data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, [codigoRevendedor]);

    const carregarTipos = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.revendedor.axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.tiposRevendedor}`);
            setState(prev => ({ ...prev, tipos: response.data.tipos, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const carregarRevendedor = async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.revendedor.axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.consultarRevendedor}/${codigoRevendedor}`, { headers: { 'Content-Type': 'application/json' } });

            if (response) {

                const dtoRevendedor = response.data.revendedor;
                const revendedorFormatado = {
                    nomeRevendedor: dtoRevendedor.nome || '',
                    estoque: dtoRevendedor.codigoEstoque?.toString() || '',
                    tipo: dtoRevendedor.codigoTipoRevendedor?.toString() || '',
                    percRevenda: dtoRevendedor.percentualRevenda.toString().replace(".", ",") || '0',
                    cpfCnpj: dtoRevendedor.cpfCnpj || '',
                    telefone: dtoRevendedor.telefone || '',
                    email: dtoRevendedor.email || '',
                    rua: dtoRevendedor.rua || '',
                    complemento: dtoRevendedor.complemento || '',
                    numero: dtoRevendedor.numero || '',
                    cep: dtoRevendedor.cep || ''
                };

                setState(prev => ({ ...prev, success: true, mensagem: "Revendedor consultado com sucesso!", loading: false, revendedor: revendedorFormatado }));
            }
            else if (response.status === 2) {
                setState(prev => ({ ...prev, error: true, mensagem: response.mensagem || "Ocorreu um erro ao consultar o revendedor.", loading: false, revendedor: null }));
            }
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false}));
            return null;
        }
    };

    const adicionarRevendedor = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const DTORevendedor = { 
                Codigo: FormatadorValores.converterParaInteiro(codigoRevendedor) || 0,
                CodigoEstoque: FormatadorValores.converterParaInteiro(formData.estoque) || null,
                CodigoTipoRevendedor: FormatadorValores.converterParaInteiro(formData.tipo) || 0,
                PercentualRevenda: FormatadorValores.converterParaDecimal(formData.percRevenda) || 0,
                Nome: formData.nomeRevendedor || '',
                CpfCnpj: FormatadorValores.removerFormatacao(formData.cpfCnpj) || '',
                Telefone: FormatadorValores.removerFormatacao(formData.telefone) || '',
                Email: formData.email || '',
                Rua: formData.rua || '',
                Complemento: formData.complemento || '',
                Numero: formData.numero || '',
                Cep: FormatadorValores.removerFormatacao(formData.cep) || ''
            };

            const response = await apiConfig.revendedor.axios.post(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.cadastrarRevendedor}`, DTORevendedor, {
                headers: { 'Content-Type': 'application/json' }
            });

            if (response.data.status === 1) {
                setState(prev => ({ ...prev, success: true, mensagem: "Revendedor cadastrado com sucesso!", loading: false }));
                carregarEstoque();
                navigate(`/administrador/revendedores`);
            } else { throw new Error(response.data.mensagem || "Erro ao salvar dados"); }
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false }));
        }
    };

    useEffect(() => {
        carregarEstoque();
        carregarTipos();
        if (codigoRevendedor) carregarRevendedor()
    }, [codigoRevendedor]);

    return { ...state, adicionarRevendedor, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};