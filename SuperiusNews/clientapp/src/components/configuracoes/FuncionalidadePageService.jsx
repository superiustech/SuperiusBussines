import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiConfig from '../../Api';

export const FuncionalidadePageService = () => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', funcionalidades: [], modal: { open: false, tipo: null, data: null } });

    const carregarFuncionalidades = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.funcionalidade.axios.get(`${apiConfig.funcionalidade.baseURL}${apiConfig.funcionalidade.endpoints.pesquisarFuncionalidades}`);
            setState(prev => ({ ...prev, funcionalidades: response.data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const editarFuncionalidade = async (codigoFuncionalidade) => {
        const funcionalidadeSelecionada = state.funcionalidades.find(
            x => x.codigoFuncionalidade === codigoFuncionalidade
        );

        setState(prev => ({
            ...prev,
            modal: {
                open: true,
                tipo: 'edicao',
                data: funcionalidadeSelecionada
            }
        }));
    };

    const editarFuncionalidadeConfirmar = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.funcionalidade.axios.post(apiConfig.funcionalidade.endpoints.editarFuncionalidade, formData,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarFuncionalidades();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const inativarFuncionalidades = async (arrCodigosFuncionalidades) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosFuncionalidades); 
            const response = await apiConfig.funcionalidade.axios.post(apiConfig.funcionalidade.endpoints.inativarFuncionalidades, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarFuncionalidades();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const ativarFuncionalidades = async (arrCodigosFuncionalidades) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(arrCodigosFuncionalidades);
            const response = await apiConfig.funcionalidade.axios.post(apiConfig.funcionalidade.endpoints.ativarFuncionalidades, dadosEnvio,
                { headers: { 'Content-Type': 'application/json' } }
            );
            carregarFuncionalidades();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.mensagem, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.mensagem || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const fecharModal = () => {
        setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } }));
    };

    useEffect(() => { carregarFuncionalidades(); }, [carregarFuncionalidades]);

    return { ...state, editarFuncionalidade, editarFuncionalidadeConfirmar, fecharModal, ativarFuncionalidades, inativarFuncionalidades, carregarFuncionalidades, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};