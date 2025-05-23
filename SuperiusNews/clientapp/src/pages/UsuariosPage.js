import React from 'react';
import UsuarioTabela from '../components/configuracoes/UsuarioTabela';
import FormularioPerfilUsuario from '../components/configuracoes/FormularioPerfilUsuario';
import FormularioUsuario from '../components/configuracoes/FormularioUsuario';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import { UsuarioPageService } from '../components/configuracoes/UsuarioPageService';
import { Modal } from "react-bootstrap";

const UsuariosPage = () => {
    const { loading, error, success, mensagem, usuarios, modal, editarUsuario, editarUsuarioConfirmar, fecharModal, ativarUsuarios, inativarUsuarios, carregarUsuarios, gerenciarUsuarios, gerenciarUsuarioConfirmar, novoUsuario, novoUsuarioConfirmar, clearMessages } = UsuarioPageService();

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5">
                        <i className="bi bi-people-fill me-2"></i> Usuarios
                    </h1>
                    <UsuarioTabela
                        usuarios={usuarios}
                        loading={loading}
                        onEditarUsuario={editarUsuario}
                        onAtivarUsuarios={ativarUsuarios}
                        onInativarUsuarios={inativarUsuarios}
                        onPerfis={gerenciarUsuarios}
                        onCadastrarUsuario={novoUsuario}
                        onRefresh={carregarUsuarios} />

                    {modal.open && (
                        <Modal show={modal.open} onHide={fecharModal} centered size="md" animation={true}>
                            <Modal.Header closeButton>
                                <Modal.Title> {modal.tipo === 'perfis' ? 'Perfis' : modal.tipo === 'edicao' ? 'Editar Usuario' : 'Novo Usuario'} </Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                {modal.tipo === 'perfis' ? (
                                <FormularioPerfilUsuario
                                    perfisAtrelados={modal.data.perfisAtrelados}
                                    perfis={modal.data.perfis}
                                    usuario={modal.chave}
                                    onSubmit={gerenciarUsuarioConfirmar}
                                    onCancel={fecharModal} />
                                ) : (
                                <FormularioUsuario
                                    usuario={modal.data}
                                    tipo={modal.tipo}
                                    onSubmit={modal.tipo == 'edicao' ? editarUsuarioConfirmar : novoUsuarioConfirmar}
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

export default UsuariosPage;