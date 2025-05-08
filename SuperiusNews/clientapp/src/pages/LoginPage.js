import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../components/common/AuthContext';
import apiConfig from '../Api';
import axios from 'axios';
import Loading from '../components/ui/Loading';

const LoginPage = () => {
    const [usuario, setUsuario] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const location = useLocation();
    const { login } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {

            const response = await axios.post(
                `${apiConfig.autenticacao.baseURL}${apiConfig.autenticacao.endpoints.token}`,
                {
                    login: usuario,
                    senha: password
                },
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (response.data.token) {
                setLoading(false);
                login(response.data.token);
                const from = location.state?.from?.pathname || '/administrador';
                navigate(from, { replace: true });
            }
            else if (response.data.mensagem) {
                setLoading(false);
                setError('Ocorreu(ram) o(s) seguinte(s) erro(s): ' + response.data.mensagem);
            }
            else {
                setLoading(false);
                setError('Credenciais inválidas');
            }
        } catch (err) {
            setLoading(false);
            setError('Erro ao conectar com o servidor');
        }
    };

    return (
        <div className="min-vh-100 d-flex">
            <div className="d-none d-lg-flex col-lg-6 bg-primary p-5 align-items-center justify-content-center text-white">
                <div className="text-center">
                    <i className="fas fa-warehouse fa-4x mb-4 opacity-75"></i>
                    <h1 className="h3 fw-bold mb-3">Superius Gestão</h1>
                    <p className="lead">Controle total do seu estoque e vendas</p>
                    <p className="small mt-4 opacity-75"><i className="fas fa-chart-line me-2"></i>Relatórios em tempo real</p>
                    <p className="small opacity-75"><i className="fas fa-boxes me-2"></i>Gestão de estoque inteligente</p>
                </div>
            </div>

            <div className="col-12 col-lg-6 d-flex align-items-center justify-content-center p-4">
                <div className="w-100" style={{ maxWidth: '360px' }}>
                    {loading ? (<Loading show={true} />) : (
                        <>
                            <div className="text-center mb-4">
                                <i className="fas fa-lock fa-2x text-primary mb-3"></i>
                                <h2 className="h4 mb-1">Acesso Administrativo</h2>
                                <p className="text-muted small">Gerencie seu negócio com eficiência</p>
                            </div>

                            {error && <div className="alert alert-danger alert-dismissible fade show mb-4 py-2"><small>{error}</small><button type="button" className="btn-close btn-sm" onClick={() => setError('')}></button></div>}

                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label htmlFor="usuario" className="form-label small text-muted">USUÁRIO</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-transparent"><i className="fas fa-user text-muted"></i></span>
                                        <input type="text" id="usuario" className="form-control border-start-0" value={usuario} onChange={(e) => setUsuario(e.target.value)} required placeholder="Digite seu usuário" />
                                    </div>
                                </div>

                                <div className="mb-4">
                                    <label htmlFor="password" className="form-label small text-muted">SENHA</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-transparent"><i className="fas fa-key text-muted"></i></span>
                                        <input type="password" id="password" className="form-control border-start-0" value={password} onChange={(e) => setPassword(e.target.value)} required placeholder="Digite sua senha" />
                                    </div>
                                </div>

                                <button type="submit" className="btn btn-primary w-100 py-2 mb-3">
                                    <i className="fas fa-sign-in-alt me-2"></i>Entrar
                                </button>

                                <div className="text-center small text-muted mt-4">
                                    <p className="mb-1">Problemas para acessar? <a href="#" className="text-decoration-none">Suporte</a></p>
                                    <p className="mt-3"><i className="fas fa-copyright me-1"></i>{new Date().getFullYear()} Superius</p>
                                </div>
                            </form>
                        </>
                    )}
                </div>
            </div>
        </div>
    );

};

export default LoginPage;