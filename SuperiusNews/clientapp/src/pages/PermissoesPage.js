import React from 'react';
import PermissaoTabela from '../components/configuracoes/PermissaoTabela';
import FormularioFuncionalidadePermissao from '../components/configuracoes/FormularioFuncionalidadePermissao';
import FormularioPermissao from '../components/configuracoes/FormularioPermissao';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import { PermissoesPageService } from '../components/configuracoes/PermissoesPageService';
import { Modal } from "react-bootstrap";

const PermissoesPage = () => {
    const { loading, error, success, mensagem, permissoes, modal, editarPermissao, editarPermissaoConfirmar, fecharModal, ativarPermissoes, inativarPermissoes, carregarPermissoes, gerenciarFuncionalidades, gerenciarFuncionalidadeConfirmar, clearMessages } = PermissoesPageService();

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5">
                        <i className="bi bi-people-fill me-2"></i> Permissoes
                    </h1>
                    <PermissaoTabela
                        permissoes={permissoes}
                        loading={loading}
                        onEditarPermissao={editarPermissao}
                        onAtivarPermissoes={ativarPermissoes}
                        onInativarPermissoes={inativarPermissoes}
                        onFuncionalidades={gerenciarFuncionalidades}
                        onRefresh={carregarPermissoes}/>

                    {modal.open && (
                        <Modal show={modal.open} onHide={fecharModal} centered size="md" animation={true}>
                            <Modal.Header closeButton>
                                <Modal.Title> {modal.tipo === 'funcionalidades' ? 'Funcionalidades' : modal.tipo === 'edicao' ? 'Editar Permissao' : 'Nova Permissao'} </Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                {modal.tipo === 'funcionalidades' ? (
                                <FormularioFuncionalidadePermissao
                                    funcionalidadesAtreladas={modal.data.funcionalidadesAtreladas}
                                    funcionalidades={modal.data.funcionalidades}
                                    codigoPermissao={modal.chave}
                                    onSubmit={gerenciarFuncionalidadeConfirmar}
                                    onCancel={fecharModal}/>
                                ) : (
                                <FormularioPermissao
                                    permissao={modal.data}
                                    onSubmit={editarPermissaoConfirmar}
                                    onCancel={fecharModal}/>
                                )}
                            </Modal.Body>
                        </Modal>
                    )}
                </>
            )}
        </div>
    );
};

export default PermissoesPage;