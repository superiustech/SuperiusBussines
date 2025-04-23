const baseUrl = 'api/Produto'; 
import apiConfig from '../Api';

export const getProducts = async (page, pageSize, filters) => {
    const params = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString(),
        ...(filters.sNmProduto && { 'oFiltroRequest.sNmFiltro': filters.sNmProduto }),
        ...(filters.sDsProduto && { 'oFiltroRequest.sDsFiltro': filters.sDsProduto })
    });

    try {
        const response = await fetch(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.pesquisarProdutosComPaginacao}?${params}`);

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