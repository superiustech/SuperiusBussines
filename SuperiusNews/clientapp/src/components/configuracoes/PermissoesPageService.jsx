import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiConfig from '../../Api';

export const PermissoesPageService = () => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', permissoes: [], modal: { open: false, tipo: null, data: null, chave: null} });

    const carregarPermissoes = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.permissao.axios.get(`${apiConfig.permissao.baseURL}${apiConfig.permissao.endpoints.pesquisarPermissoes}`);
            setState(prev => ({ ...prev, permissoes: response.data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const editarPermissao = async (codigoPermissao) => {
        const permissaoSelecionada = state.permissoes.find( x => x.codigoPermissao === codigoPermissao);
        setState(prev => ({
            ...prev,
            modal: {
                open: true,
                tipo: 'edicao',
                data: permissaoSelecionada
            }
        }));
    };

    const editarPermissaoConfirmar = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.permissao.axios.post(apiConfig.permissao.endpoints.editarPermissao, formData,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarPermissoes();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const gerenciarFuncionalidades = async (codigoPermissao) => {
        setState(prev => ({ ...prev, loading: true }));
        const response = await apiConfig.permissao.axios.get(`${apiConfig.permissao.baseURL}${apiConfig.permissao.endpoints.funcionalidadesAssociadasCompleto}/` + codigoPermissao);
        setState(prev => ({
            ...prev,
            modal: {
                open: true,
                tipo: 'funcionalidades',
                data: response.data,
                chave: codigoPermissao
            }
        }));
        setState(prev => ({ ...prev, loading: false }));
    };

    const gerenciarFuncionalidadeConfirmar = async (codigoPermissao, arrCodigosFuncionalidades) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
             const dadosEnvio = { Codigo: Number(codigoPermissao), CodigosAssociacao: arrCodigosFuncionalidades.join(",")};
             const response = await apiConfig.permissao.axios.post( apiConfig.permissao.endpoints.associarDesassociarFuncionalidades, JSON.stringify(dadosEnvio),
             { headers: { 'Content-Type': 'application/json','Accept': 'application/json'}});

            carregarPermissoes();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false}));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false}));
        }
    };

    const inativarPermissoes = async (arrCodigosPermissoes) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosPermissoes);
            const response = await apiConfig.permissao.axios.post(apiConfig.permissao.endpoints.inativarPermissoes, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarPermissoes();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const ativarPermissoes = async (arrCodigosPermissoes) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosPermissoes);
            const response = await apiConfig.permissao.axios.post(apiConfig.permissao.endpoints.ativarPermissoes, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarPermissoes();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const fecharModal = () => {
        setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } }));
    };

    useEffect(() => { carregarPermissoes(); }, [carregarPermissoes]);

    return { ...state, editarPermissao, editarPermissaoConfirmar, fecharModal, ativarPermissoes, inativarPermissoes, carregarPermissoes, gerenciarFuncionalidades, gerenciarFuncionalidadeConfirmar, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};