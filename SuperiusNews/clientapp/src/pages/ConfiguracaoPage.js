import { useNavigate } from 'react-router-dom';

const ConfiguracaoPage = () => {
    const navigate = useNavigate();

    const configOptions = [
        { label: "Usuários", icon: "fas fa-users", path: "/administrador/usuarios" },
        { label: "Funcionalidades", icon: "fas fa-cogs", path: "/administrador/funcionalidades" },
        { label: "Permissões", icon: "fas fa-key", path: "/administrador/permissoes" },
        { label: "Perfis", icon: "fas fa-address-book", path: "/administrador/perfis" },
    ];

    return (
        <div className="container-fluid min-vh-100 d-flex flex-column">
            <h1 className="fw-bold display-5 text-primary m-0 mb-1"> <i className="bi bi-people-fill me-2"></i> Configurações </h1>
            <div className="flex-grow-1 d-flex justify-content-center align-items-center">
                <div className="row g-4 justify-content-center w-100 px-5">
                    {configOptions.map((option, index) => (
                        <div className="col-12 col-sm-6 col-md-4 col-lg-3" key={index}>
                            <div
                                role="button"
                                className="card text-center shadow-sm border-0 h-100"
                                onClick={() => navigate(option.path)}
                                style={{ cursor: 'pointer', transition: 'transform 0.2s ease-in-out' }}
                                onMouseEnter={e => e.currentTarget.style.transform = 'scale(1.05)'}
                                onMouseLeave={e => e.currentTarget.style.transform = 'scale(1)'}>
                                <div className="card-body d-flex flex-column align-items-center justify-content-center py-4">
                                    <i className={`${option.icon} fa-2x text-primary mb-3`}></i>
                                    <h6 className="card-title fw-semibold">{option.label}</h6>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default ConfiguracaoPage;
