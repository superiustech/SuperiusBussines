import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioPerfilUsuario = ({ perfisAtrelados: inicialPerfisAtrelados, perfis, usuario, onSubmit, onCancel }) => {
    const [perfisGeral, setPerfisGeral] = useState([]);
    const [perfisSelecionados, setPerfisSelecionados] = useState([]);
    const handleCheckboxChange = (codigoPerfil) => {
        setPerfisSelecionados(prev => {
            if (prev.includes(codigoPerfil)) {
                return prev.filter(codigo => codigo !== codigoPerfil);
            } else {
                return [...prev, codigoPerfil];
            }
        });
    };
    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(usuario, perfisSelecionados);
    };
    useEffect(() => {
        if (perfis) {
            setPerfisGeral(perfis);
        }
        if (inicialPerfisAtrelados) {
            const codigosAtrelados = inicialPerfisAtrelados.map(f => f.codigoPerfil);
            setPerfisSelecionados(codigosAtrelados);
        }
    }, [perfis, inicialPerfisAtrelados]);

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group>
                {perfisGeral.map(perfil => (
                <Form.Check
                    key={perfil.codigoPerfil}
                    type="checkbox"
                    id={`func-${perfil.codigoPerfil}`}
                    label={perfil.nomePerfil}
                    checked={perfisSelecionados.includes(perfil.codigoPerfil)}
                    onChange={() => handleCheckboxChange(perfil.codigoPerfil)}
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

export default FormularioPerfilUsuario;