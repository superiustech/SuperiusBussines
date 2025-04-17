import { useParams } from 'react-router-dom';
import FormularioImagemProduto from '../components/products/FormularioImagemProduto';
import TabelaImagensProduto from '../components/products/TabelaImagensProduto';

const ImagemProdutoPage = () => {
    const { codigoProduto } = useParams(); 

    return (
        <div className="container">
            <FormularioImagemProduto codigoProduto={codigoProduto} />
        </div>
    );
};

export default ImagemProdutoPage;