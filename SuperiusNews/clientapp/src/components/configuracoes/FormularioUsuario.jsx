import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioUsuario = ({ usuario, tipo, onSubmit, onCancel }) => {
    const [formData, setFormData] = useState({ usuario: '', nomeUsuario: '', email: '', senha: '' });
    const [showPassword, setShowPassword] = useState(false);
    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };
    useEffect(() => {
        if (usuario) { setFormData(usuario); }
    }, [usuario]);

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group>
                <Form.Label>Código</Form.Label>
                <Form.Control type="text" value={formData.usuario} onChange={e => setFormData({ ...formData, usuario: e.target.value })} disabled={tipo === 'edicao'} />
            </Form.Group>

            <Form.Group>
                <Form.Label>Nome</Form.Label>
                <Form.Control type="text" value={formData.nomeUsuario} onChange={e => setFormData({ ...formData, nomeUsuario: e.target.value })} />
            </Form.Group>

            <Form.Group className="mt-2">
                <Form.Label>E-mail</Form.Label>
                <Form.Control type="text" value={formData.email} onChange={e => setFormData({ ...formData, email: e.target.value })} />
            </Form.Group>

            <Form.Group className="mt-2">
                <Form.Label>Senha</Form.Label>
                <div className="d-flex align-items-center">
                    <Form.Control type={showPassword ? "text" : "password"} value={formData.senha} onChange={e => setFormData({ ...formData, senha: e.target.value })}/>
                    <Button variant="outline-secondary" onClick={() => setShowPassword(!showPassword)} className="ms-2" title={showPassword ? "Ocultar senha" : "Mostrar senha"}>
                        <i className={`fas ${showPassword ? "fa-eye-slash" : "fa-eye"}`}></i>
                    </Button>
                </div>
            </Form.Group>

            <div className="mt-3 d-flex justify-content-end">
                <Button variant="secondary" onClick={onCancel}>Cancelar</Button>
                <Button variant="primary" type="submit" disabled={!formData.nomeUsuario || !formData.email || !formData.senha} className="ms-2">Confirmar</Button>
            </div>
        </Form>
    );
};

export default FormularioUsuario;
