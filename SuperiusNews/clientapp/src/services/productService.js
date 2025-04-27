const baseUrl = 'api/Produto'; 
import apiConfig from '../Api';
import axios from 'axios';

export const getProducts = async () => {
    try {
        const response = await fetch(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.pesquisarProdutos}`);
        const text = await response.text();
        if (!response.ok) {
            throw new Error(`Erro ${response.status}: ${text}`);
        }
        return JSON.parse(text);
    } catch (error) {
        console.error("Erro na chamada da API:", error);
        throw error;
    }
};

export const deletarProdutos = async (arrCodigoProdutos) => {
    try {
        const dadosEnvio = JSON.stringify(String(arrCodigoProdutos));
        const response = await axios.delete(
            `${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.excluirProdutos}`,
            {
                data: dadosEnvio,
                headers: { 'Content-Type': 'application/json' }
            }
        );
        return true;
    } catch (err) {
        return false;
    }
};