import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioFuncionalidade = ({ funcionalidade, onSubmit, onCancel }) => {
const [formData, setFormData] = useState({ codigoFuncionalidade: 0, nomeFuncionalidade: '', descricaoFuncionalidade: '', ativa: true});
    useEffect(() => {
        if (funcionalidade) { setFormData(funcionalidade); }
    }, [funcionalidade]);

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <Form onSubmit={handleSubmit}>

            <Form.Group>
                <Form.Label>Código</Form.Label>
                <Form.Control type="text" value={formData.codigoFuncionalidade} onChange={e => setFormData({ ...formData, codigoFuncionalidade: e.target.value })} disabled />
            </Form.Group>

            <Form.Group>
                <Form.Label>Nome</Form.Label>
                <Form.Control type="text" value={formData.nomeFuncionalidade} onChange={e => setFormData({ ...formData, nomeFuncionalidade: e.target.value })}/>
            </Form.Group>

            <Form.Group className="mt-2">
                <Form.Label>Descrição</Form.Label>
                <Form.Control type="text" value={formData.descricaoFuncionalidade} onChange={e => setFormData({ ...formData, descricaoFuncionalidade: e.target.value })}/>
            </Form.Group>

            <div className="mt-3 d-flex justify-content-end">
                <Button variant="secondary" onClick={onCancel}>Cancelar</Button>
                <Button variant="primary" type="submit" disabled={!formData.nomeFuncionalidade || !formData.descricaoFuncionalidade} className="ms-2">Confirmar</Button>
            </div>
        </Form>
    );
};

export default FormularioFuncionalidade;
