import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import FlashMessage from '../ui/FlashMessage';
import apiConfig from '../../Api';

const baseUrl = 'api/Administrador';
const FormularioImagemProduto = () => {
    const { codigoProduto } = useParams();
    const navigate = useNavigate();
    const [formData, setFormData] = useState({ descricao: '', imagem: null });
    const [imagens, setImagens] = useState([]);
    const [loading, setLoading] = useState(false);
    const [validated, setValidated] = useState(false);
    const [success, setSuccess] = useState(false);
    const [mensagem, setMensagem] = useState('');
    const [error, setError] = useState(false);

    const handleChange = (e) => {
        if (e.target.name === 'imagem') { setFormData(prev => ({ ...prev, imagem: e.target.files[0] })); }
        else { setFormData(prev => ({ ...prev, [e.target.name]: e.target.value })); }
    };

    const obterImagensProduto = async () => {
        try {
            const response = await apiConfig.produto.axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.obterImagensProduto}/` + codigoProduto);
            setImagens(response.data);
        } catch (err) {  console.error('Erro ao carregar imagens:', err); }
    };

    const excluirImagem = async (codigoImagem) => {
        try {
            const response = await apiConfig.produto.axios.delete(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.excluirImagem}/` + codigoImagem);
            if (response.data.status === 1) {
                setImagens(imagens.filter(img => img.codigoImagem !== codigoImagem));
                setMensagem("Imagem excluída com sucesso!");
                setSuccess(true);
            } else {
                setMensagem("Ocorreu um erro no processamento. Tente novamente.");
                setSuccess(false);
            }
        } catch (err) {
            setMensagem("Ocorreu um erro no processamento. Tente novamente.");
            setSuccess(false);
        }
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
        setSuccess(false);
        setError(false);

        try {
            const formDataToSend = new FormData();
            formDataToSend.append('codigoProduto', codigoProduto);
            formDataToSend.append('imagem', formData.imagem);
            formDataToSend.append('descricao', formData.descricao);

            const response = await apiConfig.produto.axios.post(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.adicionarImagem}`, formDataToSend, { headers: {} });

            if (response.data.status === 1) {
                setMensagem("Imagem adicionada com sucesso!");
                setSuccess(true);
                setFormData({ descricao: '', imagem: null });
                setValidated(false);
                obterImagensProduto();
            } else { setError(true); }
        } catch (err) {setError(true);
        } finally { setLoading(false);}
    };

    useEffect(() => { obterImagensProduto(); }, [codigoProduto]);

    return (
        <div className="container d-flex justify-content-center">
            <div className="p-4" style={{ maxWidth: '100%', width: '100%' }}>
             
                {success && (<FlashMessage message={mensagem} type="success" duration={3000} />)}
                {error && (<FlashMessage message={mensagem} type="error" duration={3000} />)}

                <form id="formImagem" className={`row g-3 needs-validation ${validated ? 'was-validated' : ''}`} noValidate onSubmit={handleSubmit} encType="multipart/form-data">

                    <input type="hidden" name="codigo" value={codigoProduto} />
                    <div className="col-md-12">
                        <label htmlFor="imagem" className="form-label">Selecione a Imagem</label>
                        <input type="file" className="form-control" id="imagem" name="imagem" accept="image/*" onChange={handleChange} required />
                        <div className="invalid-feedback">Por favor, selecione uma imagem.</div>
                    </div>

                    <div className="col-md-12">
                        <label htmlFor="descricao" className="form-label">Descrição da Imagem</label>
                        <input type="text" className="form-control" id="descricao" name="descricao" value={formData.descricao} onChange={handleChange} required />
                        <div className="invalid-feedback">Por favor, insira uma descrição.</div>
                    </div>

                    <div className="col-md-12">
                        <button type="submit" className="btn btn-primary" disabled={loading}> {loading ? 'Enviando...' : 'Adicionar Imagem'}</button>
                    </div>
                </form>
                <hr />

                <div className="mt-4" style={{ maxHeight: '350px', overflow: 'scroll' }}>
                    <label className="form-label">Lista de Imagens</label>
                    <div className="table-responsive">
                        <table className="table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th scope="col">#</th>
                                    <th scope="col">Imagem</th>
                                    <th scope="col">Descrição</th>
                                    <th scope="col">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {imagens.map((imagem, index) => (
                                    <tr key={imagem.codigoImagem}>
                                        <th scope="row">{index + 1}</th>
                                        <td> <img src={`/${imagem.caminho}`} alt={`Imagem ${imagem.codigoImagem}`} style={{ width: '100px', height: 'auto' }} className="img-thumbnail" onError={(e) => { e.target.onerror = null; e.target.src = ''; }} /> </td>
                                        <td>{imagem.descricao}</td>
                                        <td> <button onClick={() => excluirImagem(imagem.codigoImagem)} className="btn btn-sm btn-danger"> Excluir</button> </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="col-md-12 mt-3">
                    <button onClick={() => navigate(`/administrador/produto-variacao/${codigoProduto}`)} className="btn btn-secondary me-2"> Voltar </button>
                    <button onClick={() => navigate('/administrador/produtos')} className="btn btn-primary" > Finalizar </button>
                </div>
            </div>
        </div>
    );
};

export default FormularioImagemProduto;