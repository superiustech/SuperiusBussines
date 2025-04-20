import React from 'react';
import FormularioRevendedor from '../components/revendedor/FormularioRevendedor';
import { useRevendedor } from '../components/revendedor/useRevendedor';
import { useNavigate, useParams } from 'react-router-dom';

const CadastrarRevendedorPage = () => {
    const { codigoRevendedor } = useParams();
    const { loading, error, success, mensagem, estoques, tipos, revendedor, adicionarRevendedor, clearMessages } = useRevendedor(codigoRevendedor);

    return (
        <div className="container">
            <FormularioRevendedor loading={loading} error={error} success={success} mensagem={mensagem} estoques={estoques} tipos={tipos} revendedor={revendedor} onAdicionarRevendedor={adicionarRevendedor} clearMessages={clearMessages} />
        </div>
    );
};

export default CadastrarRevendedorPage;