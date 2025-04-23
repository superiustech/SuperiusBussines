import React from 'react';

const EstoqueProdutoTable = ({ estoqueProdutos, loading, onRefresh, onRemoverProdutoEstoque }) => {
    const handleEditar = (produtoId) => {};
    const handleRemover = async (produtoId) => { const success = await onRemoverProdutoEstoque(produtoId);};

    return (
        <>
        <div className="d-flex justify-content-between align-items-center mb-3">
            <h5>Lista de Produtos neste Estoque</h5>
            <button onClick={onRefresh} className="btn btn-sm btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'} </button>
        </div>

        <div className="table-responsive" style={{ maxHeight: '350px', overflow: 'auto' }}>
        <table className="table table-bordered table-striped table-hover">
            <thead className="table-dark">
                <tr>
                <th>#</th>
                <th>Nome Produto</th>
                <th>Descrição</th>
                <th>Qt. Mínima</th>
                <th>Qt. Estoque</th>
                <th>Vl. Venda</th>
                <th>Vl. Custo</th>
                <th>Ações</th>
                </tr>
            </thead>
            <tbody>
                {estoqueProdutos.length > 0 ? (estoqueProdutos.map((item) => (
                    <tr key={`${item.nCdProduto}-${item.nCdEstoque}`}>
                    <td>{item.nCdProduto}</td>
                    <td>{item.sNmProduto || 'N/A'}</td>
                    <td>{item.sDsProduto || 'N/A'}</td>
                    <td>{item.dQtMinima || '0'}</td>
                    <td>{item.dQtEstoque || '0'}</td>
                    <td>{item.dVlVenda}</td>
                    <td>{item.dVlCusto}</td><td>
                    <div className="btn-group btn-group-sm">
                    <button className="btn btn-primary" onClick={() => handleEditar(item.nCdProduto)}>Editar</button>
                    <button className="btn btn-danger"  onClick={() => handleRemover(item.nCdProduto)}>Remover</button>
                    </div>
                    </td>
                    </tr>
                )))
                : (<tr> <td colSpan="8" className="text-center"> {loading ? 'Carregando...' : 'Nenhum produto encontrado'}</td></tr>)}
            </tbody>
        </table>
        </div>
        </>
    );
};

export default EstoqueProdutoTable;