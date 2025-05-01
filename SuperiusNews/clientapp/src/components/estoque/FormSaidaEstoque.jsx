import React, { useState } from "react";
import { Form, Button } from "react-bootstrap";

const FormSaidaEstoque = ({ produtos, tipo, onSubmit, onCancel }) => {
    const [formData, setFormData] = useState({ produtoId: '', quantidade: 0, tipo: tipo });

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group>
                <Form.Label>Produto</Form.Label>
                <Form.Select
                    value={formData.produtoId} onChange={e => setFormData({ ...formData, produtoId: e.target.value })}>
                    <option value="">Selecione...</option>
                    {produtos.map(p => ( <option key={p.nCdProduto} value={p.nCdProduto}>{p.sNmProduto}</option> ))}
                </Form.Select>
            </Form.Group>

            <Form.Group className="mt-2">
                <Form.Label>Quantidade</Form.Label>
                <Form.Control type="number" value={formData.quantidade} onChange={e => setFormData({ ...formData, quantidade: parseInt(e.target.value) })} />
            </Form.Group>

            <div className="mt-3 d-flex justify-content-end">
                <Button variant="secondary" onClick={onCancel}>Cancelar</Button>
                <Button variant="primary" type="submit" disabled={!formData.produtoId || formData.quantidade <= 0 || !formData.quantidade} className="ms-2">Confirmar</Button>
            </div>
        </Form>
    );
};

export default FormSaidaEstoque;
