import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioPermissaoPerfil = ({ permissoesAtreladas: inicialPermissoesAtreladas, permissoes, codigoPerfil, onSubmit, onCancel }) => {
    const [permissoesGeral, setPermissoesGeral] = useState([]);
    const [permissoesSelecionadas, setPermissoesSelecionadas] = useState([]);
    const handleCheckboxChange = (codigoPermissao) => {
        setPermissoesSelecionadas(prev => {
            if (prev.includes(codigoPermissao)) {
                return prev.filter(codigo => codigo !== codigoPermissao);
            } else {
                return [...prev, codigoPermissao];
            }
        });
    };
    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(codigoPerfil, permissoesSelecionadas);
    };
    useEffect(() => {
        if (permissoes) {
            setPermissoesGeral(permissoes);
        }
        if (inicialPermissoesAtreladas) {
            const codigosAtrelados = inicialPermissoesAtreladas.map(f => f.codigoPermissao);
            setPermissoesSelecionadas(codigosAtrelados);
        }
    }, [permissoes, inicialPermissoesAtreladas]);

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group>
                {permissoesGeral.map(permissao => (
                <Form.Check
                    key={permissao.codigoPermissao}
                    type="checkbox"
                    id={`func-${permissao.codigoPermissao}`}
                    label={permissao.nomePermissao}
                    checked={permissoesSelecionadas.includes(permissao.codigoPermissao)}
                    onChange={() => handleCheckboxChange(permissao.codigoPermissao)}
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

export default FormularioPermissaoPerfil;