const baseUrl = 'api/Produto'; 
import apiConfig from '../Api';
import axios from 'axios';

export const getProducts = async () => {
    try {
        const response = await apiConfig.produto.axios.get(
            `${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.pesquisarProdutos}`
        );

        if (response.status !== 200) {
            throw new Error(`Erro ${response.status}: ${response.data}`);
        }

        return response.data;
    } catch (error) {
        console.error("Erro na chamada da API:", error);
        throw error;
    }
};


export const deletarProdutos = async (arrCodigoProdutos) => {
    try {
        const dadosEnvio = JSON.stringify(String(arrCodigoProdutos));
        const response = await apiConfig.produto.axios.delete(
            `${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.excluirProdutos}`,
            {
                data: dadosEnvio,
                headers: { 'Content-Type': 'application/json' }
            }
        );
        return response;
    } catch (err) {
        return null;
    }
};