import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import FlashMessage from '../ui/FlashMessage';
import apiConfig from '../../Api';

const baseUrl = 'api/Administrador';
const TabelaImagensProduto = () => {
    const { codigoProduto } = useParams();
    const navigate = useNavigate();
    const [imagens, setImagens] = useState([]);

    const obterImagensProduto = async () => {
        try {
            const response = await axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.getImages}/` + codigoProduto);
            setImagens(response.data);
        } catch (err) {
            console.error('Erro ao carregar imagens:', err);
            alert("Ocorreu um erro ao carregar as imagens");
        }
    };

    const excluirImagem = async (nCdImagem) => {
        try {
            const response = await axios.delete(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.deletarImagens}/` + codigoProduto);
            if (response.data.success) {
                setImagens(imagens.filter(img => img.nCdImagem !== nCdImagem));
            } else {
                alert(response.data.message || 'Erro ao excluir imagem.');
            }
        } catch (err) {
            console.error('Erro ao excluir imagem:', err);
            alert('Erro ao excluir imagem.');
        }
    };

    useEffect(() => { obterImagensProduto(); }, [codigoProduto]);

    return (
        <div className="container d-flex justify-content-center">
            <div className="p-4" style={{ maxWidth: '100%', width: '100%' }}>
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
                                        <th scope="row"> {index + 1} </th>
                                        <td> <img src={`/${imagem.sDsCaminho}`} alt={`Imagem ${imagem.nCdImagem}`} style={{ width: '100px', height: 'auto' }} className="img-thumbnail" onError={(e) => { e.target.onerror = null; e.target.src = ''; }} /></td>
                                        <td>{imagem.sDsImagem}</td>
                                        <td> <button onClick={() => excluirImagem(imagem.nCdImagem)} className="btn btn-sm btn-danger"> Excluir</button> </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className="col-md-12 mt-3">
                    <button onClick={() => navigate(`/administrador/editar-variacoes${codigoProduto}`)} className="btn btn-secondary me-2"> Voltar </button>
                    <button onClick={() => navigate('/administrador/produtos')} className="btn btn-primary" > Finalizar </button>
                </div>
            </div>
            <hr/>
        </div>
    );
};

export default TabelaImagensProduto;