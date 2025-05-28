import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from './AuthContext';

const ProtectedRoute = ({ children, funcionalidade }) => {
    const { isAuthenticated, validarFuncionalidade } = useAuth();
    const location = useLocation();

    if (!isAuthenticated()) {
        return <Navigate to="/administrador/login" state={{ from: location }} replace />;
    } else if (funcionalidade && !validarFuncionalidade(funcionalidade)){
        return <Navigate to="/administrador/sem-acesso" state={{ from: location }} replace />;
    }

    return children;
};

export default ProtectedRoute;