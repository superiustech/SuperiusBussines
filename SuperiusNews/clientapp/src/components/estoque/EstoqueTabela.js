import React from 'react';
const EstoqueTabela = ({ estoques, onEstoqueClick, onDeleteClick, currentPage, totalPages, onPageChange }) => {
    return (
        <div className="mt-4" style={{ maxHeight: '800px', overflow: 'scroll' }}>
            <label className="form-label">Estoque</label>
            <div className="table-responsive">
                <table className="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Imagem</th>
                            <th scope="col">Nome</th>
                            <th scope="col">Descrição</th>
                            <th scope="col">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {estoques.length > 0 ? (
                            estoques.map((item) => (
                                <tr key={item.nCdEstoque}>
                                    <th scope="row">{item.nCdEstoque}</th>
                                    <td><img src="https://i.postimg.cc/RZXH7Gwb/sem-imagem.png" alt="Imagem" style={{ width: '100px', height: 'auto' }} /></td>
                                    <td>{item.sNmEstoque}</td>
                                    <td>{item.sDsEstoque}</td>
                                    <td>
                                        <button onClick={() => onDeleteClick(item.nCdEstoque)}  className="btn btn-sm btn-danger"> Excluir </button>
                                        <button onClick={() => onEstoqueClick(item.nCdEstoque)} className="btn btn-primary btn-sm ms-2"> Abrir Estoque </button>
                                    </td>
                                </tr>
                            ))) : (
                            <tr>
                                <td colSpan="5" className="text-center"> Nenhum estoque encontrado </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>

            <div className="pagination-controls d-flex justify-content-center mt-3">
                <button onClick={() => onPageChange(currentPage - 1)} disabled={currentPage <= 1} className="btn btn-secondary"> Anterior </button>
                <div className="mx-3">
                    {Array.from({ length: totalPages }, (_, i) => i + 1).map(page => (
                        <button key={page} onClick={() => onPageChange(page)} className={`btn btn-secondary ms-1 ${currentPage === page ? 'active' : ''}`}> {page} </button>
                    ))}
                </div>
                <button onClick={() => onPageChange(currentPage + 1)} disabled={currentPage >= totalPages} className="btn btn-secondary"> Próximo </button>
            </div>
        </div>
    );
};

export default EstoqueTabela;