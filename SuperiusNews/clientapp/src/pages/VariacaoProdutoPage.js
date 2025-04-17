import { useParams } from 'react-router-dom';
import VariacaoProduto from '../components/products/VariacaoProduto';

const VariacaoProdutoPage = () => {
    const { codigoProduto } = useParams(); 
    return (
        <div className="container">
            <VariacaoProduto codigoProduto={codigoProduto} />
        </div>
    );
};

export default VariacaoProdutoPage;