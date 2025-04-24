import { useNavigate, useParams } from 'react-router-dom';
import { IMaskInput } from 'react-imask';
import React, { useState, useEffect } from 'react';
import apiConfig from '../../Api';
import axios from 'axios';
import FormatadorValores from '../common/FormatadorValores';
import Loading from '../ui/Loading';

const FormularioEstoque = () => {
    const navigate = useNavigate();
    const { codigoEstoque } = useParams();
    const [loading, setLoading] = useState(false);
    const [validated, setValidated] = useState(false);
    const [formData, setFormData] = useState({ nomeEstoque: '', codigoEstoque: '', descricaoEstoque: '', tagsBusca: '', rua: '', complemento: '', numero: '', cep: '' });

    const handleChange = (e) => {
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

        setLoading(true);

        try {
            const dadosEnvio = {
                nCdEstoque: codigoEstoque || 0,
                sNmEstoque: formData.nomeEstoque,
                sCdEstoque: formData.codigoEstoque,
                sDsEstoque: formData.descricaoEstoque,
                sDsRua: formData.rua,
                sDsComplemento: formData.complemento,
                sNrNumero: formData.numero,
                sCdCep: FormatadorValores.removerFormatacao(formData.cep)
            };

            const response = await axios.post(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.cadastrarEstoque}`, dadosEnvio, {
                headers: { 'Content-Type': 'application/json'}
            });

            if (response.data.success) {
                setTimeout(function () { navigate(`/administrador/estoque-produto/${response.data.codigoEstoque}`); }, 3000);
            } else {
                alert(response.data.message);
            }
        } catch (error) {
            if (error.response?.status === 400) {
                const errors = error.response.data.errors;
                alert("Erros de validação: " + errors.join(", "));
            } else {
                alert('Ocorreu um erro ao salvar os dados: ' + error.message);
            }
        } finally {
            setLoading(false);
        }
    };

    const carregarEstoque = async () => {
        try {
            setLoading(true);
            const response = await axios.get(`${apiConfig.estoque.baseURL}${apiConfig.estoque.endpoints.consultarEstoque}/${codigoEstoque}`);
            if (response.data.success) {
                const estoque = response.data.estoque;
                setFormData({
                    nomeEstoque: estoque.sNmEstoque || '',
                    codigoEstoque: estoque.sCdEstoque || '',
                    descricaoEstoque: estoque.sDsEstoque || '',
                    tagsBusca: '',
                    rua: estoque.sDsRua || '',
                    complemento: estoque.sDsComplemento || '',
                    numero: estoque.sNrNumero || '',
                    cep: estoque.sCdCep?.toString() || ''
                });
            } else {
                alert('Estoque não encontrado.');
            }
        } catch (error) {
            console.error('Erro ao carregar Estoque:', error);
            alert('Erro ao carregar os dados do Estoque.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { if (codigoEstoque) { carregarEstoque(); } }, [codigoEstoque]);

    return (
        <div className="container d-flex justify-content-center">
            <div className="p-4" style={{ maxWidth: '70%', width: '100%' }}>
                {loading ? (<Loading show={true} />) : (
                    <form id="formEstoque" className={`row g-3 needs-validation ${validated ? 'was-validated' : ''}`} noValidate onSubmit={handleSubmit}>
                        <div className="col-md-10">
                            <label htmlFor="nomeEstoque" className="form-label">Nome do Estoque</label>
                            <input type="text" className="form-control" id="nomeEstoque" name="nomeEstoque" value={formData.nomeEstoque} onChange={handleChange} required />
                            <div className="valid-feedback"> Nome válido! </div>
                            <div className="invalid-feedback"> Por favor, insira o nome do estoque. </div>
                        </div>

                        <div className="col-md-2">
                            <label htmlFor="codigoEstoque" className="form-label">Código</label>
                            <div className="input-group has-validation">
                                <span className="input-group-text" id="inputGroupPrepend">#</span>
                                <input type="text" className="form-control" id="codigoEstoque" name="codigoEstoque" value={formData.codigoEstoque} onChange={handleChange} aria-describedby="inputGroupPrepend" required />
                                <div className="invalid-feedback"> Por favor, insira o código do estoque. </div>
                            </div>
                        </div>

                        <div className="col-md-12">
                            <label htmlFor="descricaoEstoque" className="form-label">Descrição do estoque</label>
                            <textarea className="form-control" style={{ maxHeight: '150px' }} id="descricaoEstoque" name="descricaoEstoque" value={formData.descricaoEstoque} onChange={handleChange} required />
                            <div className="invalid-feedback"> Por favor, insira a descrição do estoque. </div>
                        </div>

                        <br />

                        <h6 className="mt-12"> Localidade 
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
                            <button type="submit" className="btn btn-primary" disabled={loading}>
                                {loading ? 'Salvando...' : 'Salvar e avançar'}
                            </button>
                        </div>
                    </form>
                )}
            </div>
        </div>
    );
};

export default FormularioEstoque;