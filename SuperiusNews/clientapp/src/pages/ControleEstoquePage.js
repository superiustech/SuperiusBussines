import React from 'react';
import { useParams } from 'react-router-dom';
import { Modal } from "react-bootstrap";

import EstoqueProdutoHistoricoTable from '../components/estoque/EstoqueProdutoHistoricoTable';
import EstoqueProdutoTabela from '../components/estoque/EstoqueProdutoTabela';
import FormEntradaEstoque from '../components/estoque/FormEntradaEstoque';
import FormSaidaEstoque from '../components/estoque/FormSaidaEstoque';
import Loading from '../components/ui/Loading';
import FlashMessage from '../components//ui/FlashMessage';
import { ControleEstoquePageService } from '../components/estoque/ControleEstoquePageService';

const ControleEstoquePage = () => {
    const { codigoEstoque } = useParams();
    const { loading, error, success, mensagem, historico, produtos, estoqueProdutos, modal, abrirModal, fecharModal, confirmar, carregarEstoque, deletarProduto, clearMessages } = ControleEstoquePageService(codigoEstoque);

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={5000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={5000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-primary m-0 mb-5"> <i className="bi bi-people-fill me-2"></i> Estoque </h1>

                    <EstoqueProdutoTabela estoqueProdutos={estoqueProdutos} loading={loading} onDeletarProduto={deletarProduto} onEntradaEstoque={() => abrirModal("entrada")} onSaidaEstoque={() => abrirModal("saida")} onRefresh={carregarEstoque} />
                    <EstoqueProdutoHistoricoTable historico={historico} />

                    <Modal show={modal.open} onHide={fecharModal} centered size="md" animation={true}>
                        <Modal.Header closeButton>
                            <Modal.Title> {modal.tipo === "entrada" ? "Entrada de Estoque" : "Saída de Estoque"} </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            {modal.tipo === "entrada" && ( <FormEntradaEstoque produtos={produtos} tipo={modal.tipo} onSubmit={confirmar} onCancel={fecharModal} />)}
                            {modal.tipo === "saida"   && ( <FormSaidaEstoque produtos={produtos} tipo={modal.tipo} onSubmit={confirmar} onCancel={fecharModal} />)}
                        </Modal.Body>
                    </Modal>
                </>
            )}
        </div>
    );
};

export default ControleEstoquePage