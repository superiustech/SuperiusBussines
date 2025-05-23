import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const FormularioFuncionalidadePermissao = ({ funcionalidadesAtreladas: inicialFuncionalidadesAtreladas, funcionalidades, codigoPermissao, onSubmit, onCancel }) => {
    const [funcionalidadesGeral, setFuncionalidadesGeral] = useState([]);
    const [funcionalidadesSelecionadas, setFuncionalidadesSelecionadas] = useState([]);
    const handleCheckboxChange = (codigoFuncionalidade) => {
        setFuncionalidadesSelecionadas(prev => {
            if (prev.includes(codigoFuncionalidade)) {
                return prev.filter(codigo => codigo !== codigoFuncionalidade);
            } else {
                return [...prev, codigoFuncionalidade];
            }
        });
    };
    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(codigoPermissao, funcionalidadesSelecionadas);
    };
    useEffect(() => {
        if (funcionalidades) {
            setFuncionalidadesGeral(funcionalidades);
        }
        if (inicialFuncionalidadesAtreladas) {
            const codigosAtrelados = inicialFuncionalidadesAtreladas.map(f => f.codigoFuncionalidade);
            setFuncionalidadesSelecionadas(codigosAtrelados);
        }
    }, [funcionalidades, inicialFuncionalidadesAtreladas]);

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group>
                {funcionalidadesGeral.map(funcionalidade => (
                <Form.Check
                    key={funcionalidade.codigoFuncionalidade}
                    type="checkbox"
                    id={`func-${funcionalidade.codigoFuncionalidade}`}
                    label={funcionalidade.nomeFuncionalidade}
                    checked={funcionalidadesSelecionadas.includes(funcionalidade.codigoFuncionalidade)}
                    onChange={() => handleCheckboxChange(funcionalidade.codigoFuncionalidade)}
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

export default FormularioFuncionalidadePermissao;