import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioPerfil = ({ perfil, onSubmit, onCancel }) => {
const [formData, setFormData] = useState({ codigoPerfil: 0, nomePerfil: '', descricaoPerfil: '', ativa: true});
    useEffect(() => {
        if (perfil) { setFormData(perfil); }
    }, [perfil]);

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <Form onSubmit={handleSubmit}>

            <Form.Group>
                <Form.Label>Código</Form.Label>
                <Form.Control type="text" value={formData.codigoPerfil} onChange={e => setFormData({ ...formData, codigoPerfil: e.target.value })} disabled />
            </Form.Group>

            <Form.Group>
                <Form.Label>Nome</Form.Label>
                <Form.Control type="text" value={formData.nomePerfil} onChange={e => setFormData({ ...formData, nomePerfil: e.target.value })}/>
            </Form.Group>

            <Form.Group className="mt-2">
                <Form.Label>Descrição</Form.Label>
                <Form.Control type="text" value={formData.descricaoPerfil} onChange={e => setFormData({ ...formData, descricaoPerfil: e.target.value })}/>
            </Form.Group>

            <div className="mt-3 d-flex justify-content-end">
                <Button variant="secondary" onClick={onCancel}>Cancelar</Button>
                <Button variant="primary" type="submit" disabled={!formData.nomePerfil || !formData.descricaoPerfil} className="ms-2">Confirmar</Button>
            </div>
        </Form>
    );
};

export default FormularioPerfil;
