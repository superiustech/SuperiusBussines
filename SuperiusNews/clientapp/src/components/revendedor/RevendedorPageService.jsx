import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import FormatadorValores from '../common/FormatadorValores';
import apiConfig from '../../Api';

export const RevendedorPageService = () => {
    const navigate = useNavigate();
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', revendedores: [] });

    const carregarRevendedores = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.revendedor.baseURL}${apiConfig.revendedor.endpoints.pesquisarRevendedores}`);
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
            const response = await axios.delete(
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

    useEffect(() => { carregarRevendedores(); }, [carregarRevendedores]);

    return { ...state, editarRevendedor, excluirRevendedor, carregarRevendedores , clearMessages: () => setState(prev => ({ ...prev, error: false, success: false, mensagem: '' }))};
};