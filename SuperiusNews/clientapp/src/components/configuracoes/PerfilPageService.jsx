import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiConfig from '../../Api';

export const PerfisPageService = () => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', perfis: [], modal: { open: false, tipo: null, data: null, chave: null } });

    const carregarPerfis = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.perfil.axios.get(`${apiConfig.perfil.baseURL}${apiConfig.perfil.endpoints.pesquisarPerfis}`);
            setState(prev => ({ ...prev, perfis: response.data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const editarPerfil = async (codigoPerfil) => {
        const perfilSelecionado = state.perfis.find(x => x.codigoPerfil === codigoPerfil);
        setState(prev => ({
            ...prev,
            modal: {
                open: true,
                tipo: 'edicao',
                data: perfilSelecionado
            }
        }));
    };

    const editarPerfilConfirmar = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.perfil.axios.post(apiConfig.perfil.endpoints.editarPerfil, formData,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarPerfis();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const gerenciarPermissoes = async (codigoPerfil) => {
        setState(prev => ({ ...prev, loading: true }));
        const response = await apiConfig.Permissao.axios.get(`${apiConfig.perfil.baseURL}${apiConfig.perfil.endpoints.PermissoesAssociadasCompleto}/` + codigoPerfil);
        setState(prev => ({
            ...prev,
            modal: {
                open: true,
                tipo: 'permissoes',
                data: response.data,
                chave: codigoPerfil
            }
        }));
        setState(prev => ({ ...prev, loading: false }));
    };

    const gerenciarPermissaoConfirmar = async (codigoPerfil, arrCodigosPermissoes) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = { Codigo: Number(codigoPerfil), CodigosAssociacao: arrCodigosPermissoes.join(",") };
            const response = await apiConfig.perfil.axios.post(apiConfig.perfil.endpoints.associarDesassociarPermissoes, JSON.stringify(dadosEnvio),
                { headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' } });

            carregarPerfis();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false }));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false }));
        }
    };

    const inativarPerfis = async (arrCodigosPerfis) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosPerfis);
            const response = await apiConfig.perfil.axios.post(apiConfig.perfil.endpoints.inativarPerfis, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarPerfis();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const ativarPerfis = async (arrCodigosPerfis) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosPerfis);
            const response = await apiConfig.perfil.axios.post(apiConfig.perfil.endpoints.ativarPerfis, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarPerfis();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const fecharModal = () => {
        setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } }));
    };

    useEffect(() => { carregarPerfis(); }, [carregarPerfis]);

    return { ...state, editarPerfil, editarPerfilConfirmar, fecharModal, ativarPerfis, inativarPerfis, carregarPerfis, gerenciarPermissoes, gerenciarPermissaoConfirmar, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' })) };
};