import Loading from '../ui/Loading';
import React, { useState, useEffect } from 'react';
import FlashMessage from '../ui/FlashMessage';
import { IMaskInput } from 'react-imask';

const FormularioRevendedor = ({ loading, error, success, mensagem, estoques, tipos, revendedor, onAdicionarRevendedor, clearMessages }) => {

    const [validated, setValidated] = useState(false);
    const [formData, setFormData] = useState({ nomeRevendedor: '', estoque: '', tipo: '' , percRevenda: '', cpfcnpj: '', telefone: '' , email: '', rua: '', complemento: '', numero: '', cep: '' });

    const handleChange = (e) => {
        clearMessages()
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const form = e.currentTarget;

        if (form.checkValidity() === false) {
            e.stopPropagation();
            setValidated(true);
            return;
        }

        onAdicionarRevendedor(formData);
        clearMessages();
        setFormData({ nomeRevendedor: '', estoque: '', tipo: '', percRevenda: '', cpfcnpj: '', telefone: '', email: '', rua: '', complemento: '', numero: '', cep: '' });
    };

    useEffect(() => {
        if (revendedor) { setFormData(revendedor); }
    }, [revendedor]); 

    return (
        <div className="container d-flex justify-content-center">
            <div className="p-4" style={{ maxWidth: '70%', width: '100%' }}>
                {success && (<FlashMessage message={mensagem} type="success" duration={3000} />)}
                {error   && (<FlashMessage message={mensagem} type="error" duration={3000} />)} 
                {loading  ? (<Loading show={true} />) : (
                    <form id="formEstoque" className={`row g-3 needs-validation ${validated ? 'was-validated' : ''}`} noValidate onSubmit={handleSubmit}>
                        <div className="col-md-12">
                            <label htmlFor="nomeRevendedor" className="form-label">Nome/Razão Social</label>
                            <input type="text" className="form-control" id="nomeRevendedor" name="nomeRevendedor" value={formData.nomeRevendedor} onChange={handleChange} required />
                            <div className="valid-feedback"> Nome válido! </div>
                            <div className="invalid-feedback"> Por favor, insira o nome ou razão social do revendedor. </div>
                        </div>

                        <div className="col-md-8">
                            <label htmlFor="estoque" className="form-label">Estoque</label>
                            <select className="form-select unidade" id="estoque" name="estoque" value={formData.estoque} onChange={handleChange}>
                                <option selected disabled value="">Escolha...</option>
                                {estoques.map((item) => (<option key={item.codigo} value={item.codigo}> {item.codigo}) {item.nome} </option>))}
                            </select>
                            <div className="invalid-feedback"> Por favor, selecione um estoque para o revendedor. </div>
                        </div>

                        <div className="col-md-4">
                            <label htmlFor="percRevenda" className="form-label">Perc. de Revenda</label>
                            <div className="input-group has-validation">
                                <span className="input-group-text" id="inputGroupPrepend">%</span>
                                <IMaskInput mask={Number} radix="," scale={2} thousandsSeparator="." padFractionalZeros={true} max={100} normalizeZeros={true} mapToRadix={["."]} className="form-control" id="percRevenda"
                                 name="percRevenda" value={formData.percRevenda} onAccept={(value) => handleChange({ target: { name: 'percRevenda', value } })} aria-describedby="inputGroupPrepend" required />
                                <div className="invalid-feedback"> Por favor, insira o Percentual de Revenda do revendedor. </div>
                            </div>
                        </div>

                        <div className="col-md-6">
                            <label htmlFor="tipo" className="form-label">Tipo</label>
                            <select className="form-select unidade" id="tipo" name="tipo" value={formData.tipo} onChange={handleChange} required >
                                <option selected disabled value="">Escolha...</option>
                                {tipos.map((item) => (<option key={item.codigo} value={item.codigo}> {item.codigo}) {item.nome}</option>))}
                            </select>
                            <div className="invalid-feedback"> Por favor, selecione o tipo do revendedor. </div>
                        </div>


                        <div className="col-md-6">
                            <label htmlFor="cpfcnpj" className="form-label">CPF/CNPJ</label>
                            <div className="input-group has-validation">
                                <IMaskInput key={formData.tipo} mask={ Number(formData.tipo) === 1 ? '00.000.000/0000-00' : Number(formData.tipo) === 2 ? '000.000.000-00' : '' }
                                 disabled={formData.tipo === undefined} className="form-control" id="cpfcnpj" name="cpfcnpj" value={formData.cpfCnpj} onAccept={(value) => setFormData({ ...formData, cpfCnpj: value })} unmask={false} required/>
                                <div className="invalid-feedback"> Por favor, insira o CPF/CNPJ do revendedor.</div>
                            </div>
                        </div>

                        <div className="col-md-4">
                            <label htmlFor="telefone" className="form-label">Telefone</label>
                            <div className="input-group has-validation">
                                <span className="input-group-text" id="inputGroupPrepend">#</span>
                                <IMaskInput mask="(00) 00000-0000" placeholder="(00) 00000-0000" className="form-control" name="telefone" value={formData.telefone} onAccept={(value) => setFormData({ ...formData, telefone: value })} required/>
                                <div className="invalid-feedback">  Por favor, insira o telefone do revendedor.. </div>
                            </div>
                        </div>

                        <div className="col-md-8">
                            <label htmlFor="email" className="form-label">E-mail</label>
                            <input type="text" className="form-control" id="email" name="email" value={formData.email} onChange={handleChange} />
                            <div className="invalid-feedback"> Por favor, insira o e-mail do revendedor. </div>
                        </div>

                        <br />
                        <h6 className="mt-12"> Endereço
                            <span className="badge bg-secondary"><i className="fas fa-gear"></i></span>
                        </h6>

                        <div className="col-md-6">
                            <label htmlFor="rua" className="form-label">Rua</label>
                            <input type="text" className="form-control" id="rua" name="rua" value={formData.rua} onChange={handleChange} required />
                            <div className="invalid-feedback"> Por favor, insira a rua. </div>
                        </div>

                        <div className="col-md-6">
                            <label htmlFor="numero" className="form-label">Número</label>
                            <input type="text" className="form-control" id="numero" name="numero" value={formData.numero} onChange={handleChange} required />
                            <div className="invalid-feedback"> Por favor, insira o número. </div>
                        </div>

                        <div className="col-md-6">
                            <label htmlFor="complemento" className="form-label">Complemento</label>
                            <input type="text" className="form-control" id="complemento" name="complemento" value={formData.complemento} onChange={handleChange} required />
                            <div className="invalid-feedback"> Por favor, insira o complemento. </div>
                        </div>

                        <div className="col-md-6">
                            <label htmlFor="cep" className="form-label">CEP</label>
                            <IMaskInput mask="00000-000" placeholder="00000-000" className="form-control" name="cep" value={formData.cep} onAccept={(value) => setFormData({ ...formData, cep: value })} required />
                            <div className="invalid-feedback"> Por favor, insira o CEP. </div>
                        </div>

                        <br />
                        <div className="col-md-6">
                            <button type="submit" className="btn btn-primary" disabled={loading}>{loading ? 'Salvando...' : 'Enviar'}</button>
                        </div>
                    </form>
                )}
            </div>
        </div>
    );
};

export default FormularioRevendedor;