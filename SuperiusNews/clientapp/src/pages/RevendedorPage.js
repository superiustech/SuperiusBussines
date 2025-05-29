import React from 'react';
import RevendedorTable from '../components/revendedor/RevendedorTable';
import FormularioRevendedorUsuario from '../components/revendedor/FormularioRevendedorUsuario';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components//ui/FlashMessage';
import { RevendedorPageService } from '../components/revendedor/RevendedorPageService';
import { Modal } from "react-bootstrap";

const RevendedorPage = () => {
    const { loading, error, success, mensagem, revendedores, modal, editarRevendedor, excluirRevendedor, carregarRevendedores, fecharModal, gerenciarUsuarios, gerenciarUsuariosConfirmar, clearMessages} = RevendedorPageService();

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5"> <i className="bi bi-people-fill me-2"></i> Revendedores </h1>
                    <RevendedorTable revendedores={revendedores} loading={loading} onEditarRevendedor={editarRevendedor} onExcluirRevendedor={excluirRevendedor} onRefresh={carregarRevendedores} onGerenciarUsuarios={gerenciarUsuarios} />

                    {modal.open && (
                        <Modal show={modal.open} onHide={fecharModal} centered size="md" animation={true}>
                            <Modal.Header closeButton>
                                <Modal.Title> {modal.tipo === 'usuarios' && 'Usuários' } </Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                {modal.tipo === 'usuarios' && (<FormularioRevendedorUsuario usuariosAtrelados={modal.data.usuariosAtrelados} usuarios={modal.data.usuarios} revendedor={modal.chave} onSubmit={gerenciarUsuariosConfirmar} onCancel={fecharModal} />)}
                            </Modal.Body>
                        </Modal>
                    )}
                </>
            )}
        </div>
    );
};

export default RevendedorPage;