import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import FlashMessage from '../ui/FlashMessage';
import apiConfig from '../../Api';
import Loading from '../ui/Loading';

const baseUrl = 'api/Administrador';
const VariacaoProduto = () => {
    const { codigoProduto } = useParams();
    const navigate = useNavigate();
    const [mensagem, setMensagem] = useState('');
    const [details, setDetails] = useState('');
    const [variacoes, setVariacoes] = useState([]);
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState(false);
    const [tipoSelecionado, setTipoSelecionado] = useState('');
    const [cardsVariacoes, setCardsVariacoes] = useState([]);
    const [contadorVariacoes, setContadorVariacoes] = useState(0);

    const obterVariacoesProduto = async () => {
        try {
            const response = await apiConfig.produto.axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.consultarVariacoesProduto}/` + codigoProduto);
            if (response.data && response.data.length > 0) {
                const novoContador = contadorVariacoes + 1;
                setContadorVariacoes(novoContador);
                const novosCards = response.data.map((variacao) => {
                    return {
                        id: `card_${novoContador}_${variacao.codigo}`,  
                        headerId: `heading_${novoContador}_${variacao.codigo}`,
                        collapseId: `collapse_${novoContador}_${variacao.codigo}`,
                        tipo: variacao.nome,
                        tipoId: variacao.codigo, 
                        opcoes: variacao.opcoes.map((item, index) => ({
                            id: `variacao_${novoContador}_${index}_${variacao.codigo}`,
                            valor: `${item.codigo}|${variacao.codigo}`,
                            nome: item.nome,
                            atrelado: item?.atrelado ?? false
                        }))
                    };
                });

                setCardsVariacoes([...cardsVariacoes, ...novosCards]); 
            }
        } catch (err) { console.error('Erro ao carregar variações do produto:', err); }
    };

    const obterVariacoes = async () => {
        try {
            const response = await apiConfig.produto.axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.tipoVariacao}`);
            setVariacoes(response.data);
        } catch (err) { console.error('Erro ao carregar variações:', err); }
    };

    const adicionarVariacao = async () => {
        if (!tipoSelecionado) { alert('Selecione um tipo de variação.'); return; }
        try {
            const novoContador = contadorVariacoes + 1;
            setContadorVariacoes(novoContador);
            const variacaoSelecionada = variacoes.find(v => v.codigo == tipoSelecionado);
            const tipoNome = variacaoSelecionada?.nome || 'Variação';
            const novoCard = {
                id: `card_${novoContador}`,
                headerId: `heading_${novoContador}`,
                collapseId: `collapse_${novoContador}`,
                tipo: tipoNome,
                tipoId: tipoSelecionado,
                opcoes: variacaoSelecionada.opcoes.map((item, index) => ({
                    id: `variacao_${novoContador}_${index}`,
                    valor: `${item.codigo}|${variacaoSelecionada.codigo}`,
                    nome: item.nome,
                    atrelado: false
                }))
            };
            setCardsVariacoes([...cardsVariacoes, novoCard]);
        } catch (error) {
            console.error('Erro ao buscar variações:', error);
            alert('Erro ao carregar as variações.');
        }
    };

    const removerVariacao = (cardId) => { setCardsVariacoes(cardsVariacoes.filter(card => card.id !== cardId)); };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const form = e.currentTarget;

        setLoading(true);
        setSuccess(false);
        setError(false);

        try {
            const variacoesParaEnvio = [];
            for (const card of cardsVariacoes) {
                const checkedInputs = document.querySelectorAll(`input[name="variacao_${card.id}"]:checked`);
                if (checkedInputs.length === 0) continue;

                const variacao = {
                    codigo: card.codigo || card.tipoId,
                    nome: card.tipo || `Variação ${card.id}`,
                    descricao: `Descrição da variação ${card.id}`,
                    ativa: true,
                    opcoes: []
                };

                checkedInputs.forEach(input => {
                    const [codigoOpcao, codigo] = input.value.split('|');
                    variacao.opcoes.push({
                        codigo: parseInt(codigoOpcao, 10),
                        nome: input.nextElementSibling.textContent.trim() || `Opção ${codigoOpcao}`,
                        descricao: `Descrição da opção ${codigoOpcao}`,
                        ativa: true
                    });
                });

                variacoesParaEnvio.push(variacao);
            }

            if (variacoesParaEnvio.length === 0) {
                throw new Error("Selecione pelo menos uma opção de variação");
            }

            const response = await apiConfig.produto.axios.put(
                `${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.editarVariacaoProduto}`,
                { variacoes: variacoesParaEnvio, codigo: codigoProduto},
                { headers: { 'Content-Type': 'application/json' }}
            );

            if (response.data) {
                setSuccess(true);
                setMensagem("Variações atualizadas com sucesso!");
                setDetails("Redirecionando...");
                setTimeout(function () {
                    navigate(`/administrador/produto-imagem/${codigoProduto}`);
                }, 4000);
                
            } else {
                setSuccess(false);
                setError(true);
                setMensagem(error.message || 'Ocorreu um erro ao salvar os dados');
            }
        } catch (error) {
            setError(true);
            setSuccess(false);

            if (error.response?.status === 400) {
                setError(true);
                setSuccess(false);
                setMensagem("Erros de validação:");
            } else {
                setError(true);
                setSuccess(false);
                setMensagem(error.message || 'Ocorreu um erro ao salvar os dados');
            }
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        obterVariacoesProduto();
        obterVariacoes();
    }, [codigoProduto]);

    return (
        <div className="container">
            <div className="p-4" style={{ maxWidth: '70%', width: '100%', margin: '0 auto' }}>
                {success && (<FlashMessage message={mensagem} details={details} type="success" duration={3000} />)}
                {error && (<FlashMessage message={mensagem} type="error" duration={3000} />)}
                {loading ? (<Loading show={true} />) : (
                    <form className="row g-3 needs-validation" noValidate onSubmit={handleSubmit}>
                        <div className="col-md-10">
                            <label htmlFor="tipoVariacao" className="form-label">Tipo de Variação</label>
                            <select className="form-select tipo-variacao" id="tipoVariacao" name="tipoVariacao" value={tipoSelecionado} onChange={(e) => setTipoSelecionado(e.target.value)} required >
                                <option value="" disabled>Escolha...</option>
                                {variacoes.map((item) => (<option key={item.codigo} value={item.codigo}> {item.nome} </option>))}
                            </select>
                            <div className="invalid-feedback"> Por favor, selecione um tipo de variação.</div>
                        </div>
                        <div className="col-md-2 d-flex align-items-end">
                            <button type="button" id="btnAdicionarVariacao" onClick={adicionarVariacao} className="btn btn-primary" disabled={!tipoSelecionado}> <i className="fa-solid fa-plus"></i></button>
                        </div>

                        <div className="variacao mt-3">
                            {cardsVariacoes.map((card) => (
                                <div className="card mt-2" key={card.id} id={card.id}>
                                    <div className="card-header d-flex justify-content-between align-items-center" id={card.headerId}>
                                        <h5 className="mb-0">
                                            <button className="btn btn-link text-decoration-none text-dark" type="button" data-bs-toggle="collapse" data-bs-target={`#${card.collapseId}`} aria-expanded="true" aria-controls={card.collapseId}>Variação: {card.tipo}</button>
                                        </h5>
                                        <button className="btn btn-danger btn-sm" onClick={() => removerVariacao(card.id)} type="button" > <i className="fa-solid fa-trash"></i></button>
                                    </div>
                                    <div id={card.collapseId} className="collapse show" aria-labelledby={card.headerId}>
                                        <div className="card-body">
                                            {card.opcoes.map((opcao) => (
                                                <div className="form-check" key={opcao.id}>
                                                    <input type="checkbox" className="form-check-input" name={`variacao_${card.id}`} value={opcao.valor} id={opcao.id} checked={opcao.atrelado} onChange={(e) => {
                                                        const updatedCards = cardsVariacoes.map(cardItem => {
                                                            if (cardItem.id === card.id) { return { ...cardItem, opcoes: cardItem.opcoes.map(opt => opt.id === opcao.id ? { ...opt, atrelado: e.target.checked } : opt), }; }
                                                            return cardItem;
                                                        });
                                                        setCardsVariacoes(updatedCards);
                                                    }} required />
                                                    <label className="form-check-label" htmlFor={opcao.id}>{opcao.nome}</label>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>

                        <div className="col-md-12 mt-3">
                            <button type="button" id="btnVoltar" className="btn btn-secondary me-2"> Voltar </button>
                            <button type="submit" className="btn btn-primary" disabled={loading}> Salvar e avançar </button>
                        </div>
                    </form>
                )}
            </div>
        </div>
    );
};

export default VariacaoProduto;