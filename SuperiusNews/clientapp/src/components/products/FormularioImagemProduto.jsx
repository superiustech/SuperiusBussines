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
            const response = await axios.get(`${apiConfig.baseURL}${apiConfig.endpoints.getImages}/` + codigoProduto);
            setImagens(response.data);
        } catch (err) {  console.error('Erro ao carregar imagens:', err); }
    };

    const excluirImagem = async (nCdImagem) => {
        try {
            const response = await axios.delete(`${apiConfig.baseURL}${apiConfig.endpoints.deletarImagens}/` + nCdImagem);
            if (response.data.success) {
                setImagens(imagens.filter(img => img.nCdImagem !== nCdImagem));
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
            formDataToSend.append('nCdProduto', codigoProduto);
            formDataToSend.append('imagem', formData.imagem);
            formDataToSend.append('descricao', formData.descricao);

            const response = await axios.post(`${apiConfig.baseURL}${apiConfig.endpoints.addImage}`, formDataToSend, { headers: { }} );

            if (response.data.success) {
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

                    <input type="hidden" name="nCdProduto" value={codigoProduto} />
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
                                    <tr key={imagem.nCdImagem}>
                                        <th scope="row">{index + 1}</th>
                                        <td> <img src={`/${imagem.sDsCaminho}`} alt={`Imagem ${imagem.nCdImagem}`} style={{ width: '100px', height: 'auto' }} className="img-thumbnail" onError={(e) => { e.target.onerror = null; e.target.src = ''; }} /> </td>
                                        <td>{imagem.sDsImagem}</td>
                                        <td> <button onClick={() => excluirImagem(imagem.nCdImagem)} className="btn btn-sm btn-danger"> Excluir</button> </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="col-md-12 mt-3">
                    <button onClick={() => navigate(`/produto-variacao/${codigoProduto}`)} className="btn btn-secondary me-2"> Voltar </button>
                    <button onClick={() => navigate('/produtos')} className="btn btn-primary" > Finalizar </button>
                </div>
            </div>
        </div>
    );
};

export default FormularioImagemProduto;