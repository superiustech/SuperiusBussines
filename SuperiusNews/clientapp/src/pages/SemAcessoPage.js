import React from 'react';
import { useNavigate } from 'react-router-dom';

const SemAcessoPage = () => {
    const navigate = useNavigate();

    return (
        <div className="d-flex flex-column vh-100 justify-content-center align-items-center bg-body-tertiary px-3">
            <div className="text-center p-5 rounded-4 shadow-lg bg-white" style={{ maxWidth: '500px', width: '100%' }}>
                <div className="mb-4 text-primary">
                    <i className="fas fa-shield-alt fa-3x"></i>
                </div>
                <h2 className="fw-bold mb-3 text-primary">Acesso Negado</h2>
                <p className="text-secondary mb-4">
                    Você não tem permissão para acessar esta página. Verifique com o administrador do sistema.
                </p>
                <button className="btn btn-outline-primary px-4 py-2 rounded-pill fw-semibold" onClick={() => navigate(-1)}>
                    <i className="fas fa-arrow-left me-2"></i>
                    Voltar
                </button>
            </div>
        </div>
    );
};

export default SemAcessoPage;
