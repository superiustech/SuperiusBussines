import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiConfig from '../../Api';

export const UsuarioPageService = () => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', usuarios: [], modal: { open: false, tipo: null, data: null, chave: null } });

    const carregarUsuarios = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.usuario.axios.get(`${apiConfig.usuario.baseURL}${apiConfig.usuario.endpoints.pesquisarUsuarios}`);
            setState(prev => ({ ...prev, usuarios: response.data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const editarUsuario = async (usuario) => {
        const usuarioSelecionado = state.usuarios.find(x => x.usuario === usuario);
        setState(prev => ({
            ...prev,
            modal: {
                open: true,
                tipo: 'edicao',
                data: usuarioSelecionado
            }
        }));
    };

    const novoUsuario = async () => {
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'cadastrar', data: null}}));
    };

    const novoUsuarioConfirmar = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.usuario.axios.post(apiConfig.usuario.endpoints.editarUsuario, formData,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarUsuarios();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const editarUsuarioConfirmar = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.usuario.axios.put(apiConfig.usuario.endpoints.editarUsuario, formData,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarUsuarios();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const gerenciarUsuarios = async (usuario) => {
        setState(prev => ({ ...prev, loading: true }));
        const response = await apiConfig.usuario.axios.get(`${apiConfig.usuario.baseURL}${apiConfig.usuario.endpoints.perfisAssociadosCompleto}/` + usuario);
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'perfis', data: response.data, chave: usuario}}));
        setState(prev => ({ ...prev, loading: false }));
    };

    const gerenciarUsuarioConfirmar = async (usuario, arrCodigosperfis) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = { Codigo: usuario, CodigosAssociacao: arrCodigosperfis.join(",") };
            const response = await apiConfig.usuario.axios.post(apiConfig.usuario.endpoints.associarDesassociarPerfis, JSON.stringify(dadosEnvio),
                { headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' } });

            carregarUsuarios();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false }));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false }));
        }
    };

    const inativarUsuarios = async (arrCodigosUsuarios) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosUsuarios);
            const response = await apiConfig.usuario.axios.post(apiConfig.usuario.endpoints.inativarUsuarios, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarUsuarios();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const ativarUsuarios = async (arrCodigosUsuarios) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosUsuarios);
            const response = await apiConfig.usuario.axios.post(apiConfig.usuario.endpoints.ativarUsuarios, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarUsuarios();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const fecharModal = () => { setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } })); };

    useEffect(() => { carregarUsuarios(); }, [carregarUsuarios]);

    return { ...state, editarUsuario, editarUsuarioConfirmar, fecharModal, ativarUsuarios, inativarUsuarios, carregarUsuarios, gerenciarUsuarios, gerenciarUsuarioConfirmar, novoUsuario, novoUsuarioConfirmar, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' })) };
};