import EstoqueProdutoHistoricoTable from '../components/estoque/EstoqueProdutoHistoricoTable';
import EstoqueProdutosChart from '../components/dashboard/EstoqueProdutosChart';
import BotoesRapidos from '../components/dashboard/BotoesRapidos';
import DashboardResumo from '../components/dashboard/DashboardResumo';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import { DashboardService } from '../services/DashboardService';

const DashboardPage = () => {
    const { loading, error, success, mensagem, dashboardResumo, historico, produtosEstoques, clearMessages } = DashboardService();

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary mb-4 px-5"> <i className="bi bi-speedometer2 me-2"></i> Dashboard</h1>
                    <DashboardResumo dashboardResumo={dashboardResumo} />
                    <EstoqueProdutoHistoricoTable historico={historico} />

                    <div className="d-flex gap-4 mt-4">
                        <div style={{ width: '30%' }}>
                            <BotoesRapidos />
                        </div>

                        <div style={{ width: '65%', marginLeft: 'auto' }}>
                            <EstoqueProdutosChart produtosEstoques={produtosEstoques} />
                        </div>
                    </div>
                </>
            )}
        </div>
    );
};

export default DashboardPage;
