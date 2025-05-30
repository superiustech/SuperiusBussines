import { useNavigate } from 'react-router-dom';
import funcionalidades from '../../components/common/Funcionalidades';
import { useAuth } from '../../components/common/AuthContext';

const DashboardResumo = ({ dashboardResumo }) => {
    const navigate = useNavigate();
    const { validarFuncionalidade } = useAuth();

    const configOptions = [
        { label: "Estoques Ativos", icon: "fas fa-warehouse", path: "/administrador/estoques", funcionalidade: funcionalidades.VISUALIZAR_ESTOQUES, data: dashboardResumo.estoquesAtivos },
        { label: "Produtos nos Estoques", icon: "fas fa-boxes", path: "/administrador/estoques", funcionalidade: funcionalidades.VISUALIZAR_ESTOQUES, data: dashboardResumo.produtosCadastrados },
        { label: "Revendedores Ativos", icon: "fas fa-user-tie", path: "/administrador/revendedores", funcionalidade: funcionalidades.VISUALIZAR_REVENDEDORES, data: dashboardResumo.revendedoresAtivos },
        { label: "Média de comissão Revendedores", icon: "fas fa-percentage", path: "#", funcionalidade: funcionalidades.VISUALIZAR_REVENDEDORES, data: dashboardResumo.mediaRevendedores?.toFixed(2) + "%" },
    ];

    return (
        <div className="container-fluid p-0">
            <div className="row g-3">
                {configOptions.map((option, index) => (
                    validarFuncionalidade(option.funcionalidade) && (
                        <div className="col-12 col-sm-6 col-md-4 col-lg-3" key={index}>
                            <div
                                role="button"
                                className="card text-center border-0 shadow-sm rounded-3"
                                onClick={option.path !== '#' ? () => navigate(option.path) : undefined}
                                style={{
                                    cursor: option.path !== '#' ? 'pointer' : 'default',
                                    transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
                                    height: '120px'
                                }}
                                onMouseEnter={e => {
                                    if (option.path !== '#') {
                                        e.currentTarget.style.transform = 'translateY(-3px)';
                                        e.currentTarget.style.boxShadow = '0 8px 16px rgba(0,0,0,0.1)';
                                    }
                                }}
                                onMouseLeave={e => {
                                    if (option.path !== '#') {
                                        e.currentTarget.style.transform = 'translateY(0)';
                                        e.currentTarget.style.boxShadow = '0 4px 6px rgba(0,0,0,0.05)';
                                    }
                                }}>
                                <div className="card-body d-flex flex-column justify-content-center p-3">
                                    <div className="d-flex align-items-center justify-content-start gap-4">
                                        <div className="d-flex align-items-center justify-content-center">
                                            <i
                                                className={`${option.icon} fa-2x text-primary`}
                                                style={{
                                                    minWidth: '40px',
                                                    textAlign: 'center'
                                                }}></i>
                                        </div>
                                        <div className="text-start flex-grow-1">
                                            <h6 className="card-title fw-medium mb-1 text-muted" style={{ fontSize: '1rem' }}>
                                                {option.label}
                                            </h6>
                                            <div className="fs-4 fw-bold text-dark">
                                                {option.data ?? '--'}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )
                ))}
            </div>
        </div>
    );
};

export default DashboardResumo;