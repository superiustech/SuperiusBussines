import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useEstoque } from '../components/estoque/useEstoque';
import EstoqueProdutoForm from '../components/estoque/EstoqueProdutoFormulario';
import EstoqueProdutoTable from '../components/estoque/EstoqueProdutoTable';

const EstoqueProdutoPage = () => {
    const { codigoEstoque } = useParams();
    const navigate = useNavigate();
    const { produtos, estoqueProdutos, loading, error, success, mensagem, obterEstoqueCompleto, adicionarProduto, removerProdutoEstoque, clearMessages } = useEstoque(codigoEstoque);

    return (
        <div className="container d-flex justify-content-center">
            <div className="p-4" style={{ maxWidth: '100%', width: '100%' }}>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h5>Estoque - Gerenciamento de Produtos</h5>
            </div>

            <EstoqueProdutoForm produtos={produtos} loading={loading} error={error} success={success} mensagem={mensagem} onAdicionarProduto={adicionarProduto} clearMessages={clearMessages}/><hr />
            <EstoqueProdutoTable estoqueProdutos={estoqueProdutos} loading={loading} onRefresh={obterEstoqueCompleto} onRemoverProdutoEstoque={removerProdutoEstoque}/>

            <button onClick={() => navigate(`/administrador/editar-estoque/${codigoEstoque}`)} className="btn btn-secondary"> Voltar </button>
            <button onClick={() => navigate('/administrador/estoques')} className="btn btn-primary"> Finalizar </button>
            </div>
        </div>
    );
};

export default EstoqueProdutoPage;