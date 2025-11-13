import Loading from '../ui/Loading';
import { useNavigate, useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import apiConfig from '../../Api';
import { IMaskInput } from 'react-imask';
import FormatadorValores from '../common/FormatadorValores';

const FormularioProduto = () => {
    const navigate = useNavigate();
    const {codigoProduto} = useParams();
    const [unidadesMedida, setUnidadesMedida] = useState([]);
    const [loading, setLoading] = useState(false);
    const [validated, setValidated] = useState(false);
    const [formData, setFormData] = useState({ nomeProduto: '', codigoProduto: '', preco: '', precoUnitario: '', descricaoProduto: '', tipoUnidade: '', tagsBusca: '', videoYoutube: '', largura: '', comprimento: '', altura: '', peso: ''});

    const carregarUnidadesMedida = async () => {
        try {
            setLoading(true);
            const response = await apiConfig.produto.axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.unidadeDeMedida}`);
            const data = await response.data.unidade;
            setUnidadesMedida(data);
        } catch (error) {
            console.error('Erro ao buscar unidades de medidas:', error);
            alert('Erro ao carregar as unidades de medidas.');
        } finally {
            setLoading(false);
        }
    };

    const carregarProduto = async (codigo) => {
        try {
            setLoading(true);
            const response = await apiConfig.produto.axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.consultarProduto}/${codigoProduto}`);
            if (response.data) {
                const produto = response.data.produto;
                setFormData({
                    nomeProduto: produto.nome|| '',
                    codigoProduto: produto.codigoProduto || '',
                    preco: produto.valorVenda?.toString().replace(".", ",") || '',
                    precoUnitario: produto.valorUnitario?.toString().replace(".", ",") || '',
                    descricaoProduto: produto.descricao || '',
                    tipoUnidade: produto.codigoUnidadeMedida?.toString() || '',
                    tagsBusca: produto.tagsBusca || '',
                    videoYoutube: produto.urlVideo || '',
                    largura: produto.largura || '',
                    comprimento: produto.comprimento || '',
                    altura: produto.altura || '',
                    peso: produto.peso || ''
                });
            } else {
                alert('Produto não encontrado.');
            }
        } catch (error) {
            console.error('Erro ao carregar produto:', error);
            alert('Erro ao carregar os dados do produto.');
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleBlur = (e) => { const { name, value } = e.target; };
    
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
                codigo: codigoProduto || 0,
                nome: formData.nomeProduto,
                codigoProduto: formData.codigoProduto,
                descricao: formData.descricaoProduto,
                codigoUnidadeMedida: parseInt(formData.tipoUnidade),
                urlVideo: formData.videoYoutube,
                largura: formData.largura,
                comprimento: formData.comprimento,
                altura: formData.altura,
                peso: formData.peso,
                valorVenda: FormatadorValores.converterParaDecimal(formData.preco),
                valorUnitario: FormatadorValores.converterParaDecimal(formData.precoUnitario)
            };

            const response = await apiConfig.produto.axios.post(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.cadastrarProduto}`, dadosEnvio);

            if (response.data.status === 1) {
                navigate(`/administrador/produto-variacao/${response.data.id}`);
            } else {
                alert(response.data.mensagem)
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

    useEffect(() => {
        carregarUnidadesMedida();
        if (codigoProduto) { carregarProduto(codigoProduto); }
    }, [codigoProduto]);

    return (
        <div className="container d-flex justify-content-center">
            <div className="p-4" style={{ maxWidth: '70%', width: '100%' }}>
                {loading ? (<Loading show={true} />) : (
                <form id="formProduto" className={`row g-3 needs-validation ${validated ? 'was-validated' : ''}`} noValidate onSubmit={handleSubmit}>
                    <div className="col-md-10">
                        <label htmlFor="nomeProduto" className="form-label">Nome do Produto</label>
                        <input type="text" className="form-control" id="nomeProduto" name="nomeProduto" value={formData.nomeProduto} onChange={handleChange} required />
                        <div className="valid-feedback"> Nome válido! </div>
                        <div className="invalid-feedback"> Por favor, insira o nome do produto. </div>
                    </div>

                    <div className="col-md-2">
                        <label htmlFor="codigoProduto" className="form-label">Código</label>
                        <div className="input-group has-validation">
                            <span className="input-group-text" id="inputGroupPrepend">#</span>
                            <input type="text" className="form-control" id="codigoProduto" name="codigoProduto" value={formData.codigoProduto} onChange={handleChange} aria-describedby="inputGroupPrepend" required />
                            <div className="invalid-feedback"> Por favor, insira o código do produto. </div>
                        </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="preco" className="form-label">Preço de Venda</label>
                        <div className="input-group has-validation">
                            <span className="input-group-text" id="inputGroupPrepend">R$</span>
                            <IMaskInput mask={Number} radix="," scale={2} thousandsSeparator="." padFractionalZeros={true} normalizeZeros={true} mapToRadix={["."]} className="form-control" id="preco"
                            name="preco" value={formData.preco} onAccept={(value) => handleChange({ target: { name: 'preco', value } })} aria-describedby="inputGroupPrepend" required />
                            <div className="invalid-feedback"> Por favor, insira um valor monetário válido. </div>
                        </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="precoUnitario" className="form-label">Preço unitário</label>
                        <div className="input-group has-validation">
                            <span className="input-group-text" id="inputGroupPrepend">R$</span>
                            <IMaskInput mask={Number} radix="," scale={2} thousandsSeparator="." padFractionalZeros={true} normalizeZeros={true} mapToRadix={["."]} className="form-control" id="precoUnitario"
                            name="precoUnitario" value={formData.precoUnitario} onAccept={(value) => handleChange({ target: { name: 'precoUnitario', value } })} aria-describedby="inputGroupPrepend" required />
                            <div className="invalid-feedback"> Por favor, insira um valor monetário válido. </div>
                        </div>
                    </div>

                    <div className="col-md-12">
                        <label htmlFor="descricaoProduto" className="form-label">Descrição do produto</label>
                        <textarea className="form-control" style={{ maxHeight: '150px' }} id="descricaoProduto" name="descricaoProduto" value={formData.descricaoProduto} onChange={handleChange} required/>
                        <div className="invalid-feedback"> Por favor, insira a descrição do produto. </div>
                    </div>

                    <div className="col-md-4">
                        <label htmlFor="tipoUnidade" className="form-label">Tipo de Unidade</label>
                        <select className="form-select unidade" id="tipoUnidade" name="tipoUnidade" value={formData.tipoUnidade} onChange={handleChange} required >
                            <option selected disabled value="">Escolha...</option>
                            {unidadesMedida.map((item) => ( <option key={item.codigo} value={item.codigo}> {item.descricao} </option> ))}
                        </select>
                        <div className="invalid-feedback"> Por favor, selecione o tipo de unidade. </div>
                    </div>

                    <div className="col-md-8">
                        <label htmlFor="videoYoutube" className="form-label">Vídeo do youtube</label>
                        <input type="text" className="form-control" id="videoYoutube" name="videoYoutube" value={formData.videoYoutube} onChange={handleChange} />
                        <div className="invalid-feedback">Por favor, insira o link do vídeo do YouTube.</div>
                    </div>

                    <br />
                    <h6 className="mt-12">Dimensões da embalagem <span className="badge bg-secondary"><i className="fas fa-gear"></i></span></h6>

                    <div className="col-md-6">
                        <label htmlFor="largura" className="form-label">Largura (cm)</label>
                        <IMaskInput mask={Number} className="form-control" id="largura" name="largura" value={formData.largura} onChange={handleChange} onAccept={(value) => handleChange({ target: { name: 'largura', value } })} required />
                        <div className="invalid-feedback"> Por favor, insira a largura da embalagem. </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="comprimento" className="form-label">Comprimento (cm)</label>
                        <IMaskInput mask={Number} className="form-control" id="comprimento" name="comprimento" value={formData.comprimento} onChange={handleChange} onAccept={(value) => handleChange({ target: { name: 'comprimento', value } })} required />
                        <div className="invalid-feedback"> Por favor, insira o comprimento da embalagem. </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="altura" className="form-label">Altura (cm)</label>
                        <IMaskInput mask={Number} className="form-control" id="altura" name="altura" value={formData.altura} onChange={handleChange} onAccept={(value) => handleChange({ target: { name: 'altura', value } })} required />
                        <div className="invalid-feedback"> Por favor, insira a altura da embalagem. </div>
                    </div>

                    <div className="col-md-6"> <label htmlFor="peso" className="form-label">Peso (kg)</label>
                        <IMaskInput mask={Number} className="form-control" id="peso" name="peso" value={formData.peso} onChange={handleChange} onAccept={(value) => handleChange({ target: { name: 'peso', value } })} required />
                        <div className="invalid-feedback"> Por favor, insira o peso da embalagem. </div>
                    </div>

                    <br />
                    <div className="col-md-6">
                        <button type="submit" className="btn btn-primary" disabled={loading}>Salvar e avançar</button>
                    </div>
                </form>
                )}    
            </div>
        </div>
    );
};

export default FormularioProduto;