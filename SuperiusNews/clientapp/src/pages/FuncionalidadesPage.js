import React from 'react';
import FuncionalidadeTabela from '../components/configuracoes/FuncionalidadeTabela';
import FormularioFuncionalidade from '../components/configuracoes/FormularioFuncionalidade';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import { FuncionalidadePageService } from '../components/configuracoes/FuncionalidadePageService';
import { Modal } from "react-bootstrap";

const FuncionalidadesPage = () => {
    const { loading, error, success, mensagem, funcionalidades, modal, editarFuncionalidade, editarFuncionalidadeConfirmar, fecharModal, ativarFuncionalidades, inativarFuncionalidades, carregarFuncionalidades, clearMessages } = FuncionalidadePageService();
    
    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5"> <i className="bi bi-people-fill me-2"></i> Funcionalidades </h1>
                    <FuncionalidadeTabela funcionalidades={funcionalidades} loading={loading} onEditarFuncionalidade={editarFuncionalidade} onAtivarFuncionalidades={ativarFuncionalidades} onInativarFuncionalidades={inativarFuncionalidades} onRefresh={carregarFuncionalidades} />

                    <Modal show={modal.open} onHide={fecharModal} centered size="md" animation={true}>
                        <Modal.Header closeButton>
                            <Modal.Title> {modal.tipo === 'edicao' ? 'Editar Funcionalidade' : 'Nova Funcionalidade'} </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <FormularioFuncionalidade funcionalidade={modal.data} onSubmit={editarFuncionalidadeConfirmar} onCancel={fecharModal} />
                        </Modal.Body>
                    </Modal>
                </>

            )}
        </div>
    );
};

export default FuncionalidadesPage;
