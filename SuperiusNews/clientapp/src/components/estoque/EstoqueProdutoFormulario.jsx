import React, { useState } from 'react';
import InputMask from 'react-input-mask';
import FlashMessage from '../ui/FlashMessage';

const EstoqueProdutoFormulario = ({ produtos, loading, error, success, mensagem, onAdicionarProduto, clearMessages }) => {
    const [formData, setFormData] = useState({ quantidadeMinima: '', quantidadeEstoque: '', valorCusto: '', valorVenda: '', produtoId: ''});
    const [validated, setValidated] = useState(false);

    const handleChange = (e) => {
        clearMessages();
        setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const handleSelectChange = (event) => {
        clearMessages();
        setFormData(prev => ({ ...prev, produtoId: event.target.value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const form = e.currentTarget;

        if (form.checkValidity() === false) {
            e.stopPropagation();
            setValidated(true);
            return;
        }

        setValidated(true);
        const success = await onAdicionarProduto(formData);

        if (success) {
            setFormData({ quantidadeMinima: '', quantidadeEstoque: '', valorCusto: '', valorVenda: '', produtoId: '' });
            setValidated(false);
        }
    };

    return (
        <>
        {success && (<FlashMessage message={mensagem} type="success" duration={3000} />)}
        {error && (<FlashMessage message={mensagem} type="error" duration={3000} />)}
        <form id="formEstoque" className={`row g-3 needs-validation ${validated ? 'was-validated' : ''}`} noValidate onSubmit={handleSubmit}>
        <div className="col-md-12">
            <label htmlFor="produtoSelecionado" className="form-label">Selecione um produto</label>
            <select class="form-control" id="produtoSelect" name="produtoId" onChange={handleSelectChange} required>
                <option value="" disabled selected></option>
                {produtos.map((produto, index) => ( <option value={produto.nCdProduto} data-nome={produto.sNmProduto} data-descricao={produto.sDsProduto}> {produto.sCdProduto} - {produto.sNmProduto}</option>))}
            </select>
        </div>

        <div className="col-md-12">
            <label htmlFor="quantidadeMinima" className="form-label">Quantidade mínima</label>
            <InputMask mask="99999999" maskChar={null} className="form-control numero-inteiro quantidade-input" id="quantidadeMinima" name="quantidadeMinima" value={formData.quantidadeMinima} onChange={handleChange} required />
            <div className="invalid-feedback">Por favor, insira uma quantidade válida (ex: 10).</div>
        </div>

        <div className="col-md-12">
            <label htmlFor="quantidadeEstoque" className="form-label">Quantidade estoque (inicial)</label>
            <InputMask mask="99999999" maskChar={null} className="form-control numero-inteiro quantidade-input" id="quantidadeEstoque" name="quantidadeEstoque" value={formData.quantidadeEstoque} onChange={handleChange} required />
            <div className="invalid-feedback">Por favor, insira uma quantidade válida (ex: 100).</div>
        </div>

        <div className="col-md-12">
            <label htmlFor="valorCusto" className="form-label">Valor de custo</label>
            <input type="text" className="form-control money money-input" id="valorCusto" name="valorCusto" value={formData.valorCusto} onChange={handleChange} required />
            <div className="invalid-feedback">Por favor, insira um valor válido (ex: R$ 12,75).</div>
        </div>

        <div className="col-md-12">
            <label htmlFor="valorVenda" className="form-label">Valor de venda</label>
            <input type="text" className="form-control money money-input" id="valorVenda" name="valorVenda" value={formData.valorVenda} onChange={handleChange} required />
            <div className="invalid-feedback">Por favor, insira um valor válido (ex: R$ 15,99).</div>
        </div>

        <div className="col-md-12">
            <button type="submit" className="btn btn-primary" disabled={loading}> Salvar Dados </button>
        </div>
        </form>
        </>
    );
};

export default EstoqueProdutoFormulario;