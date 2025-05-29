import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const Formulariousuariorevendedor = ({ usuariosAtrelados: inicialusuariosAtrelados, usuarios, revendedor, onSubmit, onCancel }) => {
    const [usuariosGeral, setusuariosGeral] = useState([]);
    const [usuariosSelecionados, setusuariosSelecionados] = useState([]);
    const handleCheckboxChange = (usuario) => {
        setusuariosSelecionados(prev => {
            if (prev.includes(usuario)) {
                return prev.filter(codigo => codigo !== usuario);
            } else {
                return [...prev, usuario];
            }
        });
    };
    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(revendedor, usuariosSelecionados);
    };
    useEffect(() => {
        if (usuarios) {
            setusuariosGeral(usuarios);
        }
        if (inicialusuariosAtrelados) {
            const codigosAtrelados = inicialusuariosAtrelados.map(f => f.usuario);
            setusuariosSelecionados(codigosAtrelados);
        }
    }, [usuarios, inicialusuariosAtrelados]);

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group>
                {usuariosGeral.map(usuario => (
                <Form.Check
                    key={usuario.usuario}
                    type="checkbox"
                    id={`func-${usuario.usuario}`}
                    label={usuario.nomeUsuario}
                    checked={usuariosSelecionados.includes(usuario.usuario)}
                        onChange={() => handleCheckboxChange(usuario.usuario)}
                    className="mb-2"/>
                ))}
            </Form.Group>
            <div className="d-flex justify-content-end mt-3">
                <Button variant="secondary" onClick={onCancel} className="me-2"> Cancelar </Button>
                <Button variant="primary" type="submit"> Salvar </Button>
            </div>
        </Form>
    );
};

export default Formulariousuariorevendedor;