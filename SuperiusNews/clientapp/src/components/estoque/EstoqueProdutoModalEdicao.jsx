import React from 'react';
import { Modal, Button } from 'react-bootstrap';
import InputMask from 'react-input-mask';

const EstoqueProdutoModalEdicao = ({ show, onHide, produto, loading, onEditarProduto, error, success, mensagem}) => {
  const [formData, setFormData] = React.useState({ ...produto });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onEditarProduto(formData);
  };

    return (
        <>
        {success && (<FlashMessage message={mensagem} type="success" duration={3000} />)}
        {error && (<FlashMessage message={mensagem} type="error" duration={3000} />)}

        <div class="modal fade" id="editarProdutoModal" tabindex="-1" aria-labelledby="editarProdutoModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="editarProdutoModalLabel">Editar Produto</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <form id="formEditarProduto" class="row g-3 needs-validation" novalidate>
                          <div className="col-md-12">
                                <label htmlFor="quantidadeMinima" className="form-label">Quantidade mínima</label>
                                <InputMask mask="99999999" maskChar={null} className="form-control numero-inteiro quantidade-input" id="quantidadeMinima" name="quantidadeMinima" value={produtoParaEditar.quantidadeMinima} onChange={handleChange} required />
                                <div className="invalid-feedback">Por favor, insira uma quantidade válida (ex: 10).</div>
                            </div>

                            <div className="col-md-12">
                                <label htmlFor="quantidadeEstoque" className="form-label">Quantidade estoque (inicial)</label>
                                <InputMask mask="99999999" maskChar={null} className="form-control numero-inteiro quantidade-input" id="quantidadeEstoque" name="quantidadeEstoque" value={produtoParaEditar.quantidadeEstoque} onChange={handleChange} required />
                                <div className="invalid-feedback">Por favor, insira uma quantidade válida (ex: 100).</div>
                            </div>

                            <div className="col-md-12">
                                <label htmlFor="valorCusto" className="form-label">Valor de custo</label>
                                <input type="text" className="form-control money money-input" id="valorCusto" name="valorCusto" value={produtoParaEditar.valorCusto} onChange={handleChange} required />
                                <div className="invalid-feedback">Por favor, insira um valor válido (ex: R$ 12,75).</div>
                            </div>

                            <div className="col-md-12">
                                <label htmlFor="valorVenda" className="form-label">Valor de venda</label>
                                <input type="text" className="form-control money money-input" id="valorVenda" name="valorVenda" value={produtoParaEditar.valorVenda} onChange={handleChange} required />
                                <div className="invalid-feedback">Por favor, insira um valor válido (ex: R$ 15,99).</div>
                            </div>
                            <div class="modal-footer">
                                <button type="submit" className="btn btn-primary" disabled={loading}> Salvar Dados </button>
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        </>
    );
};

export default EstoqueProdutoModalEdicao;