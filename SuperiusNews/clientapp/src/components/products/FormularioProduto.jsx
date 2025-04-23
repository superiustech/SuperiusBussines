import Loading from '../ui/Loading';
import { useNavigate, useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import apiConfig from '../../Api';

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
            const response = await axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.unidadeDeMedida}`);
            if (!response.data.success) throw new Error('Erro na resposta da API');
            const data = await response.data.unidade;
            setUnidadesMedida(data);
        } catch (error) {
            console.error('Erro ao buscar unidades de medidas:', error);
            alert('Erro ao carregar as unidades de medidas.');
        } finally {
            setLoading(false);
        }
    };

    const validarValorMonetario = (valor) => { return /^[0-9,.]*$/.test(valor); };

    const formatarValorMonetario = (valor) => {
        if (!valor) return '';
        valor = valor.toString().replace(/[^0-9,]/g, '');
        valor = valor.replace(',', '.');
        if (!valor || valor.trim() === '') return '';
        return parseFloat(valor).toFixed(2).replace('.', ',');
    };

    const carregarProduto = async (codigo) => {
        try {
            setLoading(true);
            const response = await axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.consultarProduto}/${codigoProduto}`);
            if (response.data.success) {
                const produto = response.data.produto;
                setFormData({
                    nomeProduto: produto.sNmProduto || '',
                    codigoProduto: produto.sCdProduto || '',
                    preco: formatarValorMonetario(produto.dVlVenda) || '',
                    precoUnitario: formatarValorMonetario(produto.dVlUnitario) || '',
                    descricaoProduto: produto.sDsProduto || '',
                    tipoUnidade: produto.nCdUnidadeMedida?.toString() || '',
                    tagsBusca: produto.tagsBusca || '',
                    videoYoutube: produto.sUrlVideo || '',
                    largura: produto.sLargura || '',
                    comprimento: produto.sComprimento || '',
                    altura: produto.sAltura || '',
                    peso: produto.sPeso || ''
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
        if (name === 'preco' || name === 'precoUnitario') { if (!validarValorMonetario(value)) return; }
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleBlur = (e) => {
        const { name, value } = e.target;
        if (name === 'preco' || name === 'precoUnitario') { setFormData(prev => ({ ...prev, [name]: formatarValorMonetario(value)})); }
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
                nCdProduto: codigoProduto || 0,
                sNmProduto: formData.nomeProduto,
                sCdProduto: formData.codigoProduto,
                sDsProduto: formData.descricaoProduto,
                nCdUnidadeMedida: parseInt(formData.tipoUnidade),
                sUrlVideo: formData.videoYoutube,
                sLargura: formData.largura,
                sComprimento: formData.comprimento,
                sAltura: formData.altura,
                sPeso: formData.peso,
                dVlVenda: parseFloat(formData.preco.replace(',', '.')),
                dVlUnitario: parseFloat(formData.precoUnitario.replace(',', '.'))
            };
            const response = await axios.post(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.cadastrarProduto}`, dadosEnvio);

            if (response.data.success) {
                navigate(`/administrador/produto-variacao/${response.data.codigoProduto}`);
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
                            <input type="text" className="form-control preco" id="preco" name="preco" value={formData.preco} onChange={handleChange} onBlur={handleBlur} aria-describedby="inputGroupPrepend" required />
                            <div className="invalid-feedback"> Por favor, insira um valor monetário válido. </div>
                        </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="precoUnitario" className="form-label">Preço unitário</label>
                        <div className="input-group has-validation">
                            <span className="input-group-text" id="inputGroupPrepend">R$</span>
                            <input type="text" className="form-control preco" id="precoUnitario" name="precoUnitario" value={formData.precoUnitario} onChange={handleChange} onBlur={handleBlur} aria-describedby="inputGroupPrepend" required />
                            <div className="invalid-feedback"> Por favor, insira um valor monetário válido. </div>
                        </div>
                    </div>

                    <div className="col-md-12">
                        <label htmlFor="descricaoProduto" className="form-label">Descrição do produto</label>
                        <textarea className="form-control" style={{ maxHeight: '150px' }} id="descricaoProduto" name="descricaoProduto" value={formData.descricaoProduto} onChange={handleChange} required/>
                        <div className="invalid-feedback"> Por favor, insira a descrição do produto. </div>
                    </div>

                    <div className="col-md-3">
                        <label htmlFor="tipoUnidade" className="form-label">Tipo de Unidade</label>
                        <select className="form-select unidade" id="tipoUnidade" name="tipoUnidade" value={formData.tipoUnidade} onChange={handleChange} required >
                            <option selected disabled value="">Escolha...</option>
                            {unidadesMedida.map((item) => ( <option key={item.nCdUnidadeMedida} value={item.nCdUnidadeMedida}> {item.sDsUnidadeMedida} </option> ))}
                        </select>
                        <div className="invalid-feedback"> Por favor, selecione o tipo de unidade. </div>
                    </div>

                    <div className="col-md-4">
                        <label htmlFor="tagsBusca" className="form-label">Tags para busca</label>
                        <input type="text" className="form-control" id="tagsBusca" name="tagsBusca" value={formData.tagsBusca} onChange={handleChange}/>
                        <div className="invalid-feedback"> Por favor, insira as tags para busca. </div>
                    </div>

                    <div className="col-md-5">
                        <label htmlFor="videoYoutube" className="form-label">Vídeo do youtube</label>
                        <input type="text" className="form-control" id="videoYoutube" name="videoYoutube" value={formData.videoYoutube} onChange={handleChange} required />
                        <div className="invalid-feedback">Por favor, insira o link do vídeo do YouTube.</div>
                    </div>

                    <br />
                    <h6 className="mt-12">Dimensões da embalagem <span className="badge bg-secondary"><i className="fas fa-gear"></i></span></h6>

                    <div className="col-md-6">
                        <label htmlFor="largura" className="form-label">Largura (cm)</label>
                        <input type="text" className="form-control" id="largura" name="largura" value={formData.largura} onChange={handleChange} required />
                        <div className="invalid-feedback"> Por favor, insira a largura da embalagem. </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="comprimento" className="form-label">Comprimento (cm)</label>
                        <input type="text" className="form-control" id="comprimento" name="comprimento" value={formData.comprimento} onChange={handleChange} required />
                        <div className="invalid-feedback"> Por favor, insira o comprimento da embalagem. </div>
                    </div>

                    <div className="col-md-6">
                        <label htmlFor="altura" className="form-label">Altura (cm)</label>
                        <input type="text"className="form-control" id="altura" name="altura" value={formData.altura} onChange={handleChange} required />
                        <div className="invalid-feedback"> Por favor, insira a altura da embalagem. </div>
                    </div>

                    <div className="col-md-6"> <label htmlFor="peso" className="form-label">Peso (kg)</label>
                        <input type="text" className="form-control" id="peso" name="peso" value={formData.peso} onChange={handleChange} required />
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