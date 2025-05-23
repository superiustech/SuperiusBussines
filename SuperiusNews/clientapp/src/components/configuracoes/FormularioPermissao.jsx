import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioPermissao = ({ permissao, onSubmit, onCancel }) => {
const [formData, setFormData] = useState({ codigoPermissao: 0, nomePermissao: '', descricaoPermissao: '', ativa: true});
    useEffect(() => {
        if (permissao) { setFormData(permissao); }
    }, [permissao]);

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <Form onSubmit={handleSubmit}>

            <Form.Group>
                <Form.Label>Código</Form.Label>
                <Form.Control type="text" value={formData.codigoPermissao} onChange={e => setFormData({ ...formData, codigoPermissao: e.target.value })} disabled />
            </Form.Group>

            <Form.Group>
                <Form.Label>Nome</Form.Label>
                <Form.Control type="text" value={formData.nomePermissao} onChange={e => setFormData({ ...formData, nomePermissao: e.target.value })}/>
            </Form.Group>

            <Form.Group className="mt-2">
                <Form.Label>Descrição</Form.Label>
                <Form.Control type="text" value={formData.descricaoPermissao} onChange={e => setFormData({ ...formData, descricaoPermissao: e.target.value })}/>
            </Form.Group>

            <div className="mt-3 d-flex justify-content-end">
                <Button variant="secondary" onClick={onCancel}>Cancelar</Button>
                <Button variant="primary" type="submit" disabled={!formData.nomePermissao || !formData.descricaoPermissao} className="ms-2">Confirmar</Button>
            </div>
        </Form>
    );
};

export default FormularioPermissao;
