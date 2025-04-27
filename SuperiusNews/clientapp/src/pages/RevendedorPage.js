import React from 'react';
import RevendedorTable from '../components/revendedor/RevendedorTable';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components//ui/FlashMessage';
import { RevendedorPageService } from '../components/revendedor/RevendedorPageService';

const RevendedorPage = () => {
    const { loading, error, success, mensagem, revendedores, editarRevendedor, excluirRevendedor, clearMessages, carregarRevendedores } = RevendedorPageService();

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
            <><h1 className="fw-bold display-5 text-primary m-0 mb-5"> <i className="bi bi-people-fill me-2"></i> Revendedores </h1>
            <RevendedorTable revendedores={revendedores} loading={loading} onEditarRevendedor={editarRevendedor} onExcluirRevendedor={excluirRevendedor} onRefresh={carregarRevendedores} /></>
            )}
        </div>
    );
};

export default RevendedorPage;