import { useState, useEffect, useCallback } from 'react';
import axios from 'axios';
import FormatadorValores from '../common/FormatadorValores';
import apiConfig from '../../Api';

export const useRevendedor = (codigoRevendedor) => {

    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', estoques: [], tipos: [], revendedor: [] });

    const carregarEstoque = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const url = `${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.pesquisarEstoquesSemRevendedor}`;
            const finalUrl = codigoRevendedor ? `${url}?nCdRevendedor=${codigoRevendedor}` : url;

            const response = await axios.get(finalUrl);
            const data = response.data.estoques;
            setState(prev => ({ ...prev, estoques: data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, [codigoRevendedor]);

    const carregarTipos = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.tiposRevendedor}`);
            const data = await response.data.tipos;
            setState(prev => ({ ...prev, tipos: data.result, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const carregarRevendedor = async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.consultarRevendedor}/${codigoRevendedor}`, { headers: { 'Content-Type': 'application/json' } });

            if (!response.data.success) { throw new Error(response.data.message || "Erro ao carregar revendedor");}

            const formData = response.data.revendedor.result;
            const revendedorFormatado = {
                nomeRevendedor: formData.sNmRevendedor?.toString() || '',
                estoque: formData.nCdEstoque?.toString() || '',
                tipo: formData.nCdTipoRevendedor?.toString() || '',
                percRevenda: formData.dPcRevenda?.toString().replace(".", ",") || '',
                cpfcnpj: formData.sNrCpfCnpj?.toString() || '',
                telefone: formData.sTelefone?.toString() || '',
                email: formData.sEmail?.toString() || '',
                rua: formData.sDsRua?.toString() || '',
                complemento: formData.sDsComplemento?.toString() || '',
                numero: formData.sNrNumero?.toString() || '',
                cep: formData.sCdCep?.toString() || ''
            };

            setState(prev => ({ ...prev, success: true, mensagem: "Revendedor consultado com sucesso!", loading: false, revendedor: revendedorFormatado}));

            return revendedorFormatado;
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false}));
            return null;
        }
    };

    const adicionarRevendedor = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const revendedorFormatado = {
                nCdRevendedor: FormatadorValores.converterParaInteiro(codigoRevendedor) || 0,
                nCdEstoque: FormatadorValores.converterParaInteiro(formData.estoque) || 0,
                nCdTipoRevendedor: FormatadorValores.converterParaInteiro(formData.tipo) || 0,
                dPcRevenda: FormatadorValores.converterParaDecimal(formData.percRevenda) || 0,
                sNmRevendedor: formData.nomeRevendedor || '',
                sNrCpfCnpj: FormatadorValores.removerFormatacao(formData.cpfcnpj) || '',
                sTelefone: FormatadorValores.removerFormatacao(formData.telefone) || '',
                sEmail: formData.email || '',
                sDsRua: formData.rua || '',
                sDsComplemento: formData.complemento || '',
                sNrNumero: formData.numero || '',
                sCdCep: formData.cep || ''
            };

            const response = await axios.post(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.cadastrarRevendedor}`, revendedorFormatado, {
                headers: { 'Content-Type': 'application/json' }
            });

            if (response.data.success) {
                setState(prev => ({ ...prev, success: true, mensagem: "Revendedor cadastrado com sucesso!", loading: false }));
                carregarEstoque();
                return true;
            } else { throw new Error(response.data.message || "Erro ao salvar dados"); }
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    useEffect(() => {
        carregarEstoque();
        carregarTipos();
        if (codigoRevendedor) carregarRevendedor()
    }, [codigoRevendedor]);

    return { ...state, adicionarRevendedor, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};