import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import FormatadorValores from '../common/FormatadorValores';
import apiConfig from '../../Api';

export const RevendedorPageService = () => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', revendedores: [], modal: { open: false, tipo: null, data: null, chave: null } });

    const carregarRevendedores = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await apiConfig.revendedor.axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.pesquisarRevendedores}`);
            const data = response.data.revendedores;
            setState(prev => ({ ...prev, revendedores: data, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const editarRevendedor = async (codigoRevendedor) => { navigate(`/administrador/editar-revendedor/${codigoRevendedor}`);  };

    const excluirRevendedor = async (arrCodigoRevendedor) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = JSON.stringify(String(arrCodigoRevendedor));
            const response = await apiConfig.revendedor.axios.delete(
                `${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.excluirRevendedores}`,
                {
                    data: dadosEnvio, 
                    headers: { 'Content-Type': 'application/json' }
                }
            );
            carregarRevendedores();
            setState(prev => ({ ...prev, success: true, mensagem: response.data.message, loading: false }));
        } catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: err.message || "Erro na comunicação com o servidor", loading: false }));
            return false;
        }
    };

    const gerenciarUsuarios = async (codigoRevendedor) => {
        setState(prev => ({ ...prev, loading: true }));
        const response = await apiConfig.revendedor.axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.usuariosAssociadosCompleto}/` + codigoRevendedor);
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'usuarios', data: response.data, chave: codigoRevendedor } }));
        setState(prev => ({ ...prev, loading: false }));
    };

    const gerenciarUsuariosConfirmar = async (codigoRevendedor, arrCodigosUsuarios) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = { CodigosRevendedores: String(codigoRevendedor), CodigosUsuarios: arrCodigosUsuarios.join(",") };
            const response = await apiConfig.revendedor.axios.post(apiConfig.revendedor.endpoints.associarDesassiarUsuarios, JSON.stringify(dadosEnvio), { headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' } });
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false }));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false }));
        }
    };

    const fecharModal = () => { setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } })); };

    useEffect(() => { carregarRevendedores(); }, [carregarRevendedores]);

    return { ...state, editarRevendedor, excluirRevendedor, carregarRevendedores, fecharModal, gerenciarUsuarios, gerenciarUsuariosConfirmar, clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};