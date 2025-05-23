import React from 'react';
import PerfilTabela from '../components/configuracoes/PerfilTabela';
import FormularioPermissaoPerfil from '../components/configuracoes/FormularioPermissaoPerfil';
import FormularioPerfil from '../components/configuracoes/FormularioPerfil';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import { PerfisPageService } from '../components/configuracoes/PerfisPageService';
import { Modal } from "react-bootstrap";

const PerfisPage = () => {
    const { loading, error, success, mensagem, perfis, modal, editarPerfil, editarPerfilConfirmar, fecharModal, ativarPerfis, inativarPerfis, carregarPerfis, gerenciarPerfis, gerenciarPerfilConfirmar, clearMessages } = PerfisPageService();

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5">
                        <i className="bi bi-people-fill me-2"></i> Perfis
                    </h1>
                    <PerfilTabela
                        perfis={perfis}
                        loading={loading}
                        onEditarPerfil={editarPerfil}
                        onAtivarPerfis={ativarPerfis}
                        onInativarPerfis={inativarPerfis}
                        onPerfis={gerenciarPerfis}
                        onRefresh={carregarPerfis} />

                    {modal.open && (
                        <Modal show={modal.open} onHide={fecharModal} centered size="md" animation={true}>
                            <Modal.Header closeButton>
                                <Modal.Title> {modal.tipo === 'permissoes' ? 'Permissões' : modal.tipo === 'edicao' ? 'Editar Perfil' : 'Novo Perfil'} </Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                {modal.tipo === 'permissoes' ? (
                                    <FormularioPermissaoPerfil
                                        permissoesAtreladas={modal.data.permissoesAtreladas}
                                        permissoes={modal.data.permissoes}
                                        codigoPerfil={modal.chave}
                                        onSubmit={gerenciarPerfilConfirmar}
                                        onCancel={fecharModal} />
                                ) : (
                                    <FormularioPerfil
                                        perfil={modal.data}
                                        onSubmit={editarPerfilConfirmar}
                                        onCancel={fecharModal} />
                                )}
                            </Modal.Body>
                        </Modal>
                    )}
                </>
            )}
        </div>
    );
};

export default PerfisPage;